using Newtonsoft.Json;
using System;
using System.Numerics;
using System.Threading.Tasks;
using TonSdk.Modules;

/*
* TON API version 1.0.0, boc module.
* THIS FILE WAS GENERATED AUTOMATICALLY.
*/

namespace TonSdk.Modules
{
    public class ParamsOfParse
    {
        /// <summary>
        ///  BOC encoded as base64
        /// </summary>
        [JsonProperty("boc", NullValueHandling = NullValueHandling.Ignore)]
        public string Boc { get; set; }
    }

    public class ResultOfParse
    {
        /// <summary>
        ///  JSON containing parsed BOC
        /// </summary>
        [JsonProperty("parsed", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Parsed { get; set; }
    }

    public class ParamsOfParseShardstate
    {
        /// <summary>
        ///  BOC encoded as base64
        /// </summary>
        [JsonProperty("boc", NullValueHandling = NullValueHandling.Ignore)]
        public string Boc { get; set; }

        /// <summary>
        ///  Shardstate identificator
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        ///  Workchain shardstate belongs to 
        /// </summary>
        [JsonProperty("workchain_id", NullValueHandling = NullValueHandling.Ignore)]
        public int WorkchainId { get; set; }
    }

    public class ParamsOfGetBlockchainConfig
    {
        /// <summary>
        ///  Key block BOC encoded as base64
        /// </summary>
        [JsonProperty("block_boc", NullValueHandling = NullValueHandling.Ignore)]
        public string BlockBoc { get; set; }
    }

    public class ResultOfGetBlockchainConfig
    {
        /// <summary>
        ///  Blockchain config BOC encoded as base64
        /// </summary>
        [JsonProperty("config_boc", NullValueHandling = NullValueHandling.Ignore)]
        public string ConfigBoc { get; set; }
    }

    public class ParamsOfGetBocHash
    {
        /// <summary>
        ///  BOC encoded as base64
        /// </summary>
        [JsonProperty("boc", NullValueHandling = NullValueHandling.Ignore)]
        public string Boc { get; set; }
    }

    public class ResultOfGetBocHash
    {
        /// <summary>
        ///  BOC root hash encoded with hex
        /// </summary>
        [JsonProperty("hash", NullValueHandling = NullValueHandling.Ignore)]
        public string Hash { get; set; }
    }

    /// <summary>
    ///  BOC manipulation module.
    /// </summary>
    public interface IBocModule
    {
        /// <summary>
        ///  Parses message boc into a JSON 
        ///  
        ///  JSON structure is compatible with GraphQL API message object
        /// </summary>
        Task<ResultOfParse> ParseMessageAsync(ParamsOfParse @params);

        /// <summary>
        ///  Parses transaction boc into a JSON 
        ///  
        ///  JSON structure is compatible with GraphQL API transaction object
        /// </summary>
        Task<ResultOfParse> ParseTransactionAsync(ParamsOfParse @params);

        /// <summary>
        ///  Parses account boc into a JSON 
        ///  
        ///  JSON structure is compatible with GraphQL API account object
        /// </summary>
        Task<ResultOfParse> ParseAccountAsync(ParamsOfParse @params);

        /// <summary>
        ///  Parses block boc into a JSON 
        ///  
        ///  JSON structure is compatible with GraphQL API block object
        /// </summary>
        Task<ResultOfParse> ParseBlockAsync(ParamsOfParse @params);

        /// <summary>
        ///  Parses shardstate boc into a JSON 
        ///  
        ///  JSON structure is compatible with GraphQL API shardstate object
        /// </summary>
        Task<ResultOfParse> ParseShardstateAsync(ParamsOfParseShardstate @params);

        Task<ResultOfGetBlockchainConfig> GetBlockchainConfigAsync(ParamsOfGetBlockchainConfig @params);

        /// <summary>
        ///  Calculates BOC root hash
        /// </summary>
        Task<ResultOfGetBocHash> GetBocHashAsync(ParamsOfGetBocHash @params);
    }

    internal class BocModule : IBocModule
    {
        private readonly TonClient _client;

        internal BocModule(TonClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<ResultOfParse> ParseMessageAsync(ParamsOfParse @params)
        {
            return await _client.CallFunctionAsync<ResultOfParse>("boc.parse_message", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfParse> ParseTransactionAsync(ParamsOfParse @params)
        {
            return await _client.CallFunctionAsync<ResultOfParse>("boc.parse_transaction", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfParse> ParseAccountAsync(ParamsOfParse @params)
        {
            return await _client.CallFunctionAsync<ResultOfParse>("boc.parse_account", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfParse> ParseBlockAsync(ParamsOfParse @params)
        {
            return await _client.CallFunctionAsync<ResultOfParse>("boc.parse_block", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfParse> ParseShardstateAsync(ParamsOfParseShardstate @params)
        {
            return await _client.CallFunctionAsync<ResultOfParse>("boc.parse_shardstate", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfGetBlockchainConfig> GetBlockchainConfigAsync(ParamsOfGetBlockchainConfig @params)
        {
            return await _client.CallFunctionAsync<ResultOfGetBlockchainConfig>("boc.get_blockchain_config", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfGetBocHash> GetBocHashAsync(ParamsOfGetBocHash @params)
        {
            return await _client.CallFunctionAsync<ResultOfGetBocHash>("boc.get_boc_hash", @params).ConfigureAwait(false);
        }
    }
}

namespace TonSdk
{
    public partial interface ITonClient
    {
        IBocModule Boc { get; }
    }

    public partial class TonClient
    {
        private BocModule _bocModule;

        public IBocModule Boc
        {
            get
            {
                return _bocModule ?? (_bocModule = new BocModule(this));
            }
        }
    }
}

