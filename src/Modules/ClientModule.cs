using Newtonsoft.Json;
using System;
using System.Numerics;
using System.Threading.Tasks;
using TonSdk.Modules;

/*
* TON API version 1.0.0, client module.
* THIS FILE WAS GENERATED AUTOMATICALLY.
*/

namespace TonSdk.Modules
{
    public class ClientError
    {
        [JsonProperty("code")]
        public uint Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public Newtonsoft.Json.Linq.JToken Data { get; set; }
    }

    public class ClientConfig
    {
        [JsonProperty("network")]
        public NetworkConfig Network { get; set; }

        [JsonProperty("crypto")]
        public CryptoConfig Crypto { get; set; }

        [JsonProperty("abi")]
        public AbiConfig Abi { get; set; }
    }

    public class NetworkConfig
    {
        [JsonProperty("server_address")]
        public string ServerAddress { get; set; }

        [JsonProperty("network_retries_count")]
        public sbyte? NetworkRetriesCount { get; set; }

        [JsonProperty("message_retries_count")]
        public sbyte? MessageRetriesCount { get; set; }

        [JsonProperty("message_processing_timeout")]
        public uint? MessageProcessingTimeout { get; set; }

        [JsonProperty("wait_for_timeout")]
        public uint? WaitForTimeout { get; set; }

        [JsonProperty("out_of_sync_threshold")]
        public uint? OutOfSyncThreshold { get; set; }

        [JsonProperty("access_key")]
        public string AccessKey { get; set; }
    }

    public class CryptoConfig
    {
        [JsonProperty("mnemonic_dictionary")]
        public byte? MnemonicDictionary { get; set; }

        [JsonProperty("mnemonic_word_count")]
        public byte? MnemonicWordCount { get; set; }

        [JsonProperty("hdkey_derivation_path")]
        public string HdkeyDerivationPath { get; set; }

        [JsonProperty("hdkey_compliant")]
        public bool? HdkeyCompliant { get; set; }
    }

    public class AbiConfig
    {
        [JsonProperty("workchain")]
        public int? Workchain { get; set; }

        [JsonProperty("message_expiration_timeout")]
        public uint? MessageExpirationTimeout { get; set; }

        [JsonProperty("message_expiration_timeout_grow_factor")]
        public float? MessageExpirationTimeoutGrowFactor { get; set; }
    }

    public class BuildInfoDependency
    {
        /// <summary>
        ///  Dependency name. Usually it is a crate name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        ///  Git commit hash of the related repository.
        /// </summary>
        [JsonProperty("git_commit")]
        public string GitCommit { get; set; }
    }

    public class ResultOfGetApiReference
    {
        [JsonProperty("api")]
        public Newtonsoft.Json.Linq.JToken Api { get; set; }
    }

    public class ResultOfVersion
    {
        /// <summary>
        ///  Core Library version
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }
    }

    public class ResultOfBuildInfo
    {
        /// <summary>
        ///  Build number assigned to this build by the CI.
        /// </summary>
        [JsonProperty("build_number")]
        public uint BuildNumber { get; set; }

        /// <summary>
        ///  Fingerprint of the most important dependencies.
        /// </summary>
        [JsonProperty("dependencies")]
        public BuildInfoDependency[] Dependencies { get; set; }
    }

    /// <summary>
    ///  Provides information about library.
    /// </summary>
    public interface IClientModule
    {
        /// <summary>
        ///  Returns Core Library API reference
        /// </summary>
        Task<ResultOfGetApiReference> GetApiReferenceAsync();

        /// <summary>
        ///  Returns Core Library version
        /// </summary>
        Task<ResultOfVersion> VersionAsync();

        /// <summary>
        ///  Returns detailed information about this build.
        /// </summary>
        Task<ResultOfBuildInfo> BuildInfoAsync();
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

