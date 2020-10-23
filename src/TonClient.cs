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

        public static async Task<ITonClient> CreateAsync(TonClientConfig config = default)
        {
            var client = new TonClient(config ?? new TonClientConfig());
            await client.InitAsync().ConfigureAwait(false);
            return client;
        }

        private TonClient(TonClientConfig config)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
        }

        private async Task InitAsync()
        {
            if (_initialized)
            {
                return;
            }
            _context = await CreateContextAsync();
            _initialized = true;
        }

        public void Dispose()
        {
            if (_initialized)
            {
                Interop.tc_bridge_destroy_context(_context);
            }
        }

        public Task<string> CallFunction(string functionName, string functionParamsJson = "")
        {
            if (functionParamsJson == null)
            {
                functionParamsJson = "";
            }

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
                    tcs.SetException(new TonClientException(json));
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

            return tcs.Task;
        }

        private Task<uint> CreateContextAsync()
        {
            var tcs = new TaskCompletionSource<uint>();

            Logger.Debug("Init context");

            var jsonConfig = JsonConvert.SerializeObject(new
            {
                // TODO: fill?
            });

            var callbackHandle = default(GCHandle);

            void NativeCallback(IntPtr jsonPtr, int len)
            {
                try
                {
                    if (jsonPtr == IntPtr.Zero)
                    {
                        Logger.Error("Init context returned null");
                        tcs.SetException(new TonClientException($"{nameof(Interop.tc_bridge_create_context)} returned null"));
                    }
                    else
                    {
                        var json = Utf8String.ToString(jsonPtr, len);
                        Logger.Debug($"Init context returned JSON: {json}");
                        var token = JObject.Parse(json);
                        if (token.TryGetValue("result", out var contextToken) && contextToken != null)
                        {
                            Logger.Debug($"Init context succeeded: {contextToken}");
                            tcs.SetResult(contextToken.Value<uint>());
                        }
                        else
                        {
                            if (token.TryGetValue("error", out var errorToken) && errorToken != null)
                            {
                                Logger.Debug($"throwing exception with error {errorToken}");
                                tcs.SetException(new TonClientException($"{nameof(Interop.tc_bridge_create_context)} returned error: {errorToken}"));
                            }
                            else
                            {
                                Logger.Debug($"throwing exception with the returned JSON: {json}");
                                tcs.SetException(new TonClientException($"{nameof(Interop.tc_bridge_create_context)} returned unsuccessful result: {json}"));
                            }
                        }
                    }
                }
                finally
                {
                    if (callbackHandle.IsAllocated)
                    {
                        callbackHandle.Free();
                    }
                }
            }

            callbackHandle = GCHandle.Alloc((Interop.tc_bridge_json_callback_t)NativeCallback);

            var configStr = new Utf8String(jsonConfig);
            Interop.tc_bridge_create_context(configStr.Ptr, configStr.Length, NativeCallback);

            return tcs.Task;
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
