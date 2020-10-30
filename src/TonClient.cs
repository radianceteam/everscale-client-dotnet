using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TonSdk
{
    public partial class TonClient : ITonClient
    {
        private uint _context;
        private volatile bool _initialized;
        internal readonly TonClientConfig Config;

        internal ILogger Logger => Config.Logger ?? DummyLogger.Instance;

        public static ITonClient Create(TonClientConfig config = default)
        {
            var client = new TonClient(config ?? new TonClientConfig());
            client.Init();
            return client;
        }

        private TonClient(TonClientConfig config)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
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
                Interop.tc_bridge_destroy_context(_context);
            }
        }

        public async Task<T> CallFunctionAsync<T>(string functionName, object @params = null)
        {
            var result = await GetJsonResponse(functionName, @params);
            return Deserialize<T>(result);
        }

        public async Task CallFunctionAsync(string functionName, object @params = null)
        {
            await GetJsonResponse(functionName, @params);
        }

        private async Task<string> GetJsonResponse(string functionName, object @params)
        {
            var functionParamsJson = @params != null
                ? Serialize(@params)
                : "";

            Logger.Debug($"Calling function {functionName} with parameters {functionParamsJson}");

            // Two GCHandles to store references to the native callback handlers.
            // This is to avoid native handlers being garbage collected while waiting
            // for result from the native lib.

            var successCallbackHandle = default(GCHandle);
            var errorCallbackHandle = default(GCHandle);

            var tcs = new TaskCompletionSource<string>();

            void SuccessHandler(IntPtr jsonPtr, int len)
            {
                try
                {
                    var json = Utf8String.ToString(jsonPtr, len);
                    Logger.Debug($"{functionName} executed successfully");
                    Logger.Debug($"JSON returned by {functionName}: {json}");
                    tcs.SetResult(json);
                }
                finally
                {
                    if (successCallbackHandle.IsAllocated)
                    {
                        successCallbackHandle.Free();
                    }
                }
            }

            void ErrorHandler(IntPtr jsonPtr, int len)
            {
                try
                {
                    var json = Utf8String.ToString(jsonPtr, len);
                    Logger.Debug($"{functionName} executed with error");
                    Logger.Debug($"Error JSON returned by {functionName}: {json}");
                    tcs.SetException(TonClientException.FromJson(json));
                }
                finally
                {
                    if (errorCallbackHandle.IsAllocated)
                    {
                        errorCallbackHandle.Free();
                    }
                }
            }

            successCallbackHandle = GCHandle.Alloc((Interop.tc_bridge_response_handler_t)SuccessHandler);
            errorCallbackHandle = GCHandle.Alloc((Interop.tc_bridge_response_handler_t)ErrorHandler);

            var funcNameStr = new Utf8String(functionName);
            var funcParamsStr = new Utf8String(functionParamsJson);

            Interop.tc_bridge_request(_context,
                funcNameStr.Ptr,
                funcNameStr.Length,
                funcParamsStr.Ptr,
                funcParamsStr.Length,
                SuccessHandler,
                ErrorHandler);

            var result = await tcs.Task;
            return result;
        }

        private uint CreateContext()
        {
            Logger.Debug("Init context");

            uint context = 0;
            Exception exception = null;

            void NativeCallback(IntPtr jsonPtr, int len)
            {
                try
                {
                    if (jsonPtr == IntPtr.Zero)
                    {
                        Logger.Error("Init context returned null");
                        exception = new TonClientException($"{nameof(Interop.tc_bridge_create_context)} returned null");
                    }
                    else
                    {
                        var json = Utf8String.ToString(jsonPtr, len);
                        Logger.Debug($"Init context returned JSON: {json}");
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
                                exception = TonClientException.FromJson(errorToken.Value<JToken>());
                            }
                            else
                            {
                                Logger.Debug($"throwing exception with the returned JSON: {json}");
                                exception = TonClientException.FromJson(json);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    exception = e;
                }
            }

            var configStr = new Utf8String(JsonConvert.SerializeObject(new
            {
                // TODO: fill?
            }));

            Interop.tc_bridge_create_context(configStr.Ptr, configStr.Length, NativeCallback);

            if (exception != null)
            {
                throw exception;
            }

            return context;
        }

        private T Deserialize<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                Logger.Warning("Empty JSON passed to deserialize method");
                return default;
            }
            return JsonConvert.DeserializeObject<T>(json);
        }

        private string Serialize(object any)
        {
            if (any == null)
            {
                Logger.Warning("Null passed to serialize method");
            }
            return JsonConvert.SerializeObject(any);
        }
    }

    internal class Utf8String : IDisposable
    {
        private readonly string _str;

        public IntPtr Ptr { get; }
        public byte[] Bytes { get; }
        public int Length => Bytes.Length;

        public Utf8String(string str)
        {
            _str = str;
            Bytes = Encoding.UTF8.GetBytes(str);
            Ptr = Marshal.AllocHGlobal(Bytes.Length);
            Marshal.Copy(Bytes, 0, Ptr, Bytes.Length);
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(Ptr);
        }

        public override string ToString()
        {
            return _str;
        }

        public static string ToString(IntPtr ptr, int len)
        {
            var bytes = new byte[len];
            Marshal.Copy(ptr, bytes, 0, bytes.Length);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
