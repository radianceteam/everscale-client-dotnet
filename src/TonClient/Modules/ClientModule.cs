using Newtonsoft.Json;
using System;
using System.Numerics;
using System.Threading.Tasks;
using TonSdk.Modules;

/*
* TON API version 1.5.0, client module.
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
    }

    public class NetworkConfig
    {
        /// <summary>
        /// DApp Server public address. For instance, for `net.ton.dev/graphql` GraphQL endpoint the server
        /// address will be net.ton.dev
        /// </summary>
        [JsonProperty("server_address", NullValueHandling = NullValueHandling.Ignore)]
        public string ServerAddress { get; set; }

        /// <summary>
        /// Any correct URL format can be specified, including IP addresses
        /// </summary>
        [JsonProperty("endpoints", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Endpoints { get; set; }

        /// <summary>
        /// The number of automatic network retries that SDK performs in case of connection problems The default
        /// value is 5.
        /// </summary>
        [JsonProperty("network_retries_count", NullValueHandling = NullValueHandling.Ignore)]
        public sbyte? NetworkRetriesCount { get; set; }

        /// <summary>
        /// The number of automatic message processing retries that SDK performs in case of `Message Expired
        /// (507)` error - but only for those messages which local emulation was successfull or failed with
        /// replay protection error. The default value is 5.
        /// </summary>
        [JsonProperty("message_retries_count", NullValueHandling = NullValueHandling.Ignore)]
        public sbyte? MessageRetriesCount { get; set; }

        /// <summary>
        /// Timeout that is used to process message delivery for the contracts which ABI does not include
        /// "expire" header. If the message is not delivered within the speficied timeout the appropriate error
        /// occurs.
        /// </summary>
        [JsonProperty("message_processing_timeout", NullValueHandling = NullValueHandling.Ignore)]
        public uint? MessageProcessingTimeout { get; set; }

        /// <summary>
        /// Maximum timeout that is used for query response. The default value is 40 sec.
        /// </summary>
        [JsonProperty("wait_for_timeout", NullValueHandling = NullValueHandling.Ignore)]
        public uint? WaitForTimeout { get; set; }

        /// <summary>
        /// If client's device time is out of sink and difference is more thanthe threshhold then error will
        /// occur. Also the error will occur if the specified threshhold is more than
        /// `message_processing_timeout/2`.
        /// The default value is 15 sec.
        /// </summary>
        [JsonProperty("out_of_sync_threshold", NullValueHandling = NullValueHandling.Ignore)]
        public uint? OutOfSyncThreshold { get; set; }

        /// <summary>
        /// Timeout between reconnect attempts
        /// </summary>
        [JsonProperty("reconnect_timeout", NullValueHandling = NullValueHandling.Ignore)]
        public uint? ReconnectTimeout { get; set; }

        /// <summary>
        /// At the moment is not used in production
        /// </summary>
        [JsonProperty("access_key", NullValueHandling = NullValueHandling.Ignore)]
        public string AccessKey { get; set; }
    }

    /// <summary>
    /// Crypto config.
    /// </summary>
    public class CryptoConfig
    {
        /// <summary>
        /// Mnemonic dictionary that will be used by default in crypto funcions. If not specified, 1 dictionary
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
        /// Error occured during request processing
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
        [JsonConverter(typeof(PolymorphicConcreteTypeConverter))]
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

