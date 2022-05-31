using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Numerics;
using System.Threading.Tasks;
using TonSdk.Modules;

/*
* TON API version 1.34.2, client module.
* THIS FILE WAS GENERATED AUTOMATICALLY.
*/

namespace TonSdk.Modules
{
    public enum ClientErrorCode
    {
        NotImplemented = 1,
        InvalidHex = 2,
        InvalidBase64 = 3,
        InvalidAddress = 4,
        CallbackParamsCantBeConvertedToJson = 5,
        WebsocketConnectError = 6,
        WebsocketReceiveError = 7,
        WebsocketSendError = 8,
        HttpClientCreateError = 9,
        HttpRequestCreateError = 10,
        HttpRequestSendError = 11,
        HttpRequestParseError = 12,
        CallbackNotRegistered = 13,
        NetModuleNotInit = 14,
        InvalidConfig = 15,
        CannotCreateRuntime = 16,
        InvalidContextHandle = 17,
        CannotSerializeResult = 18,
        CannotSerializeError = 19,
        CannotConvertJsValueToJson = 20,
        CannotReceiveSpawnedResult = 21,
        SetTimerError = 22,
        InvalidParams = 23,
        ContractsAddressConversionFailed = 24,
        UnknownFunction = 25,
        AppRequestError = 26,
        NoSuchRequest = 27,
        CanNotSendRequestResult = 28,
        CanNotReceiveRequestResult = 29,
        CanNotParseRequestResult = 30,
        UnexpectedCallbackResponse = 31,
        CanNotParseNumber = 32,
        InternalError = 33,
        InvalidHandle = 34,
        LocalStorageError = 35,
    }

    public class ClientError
    {
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public uint Code { get; set; }

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Data { get; set; }
    }

    public class ClientConfig
    {
        [JsonProperty("network", NullValueHandling = NullValueHandling.Ignore)]
        public NetworkConfig Network { get; set; }

        [JsonProperty("crypto", NullValueHandling = NullValueHandling.Ignore)]
        public CryptoConfig Crypto { get; set; }

        [JsonProperty("abi", NullValueHandling = NullValueHandling.Ignore)]
        public AbiConfig Abi { get; set; }

        [JsonProperty("boc", NullValueHandling = NullValueHandling.Ignore)]
        public BocConfig Boc { get; set; }

        [JsonProperty("proofs", NullValueHandling = NullValueHandling.Ignore)]
        public ProofsConfig Proofs { get; set; }

        /// <summary>
        /// For file based storage is a folder name where SDK will store its data. For browser based is a
        /// browser async storage key prefix. Default (recommended) value is "~/.tonclient" for native
        /// environments and ".tonclient" for web-browser.
        /// </summary>
        [JsonProperty("local_storage_path", NullValueHandling = NullValueHandling.Ignore)]
        public string LocalStoragePath { get; set; }
    }

    public class NetworkConfig
    {
        /// <summary>
        /// **This field is deprecated, but left for backward-compatibility.** DApp Server public address.
        /// </summary>
        [JsonProperty("server_address", NullValueHandling = NullValueHandling.Ignore)]
        public string ServerAddress { get; set; }

        /// <summary>
        /// Any correct URL format can be specified, including IP addresses. This parameter is prevailing over
        /// `server_address`.
        /// Check the full list of [supported network endpoints](../ton-os-api/networks.md).
        /// </summary>
        [JsonProperty("endpoints", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Endpoints { get; set; }

        /// <summary>
        /// You must use `network.max_reconnect_timeout` that allows to specify maximum network resolving
        /// timeout.
        /// </summary>
        [JsonProperty("network_retries_count", NullValueHandling = NullValueHandling.Ignore)]
        public sbyte? NetworkRetriesCount { get; set; }

        /// <summary>
        /// Must be specified in milliseconds. Default is 120000 (2 min).
        /// </summary>
        [JsonProperty("max_reconnect_timeout", NullValueHandling = NullValueHandling.Ignore)]
        public uint? MaxReconnectTimeout { get; set; }

        /// <summary>
        /// Deprecated
        /// </summary>
        [JsonProperty("reconnect_timeout", NullValueHandling = NullValueHandling.Ignore)]
        public uint? ReconnectTimeout { get; set; }

        /// <summary>
        /// Default is 5.
        /// </summary>
        [JsonProperty("message_retries_count", NullValueHandling = NullValueHandling.Ignore)]
        public sbyte? MessageRetriesCount { get; set; }

        /// <summary>
        /// Must be specified in milliseconds. Default is 40000 (40 sec).
        /// </summary>
        [JsonProperty("message_processing_timeout", NullValueHandling = NullValueHandling.Ignore)]
        public uint? MessageProcessingTimeout { get; set; }

        /// <summary>
        /// Must be specified in milliseconds. Default is 40000 (40 sec).
        /// </summary>
        [JsonProperty("wait_for_timeout", NullValueHandling = NullValueHandling.Ignore)]
        public uint? WaitForTimeout { get; set; }

        /// <summary>
        /// If client's device time is out of sync and difference is more than the threshold then error will
        /// occur. Also an error will occur if the specified threshold is more than
        /// `message_processing_timeout/2`.
        /// 
        /// Must be specified in milliseconds. Default is 15000 (15 sec).
        /// </summary>
        [JsonProperty("out_of_sync_threshold", NullValueHandling = NullValueHandling.Ignore)]
        public uint? OutOfSyncThreshold { get; set; }

        /// <summary>
        /// Default is 1.
        /// </summary>
        [JsonProperty("sending_endpoint_count", NullValueHandling = NullValueHandling.Ignore)]
        public byte? SendingEndpointCount { get; set; }

        /// <summary>
        /// Library periodically checks the current endpoint for blockchain data syncronization latency.
        /// If the latency (time-lag) is less then `NetworkConfig.max_latency`
        /// then library selects another endpoint.
        /// 
        /// Must be specified in milliseconds. Default is 60000 (1 min).
        /// </summary>
        [JsonProperty("latency_detection_interval", NullValueHandling = NullValueHandling.Ignore)]
        public uint? LatencyDetectionInterval { get; set; }

        /// <summary>
        /// Must be specified in milliseconds. Default is 60000 (1 min).
        /// </summary>
        [JsonProperty("max_latency", NullValueHandling = NullValueHandling.Ignore)]
        public uint? MaxLatency { get; set; }

        /// <summary>
        /// Is is used when no timeout specified for the request to limit the answer waiting time. If no answer
        /// received during the timeout requests ends with
        /// error.
        /// 
        /// Must be specified in milliseconds. Default is 60000 (1 min).
        /// </summary>
        [JsonProperty("query_timeout", NullValueHandling = NullValueHandling.Ignore)]
        public uint? QueryTimeout { get; set; }

        /// <summary>
        /// `HTTP` or `WS`. 
        /// Default is `HTTP`.
        /// </summary>
        [JsonProperty("queries_protocol", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public NetworkQueriesProtocol QueriesProtocol { get; set; }

        /// <summary>
        /// First REMP status awaiting timeout. If no status recieved during the timeout than fallback
        /// transaction scenario is activated.
        /// 
        /// Must be specified in milliseconds. Default is 1000 (1 sec).
        /// </summary>
        [JsonProperty("first_remp_status_timeout", NullValueHandling = NullValueHandling.Ignore)]
        public uint? FirstRempStatusTimeout { get; set; }

        /// <summary>
        /// Subsequent REMP status awaiting timeout. If no status recieved during the timeout than fallback
        /// transaction scenario is activated.
        /// 
        /// Must be specified in milliseconds. Default is 5000 (5 sec).
        /// </summary>
        [JsonProperty("next_remp_status_timeout", NullValueHandling = NullValueHandling.Ignore)]
        public uint? NextRempStatusTimeout { get; set; }

        /// <summary>
        /// At the moment is not used in production.
        /// </summary>
        [JsonProperty("access_key", NullValueHandling = NullValueHandling.Ignore)]
        public string AccessKey { get; set; }
    }

    /// <summary>
    /// Network protocol used to perform GraphQL queries.
    /// </summary>
    public enum NetworkQueriesProtocol
    {
        /// <summary>
        /// Each GraphQL query uses separate HTTP request.
        /// </summary>
        HTTP,
        /// <summary>
        /// All GraphQL queries will be served using single web socket connection.
        /// </summary>
        WS,
    }

    /// <summary>
    /// Crypto config.
    /// </summary>
    public class CryptoConfig
    {
        /// <summary>
        /// Mnemonic dictionary that will be used by default in crypto functions. If not specified, 1 dictionary
        /// will be used.
        /// </summary>
        [JsonProperty("mnemonic_dictionary", NullValueHandling = NullValueHandling.Ignore)]
        public byte? MnemonicDictionary { get; set; }

        /// <summary>
        /// Mnemonic word count that will be used by default in crypto functions. If not specified the default
        /// value will be 12.
        /// </summary>
        [JsonProperty("mnemonic_word_count", NullValueHandling = NullValueHandling.Ignore)]
        public byte? MnemonicWordCount { get; set; }

        /// <summary>
        /// Derivation path that will be used by default in crypto functions. If not specified
        /// `m/44'/396'/0'/0/0` will be used.
        /// </summary>
        [JsonProperty("hdkey_derivation_path", NullValueHandling = NullValueHandling.Ignore)]
        public string HdkeyDerivationPath { get; set; }
    }

    public class AbiConfig
    {
        /// <summary>
        /// Workchain id that is used by default in DeploySet
        /// </summary>
        [JsonProperty("workchain", NullValueHandling = NullValueHandling.Ignore)]
        public int? Workchain { get; set; }

        /// <summary>
        /// Message lifetime for contracts which ABI includes "expire" header. The default value is 40 sec.
        /// </summary>
        [JsonProperty("message_expiration_timeout", NullValueHandling = NullValueHandling.Ignore)]
        public uint? MessageExpirationTimeout { get; set; }

        /// <summary>
        /// Factor that increases the expiration timeout for each retry The default value is 1.5
        /// </summary>
        [JsonProperty("message_expiration_timeout_grow_factor", NullValueHandling = NullValueHandling.Ignore)]
        public float? MessageExpirationTimeoutGrowFactor { get; set; }
    }

    public class BocConfig
    {
        /// <summary>
        /// Default is 10 MB
        /// </summary>
        [JsonProperty("cache_max_size", NullValueHandling = NullValueHandling.Ignore)]
        public uint? CacheMaxSize { get; set; }
    }

    public class ProofsConfig
    {
        /// <summary>
        /// Default is `true`. If this value is set to `true`, downloaded proofs and master-chain BOCs are saved
        /// into the
        /// persistent local storage (e.g. file system for native environments or browser's IndexedDB
        /// for the web); otherwise all the data is cached only in memory in current client's context
        /// and will be lost after destruction of the client.
        /// </summary>
        [JsonProperty("cache_in_local_storage", NullValueHandling = NullValueHandling.Ignore)]
        public bool? CacheInLocalStorage { get; set; }
    }

    public class BuildInfoDependency
    {
        /// <summary>
        /// Usually it is a crate name.
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Git commit hash of the related repository.
        /// </summary>
        [JsonProperty("git_commit", NullValueHandling = NullValueHandling.Ignore)]
        public string GitCommit { get; set; }
    }

    public class ParamsOfAppRequest
    {
        /// <summary>
        /// Should be used in `resolve_app_request` call
        /// </summary>
        [JsonProperty("app_request_id", NullValueHandling = NullValueHandling.Ignore)]
        public uint AppRequestId { get; set; }

        /// <summary>
        /// Request describing data
        /// </summary>
        [JsonProperty("request_data", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken RequestData { get; set; }
    }

    public abstract class AppRequestResult
    {
        /// <summary>
        /// Error occurred during request processing
        /// </summary>
        public class Error : AppRequestResult
        {
            /// <summary>
            /// Error description
            /// </summary>
            [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
            public string Text { get; set; }
        }

        /// <summary>
        /// Request processed successfully
        /// </summary>
        public class Ok : AppRequestResult
        {
            /// <summary>
            /// Request processing result
            /// </summary>
            [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
            public Newtonsoft.Json.Linq.JToken Result { get; set; }
        }
    }

    public class ResultOfGetApiReference
    {
        [JsonProperty("api", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Api { get; set; }
    }

    public class ResultOfVersion
    {
        /// <summary>
        /// Core Library version
        /// </summary>
        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }
    }

    public class ResultOfBuildInfo
    {
        /// <summary>
        /// Build number assigned to this build by the CI.
        /// </summary>
        [JsonProperty("build_number", NullValueHandling = NullValueHandling.Ignore)]
        public uint BuildNumber { get; set; }

        /// <summary>
        /// Fingerprint of the most important dependencies.
        /// </summary>
        [JsonProperty("dependencies", NullValueHandling = NullValueHandling.Ignore)]
        public BuildInfoDependency[] Dependencies { get; set; }
    }

    public class ParamsOfResolveAppRequest
    {
        /// <summary>
        /// Request ID received from SDK
        /// </summary>
        [JsonProperty("app_request_id", NullValueHandling = NullValueHandling.Ignore)]
        public uint AppRequestId { get; set; }

        /// <summary>
        /// Result of request processing
        /// </summary>
        [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public AppRequestResult Result { get; set; }
    }

    /// <summary>
    /// Provides information about library.
    /// </summary>
    public interface IClientModule
    {
        /// <summary>
        /// Returns Core Library API reference
        /// </summary>
        Task<ResultOfGetApiReference> GetApiReferenceAsync();

        /// <summary>
        /// Returns Core Library version
        /// </summary>
        Task<ResultOfVersion> VersionAsync();

        /// <summary>
        /// Returns Core Library API reference
        /// </summary>
        Task<ClientConfig> ConfigAsync();

        /// <summary>
        /// Returns detailed information about this build.
        /// </summary>
        Task<ResultOfBuildInfo> BuildInfoAsync();

        /// <summary>
        /// Resolves application request processing result
        /// </summary>
        Task ResolveAppRequestAsync(ParamsOfResolveAppRequest @params);
    }

    internal class ClientModule : IClientModule
    {
        private readonly TonClient _client;

        internal ClientModule(TonClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<ResultOfGetApiReference> GetApiReferenceAsync()
        {
            return await _client.CallFunctionAsync<ResultOfGetApiReference>("client.get_api_reference").ConfigureAwait(false);
        }

        public async Task<ResultOfVersion> VersionAsync()
        {
            return await _client.CallFunctionAsync<ResultOfVersion>("client.version").ConfigureAwait(false);
        }

        public async Task<ClientConfig> ConfigAsync()
        {
            return await _client.CallFunctionAsync<ClientConfig>("client.config").ConfigureAwait(false);
        }

        public async Task<ResultOfBuildInfo> BuildInfoAsync()
        {
            return await _client.CallFunctionAsync<ResultOfBuildInfo>("client.build_info").ConfigureAwait(false);
        }

        public async Task ResolveAppRequestAsync(ParamsOfResolveAppRequest @params)
        {
            await _client.CallFunctionAsync("client.resolve_app_request", @params).ConfigureAwait(false);
        }
    }
}

namespace TonSdk
{
    public partial interface ITonClient
    {
        IClientModule Client { get; }
    }

    public partial class TonClient
    {
        private ClientModule _clientModule;

        public IClientModule Client
        {
            get
            {
                return _clientModule ?? (_clientModule = new ClientModule(this));
            }
        }
    }
}

