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
        [JsonProperty("boc")]
        public string Boc { get; set; }
    }

    public class ResultOfParse
    {
        /// <summary>
        ///  JSON containing parsed BOC
        /// </summary>
        [JsonProperty("parsed")]
        public Newtonsoft.Json.Linq.JRaw Parsed { get; set; }
    }

    public class ParamsOfGetBlockchainConfig
    {
        /// <summary>
        ///  Key block BOC encoded as base64
        /// </summary>
        [JsonProperty("block_boc")]
        public string BlockBoc { get; set; }
    }

    public class ResultOfGetBlockchainConfig
    {
        /// <summary>
        ///  Blockchain config BOC encoded as base64
        /// </summary>
        [JsonProperty("config_boc")]
        public string ConfigBoc { get; set; }
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

        Task<ResultOfGetBlockchainConfig> GetBlockchainConfigAsync(ParamsOfGetBlockchainConfig @params);
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

        public async Task<ResultOfGetBlockchainConfig> GetBlockchainConfigAsync(ParamsOfGetBlockchainConfig @params)
        {
            return await _client.CallFunctionAsync<ResultOfGetBlockchainConfig>("boc.get_blockchain_config", @params).ConfigureAwait(false);
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

