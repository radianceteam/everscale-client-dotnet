using Newtonsoft.Json.Linq;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace TonSdk
{
    public partial class TonClient : ITonClient
    {
        private uint _context;
        private volatile bool _initialized;
        private readonly TonSerializer _serializer;
        internal readonly object Config;

        internal ILogger Logger { get; }

        public static ITonClient Create(ILogger logger = null)
        {
            return Create(new { }, logger);
        }

        public static ITonClient Create(object config, ILogger logger = null)
        {
            var client = new TonClient(config, logger);
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
                Interop.tc_bridge_destroy_context(_context);
            }
        }

        public async Task<T> CallFunctionAsync<T>(string functionName, object @params = null)
        {
            var result = await GetJsonResponse(functionName, @params);
            return _serializer.Deserialize<T>(result);
        }

        public async Task CallFunctionAsync(string functionName, object @params = null)
        {
            await GetJsonResponse(functionName, @params);
        }

        private async Task<string> GetJsonResponse(string functionName, object @params)
        {
            var functionParamsJson = @params != null
                ? _serializer.Serialize(@params)
                : "";

            Logger.Debug($"Calling function {functionName} with parameters {functionParamsJson}");

            // Two GCHandles to store references to the native callback handlers.
            // This is to avoid native handlers being garbage collected while waiting
            // for result from the native lib.

            var successCallbackHandle = default(GCHandle);
            var errorCallbackHandle = default(GCHandle);
            var customCallbackHandle = default(GCHandle);

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


            void CustomHandler(IntPtr jsonPtr, int len)
            {
                try
                {
                    var json = Utf8String.ToString(jsonPtr, len);
                    Logger.Debug($"{functionName} fired custom callback with JSON {json}");
                    // TODO: use it somehow??
                }
                finally
                {
                    if (customCallbackHandle.IsAllocated)
                    {
                        customCallbackHandle.Free();
                    }
                }
            }

            successCallbackHandle = GCHandle.Alloc((Interop.tc_bridge_response_handler_t)SuccessHandler);
            errorCallbackHandle = GCHandle.Alloc((Interop.tc_bridge_response_handler_t)ErrorHandler);
            customCallbackHandle = GCHandle.Alloc((Interop.tc_bridge_response_handler_t)CustomHandler);

            var funcNameStr = new Utf8String(functionName);
            var funcParamsStr = new Utf8String(functionParamsJson);

            Interop.tc_bridge_request(_context,
                funcNameStr.Ptr,
                funcNameStr.Length,
                funcParamsStr.Ptr,
                funcParamsStr.Length,
                SuccessHandler,
                ErrorHandler,
                CustomHandler);

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

            var configStr = new Utf8String(_serializer.Serialize(Config));
            Interop.tc_bridge_create_context(configStr.Ptr, configStr.Length, NativeCallback);

            if (exception != null)
            {
                throw exception;
            }

            return context;
        }
    }
}
