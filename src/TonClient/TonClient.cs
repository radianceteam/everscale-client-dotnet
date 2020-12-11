using Newtonsoft.Json.Linq;
using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TonSdk.Modules;

namespace TonSdk
{
    public partial class TonClient : ITonClient
    {
        private uint _context;
        private volatile bool _initialized;
        private readonly TonSerializer _serializer;
        internal readonly object Config;

        public const int DefaultAbiVersion = 2;

        internal ILogger Logger { get; }

        public static ITonClient Create(ILogger logger = null)
        {
            return Create(null, logger);
        }

        public static ITonClient Create(object config, ILogger logger = null)
        {
            var client = new TonClient(config ?? new { }, logger);
            client.Init();
            return client;
        }

        private TonClient(object config, ILogger logger)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            Logger = logger ?? DummyLogger.Instance;
            _serializer = new TonSerializer(Logger);
        }

        private void Init()
        {
            _context = CreateContext();
            _initialized = true;
        }

        public void Dispose()
        {
            if (_initialized)
            {
                Interop.tc_destroy_context(_context);
            }
        }

        public object Clone()
        {
            return Create(Config, Logger);
        }

        public async Task<T> CallFunctionAsync<T>(string functionName, object @params = null)
        {
            var result = await GetJsonResponse<T>(functionName, @params).ConfigureAwait(false);
            return _serializer.Deserialize<T>(result);
        }

        public async Task CallFunctionAsync(string functionName, object @params = null)
        {
            await GetJsonResponse<string>(functionName, @params).ConfigureAwait(false);
        }

        public async Task<T> CallFunctionAsync<T, TC>(string functionName, object @params, Func<TC, int, Task> callback)
        {
            var result = await GetJsonResponse(functionName, @params, callback).ConfigureAwait(false);
            return _serializer.Deserialize<T>(result);
        }

        public async Task CallFunctionAsync<TC>(string functionName, object @params, Func<TC, int, Task> callback)
        {
            await GetJsonResponse(functionName, @params, callback).ConfigureAwait(false);
        }

        public async Task<T> CallFunctionAsync<T, TP, TR>(string functionName, object @params, Func<TP, Task<TR>> f)
        {
            return await CallFunctionAsync<T, JToken>(functionName, @params, CreateCallback(f))
                .ConfigureAwait(false);
        }

        public Task<T> CallFunctionAsync<T, TP, TR>(string functionName, Func<TP, Task<TR>> f)
        {
            return CallFunctionAsync<T, TP, TR>(functionName, null, f);
        }

        public async Task CallFunctionAsync<TP, TR>(string functionName, object @params, Func<TP, Task<TR>> f)
        {
            await GetJsonResponse(functionName, @params, CreateCallback(f))
                .ConfigureAwait(false);
        }

        public Task CallFunctionAsync<TP, TR>(string functionName, Func<TP, Task<TR>> f)
        {
            return CallFunctionAsync(functionName, null, f);
        }

        private Func<JToken, int, Task> CreateCallback<TP, TR>(Func<TP, Task<TR>> f)
        {
            return async (token, responseType) =>
            {
                TP data;
                switch (responseType)
                {
                    case (int)Interop.tc_response_types_t.tc_response_app_request:
                        var appRequest = _serializer.Deserialize<ParamsOfAppRequest>(token);
                        try
                        {
                            data = _serializer.Deserialize<TP>(appRequest.RequestData);
                            if (data == null)
                            {
                                throw new SerializationException($"{appRequest.RequestData} deserialized to null");
                            }
                            var result = await f(data).ConfigureAwait(false);
                            await Client.ResolveAppRequestAsync(new ParamsOfResolveAppRequest
                            {
                                AppRequestId = appRequest.AppRequestId,
                                Result = new AppRequestResult.Ok
                                {
                                    Result = _serializer.SerializeToken(result)
                                }
                            }).ConfigureAwait(false);
                        }
                        catch (Exception e)
                        {
                            Logger.Error($"Failed to process request {appRequest.AppRequestId} callback ({appRequest.RequestData})", e);
                            try
                            {
                                await Client.ResolveAppRequestAsync(new ParamsOfResolveAppRequest
                                {
                                    AppRequestId = appRequest.AppRequestId,
                                    Result = new AppRequestResult.Error
                                    {
                                        Text = e.Message
                                    }
                                }).ConfigureAwait(false);
                            }
                            catch (Exception e2)
                            {
                                Logger.Error($"Failed to send error result for app request {appRequest.AppRequestId}", e2);
                            }
                        }
                        break;

                    case (int)Interop.tc_response_types_t.tc_response_app_notify:
                        data = _serializer.Deserialize<TP>(token);
                        if (data == null)
                        {
                            Logger.Warning($"{token} deserialized to null while processing app notification");
                            return;
                        }
                        await f(data).ConfigureAwait(false);
                        break;
                }
            };
        }

        private async Task<string> GetJsonResponse<TC>(string functionName, object @params, Func<TC, int, Task> callback = null)
        {
            var functionParamsJson = @params != null
                ? _serializer.Serialize(@params)
                : "";

            Logger.Debug($"Calling function {functionName} with parameters {functionParamsJson}");

            // GCHandle to store reference to the native callback handler.
            // This is to avoid native handler being garbage collected while waiting
            // for result from the native lib.

            var callbackHandle = default(GCHandle);

            var tcs = new TaskCompletionSource<string>();

            var handler = new Interop.tc_response_handler_t((requestId, json_str, type, finished) =>
            {
                try
                {
                    var json = json_str.ToString();
                    Logger.Debug($"{functionName} status update: {type} ({json})");
                    if (type == (int)Interop.tc_response_types_t.tc_response_success)
                    {
                        tcs.SetResult(json);
                    }
                    else if (type == (int)Interop.tc_response_types_t.tc_response_error)
                    {
                        tcs.SetException(TonClientException.FromJson(json));
                    }
                    else if (type == (int)Interop.tc_response_types_t.tc_response_nop)
                    {
                        // TODO: ???
                    }
                    else
                    {
                        if (callback != null)
                        {
                            var value = _serializer.Deserialize<TC>(json);
                            callback.Invoke(value, (int)type); // TODO: call Wait here?
                        }
                    }
                }
                finally
                {
                    if (finished && callbackHandle.IsAllocated)
                    {
                        callbackHandle.Free();
                    }
                }
            });

            callbackHandle = GCHandle.Alloc(handler);

            using (var fNameStr = new TonString(functionName))
            {
                using (var fParamsStr = new TonString(functionParamsJson))
                {
                    Interop.tc_request(_context, fNameStr.ToStruct(), fParamsStr.ToStruct(), 1, handler);
                }
            }

            return await tcs.Task;
        }

        private uint CreateContext()
        {
            Logger.Debug("Init context");

            uint context = 0;

            using (var configStr = new TonString(_serializer.Serialize(Config)))
            {
                var result = Interop.tc_create_context(configStr.ToStruct());
                if (result == IntPtr.Zero)
                {
                    Logger.Error("Init context returned null");
                    throw new TonClientException($"{nameof(Interop.tc_create_context)} returned null");
                }

                var json = Interop.tc_read_string(result).ToString();
                Logger.Debug($"Init context returned JSON: {json}");
                Interop.tc_destroy_string(result);

                var token = JObject.Parse(json);
                if (token.TryGetValue("result", out var contextToken))
                {
                    Logger.Debug($"Init context succeeded: {contextToken}");
                    context = contextToken.Value<uint>();
                }
                else
                {
                    if (token.TryGetValue("error", out var errorToken))
                    {
                        Logger.Debug($"throwing exception with error {errorToken}");
                        throw TonClientException.FromJson(errorToken.Value<JToken>());
                    }
                    else
                    {
                        Logger.Debug($"throwing exception with the returned JSON: {json}");
                        throw TonClientException.FromJson(json);
                    }
                }
            }

            return context;
        }
    }

    internal class TonString : IDisposable
    {
        private readonly string _str;
        private readonly IntPtr _content;
        private readonly uint _len;

        public TonString(string str)
        {
            _str = str;
            var bytes = Encoding.UTF8.GetBytes(str);
            var length = bytes.Length;
            _content = Marshal.AllocHGlobal(length);
            Marshal.Copy(bytes, 0, _content, length);
            _len = (uint)length;
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(_content);
        }

        public Interop.tc_string_data_t ToStruct()
        {
            return new Interop.tc_string_data_t
            {
                content = _content,
                len = _len
            };
        }

        public override string ToString()
        {
            return _str;
        }
    }
}
