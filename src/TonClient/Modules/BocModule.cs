using Newtonsoft.Json;
using System;
using System.Numerics;
using System.Threading.Tasks;
using TonSdk.Modules;

/*
* TON API version 1.24.0, boc module.
* THIS FILE WAS GENERATED AUTOMATICALLY.
*/

namespace TonSdk.Modules
{
    public abstract class BocCacheType
    {
        /// <summary>
        /// Such BOC will not be removed from cache until it is unpinned
        /// </summary>
        public class Pinned : BocCacheType
        {
            [JsonProperty("pin", NullValueHandling = NullValueHandling.Ignore)]
            public string Pin { get; set; }
        }

        /// <summary>
        ///  
        /// </summary>
        public class Unpinned : BocCacheType
        {
        }
    }

    public enum BocErrorCode
    {
        InvalidBoc = 201,
        SerializationError = 202,
        InappropriateBlock = 203,
        MissingSourceBoc = 204,
        InsufficientCacheSize = 205,
        BocRefNotFound = 206,
        InvalidBocRef = 207,
    }

    public class ParamsOfParse
    {
        /// <summary>
        /// BOC encoded as base64
        /// </summary>
        [JsonProperty("boc", NullValueHandling = NullValueHandling.Ignore)]
        public string Boc { get; set; }
    }

    public class ResultOfParse
    {
        /// <summary>
        /// JSON containing parsed BOC
        /// </summary>
        [JsonProperty("parsed", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Parsed { get; set; }
    }

    public class ParamsOfParseShardstate
    {
        /// <summary>
        /// BOC encoded as base64
        /// </summary>
        [JsonProperty("boc", NullValueHandling = NullValueHandling.Ignore)]
        public string Boc { get; set; }

        /// <summary>
        /// Shardstate identificator
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        /// Workchain shardstate belongs to
        /// </summary>
        [JsonProperty("workchain_id", NullValueHandling = NullValueHandling.Ignore)]
        public int WorkchainId { get; set; }
    }

    public class ParamsOfGetBlockchainConfig
    {
        /// <summary>
        /// Key block BOC or zerostate BOC encoded as base64
        /// </summary>
        [JsonProperty("block_boc", NullValueHandling = NullValueHandling.Ignore)]
        public string BlockBoc { get; set; }
    }

    public class ResultOfGetBlockchainConfig
    {
        /// <summary>
        /// Blockchain config BOC encoded as base64
        /// </summary>
        [JsonProperty("config_boc", NullValueHandling = NullValueHandling.Ignore)]
        public string ConfigBoc { get; set; }
    }

    public class ParamsOfGetBocHash
    {
        /// <summary>
        /// BOC encoded as base64 or BOC handle
        /// </summary>
        [JsonProperty("boc", NullValueHandling = NullValueHandling.Ignore)]
        public string Boc { get; set; }
    }

    public class ResultOfGetBocHash
    {
        /// <summary>
        /// BOC root hash encoded with hex
        /// </summary>
        [JsonProperty("hash", NullValueHandling = NullValueHandling.Ignore)]
        public string Hash { get; set; }
    }

    public class ParamsOfGetBocDepth
    {
        /// <summary>
        /// BOC encoded as base64 or BOC handle
        /// </summary>
        [JsonProperty("boc", NullValueHandling = NullValueHandling.Ignore)]
        public string Boc { get; set; }
    }

    public class ResultOfGetBocDepth
    {
        /// <summary>
        /// BOC root cell depth
        /// </summary>
        [JsonProperty("depth", NullValueHandling = NullValueHandling.Ignore)]
        public uint Depth { get; set; }
    }

    public class ParamsOfGetCodeFromTvc
    {
        /// <summary>
        /// Contract TVC image or image BOC handle
        /// </summary>
        [JsonProperty("tvc", NullValueHandling = NullValueHandling.Ignore)]
        public string Tvc { get; set; }
    }

    public class ResultOfGetCodeFromTvc
    {
        /// <summary>
        /// Contract code encoded as base64
        /// </summary>
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }
    }

    public class ParamsOfBocCacheGet
    {
        /// <summary>
        /// Reference to the cached BOC
        /// </summary>
        [JsonProperty("boc_ref", NullValueHandling = NullValueHandling.Ignore)]
        public string BocRef { get; set; }
    }

    public class ResultOfBocCacheGet
    {
        /// <summary>
        /// BOC encoded as base64.
        /// </summary>
        [JsonProperty("boc", NullValueHandling = NullValueHandling.Ignore)]
        public string Boc { get; set; }
    }

    public class ParamsOfBocCacheSet
    {
        /// <summary>
        /// BOC encoded as base64 or BOC reference
        /// </summary>
        [JsonProperty("boc", NullValueHandling = NullValueHandling.Ignore)]
        public string Boc { get; set; }

        /// <summary>
        /// Cache type
        /// </summary>
        [JsonProperty("cache_type", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public BocCacheType CacheType { get; set; }
    }

    public class ResultOfBocCacheSet
    {
        /// <summary>
        /// Reference to the cached BOC
        /// </summary>
        [JsonProperty("boc_ref", NullValueHandling = NullValueHandling.Ignore)]
        public string BocRef { get; set; }
    }

    public class ParamsOfBocCacheUnpin
    {
        /// <summary>
        /// Pinned name
        /// </summary>
        [JsonProperty("pin", NullValueHandling = NullValueHandling.Ignore)]
        public string Pin { get; set; }

        /// <summary>
        /// If it is provided then only referenced BOC is unpinned
        /// </summary>
        [JsonProperty("boc_ref", NullValueHandling = NullValueHandling.Ignore)]
        public string BocRef { get; set; }
    }

    /// <summary>
    /// Cell builder operation.
    /// </summary>
    public abstract class BuilderOp
    {
        /// <summary>
        /// Append integer to cell data.
        /// </summary>
        public class Integer : BuilderOp
        {
            /// <summary>
            /// Bit size of the value.
            /// </summary>
            [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
            public uint Size { get; set; }

            /// <summary>
            /// e.g. `123`, `-123`. - Decimal string. e.g. `"123"`, `"-123"`.
            /// - `0x` prefixed hexadecimal string.
            ///   e.g `0x123`, `0X123`, `-0x123`.
            /// </summary>
            [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
            public Newtonsoft.Json.Linq.JToken Value { get; set; }
        }

        /// <summary>
        /// Append bit string to cell data.
        /// </summary>
        public class BitString : BuilderOp
        {
            /// <summary>
            /// Contains hexadecimal string representation:
            /// - Can end with `_` tag.
            /// - Can be prefixed with `x` or `X`.
            /// - Can be prefixed with `x{` or `X{` and ended with `}`.
            /// 
            /// Contains binary string represented as a sequence
            /// of `0` and `1` prefixed with `n` or `N`.
            /// 
            /// Examples:
            /// `1AB`, `x1ab`, `X1AB`, `x{1abc}`, `X{1ABC}`
            /// `2D9_`, `x2D9_`, `X2D9_`, `x{2D9_}`, `X{2D9_}`
            /// `n00101101100`, `N00101101100`
            /// </summary>
            [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
            public string Value { get; set; }
        }

        /// <summary>
        /// Append ref to nested cells
        /// </summary>
        public class Cell : BuilderOp
        {
            /// <summary>
            /// Nested cell builder
            /// </summary>
            [JsonProperty("builder", NullValueHandling = NullValueHandling.Ignore,
            ItemConverterType = typeof(PolymorphicTypeConverter))]
            public BuilderOp[] Builder { get; set; }
        }

        /// <summary>
        /// Append ref to nested cell
        /// </summary>
        public class CellBoc : BuilderOp
        {
            /// <summary>
            /// Nested cell BOC encoded with `base64` or BOC cache key.
            /// </summary>
            [JsonProperty("boc", NullValueHandling = NullValueHandling.Ignore)]
            public string Boc { get; set; }
        }
    }

    public class ParamsOfEncodeBoc
    {
        /// <summary>
        /// Cell builder operations.
        /// </summary>
        [JsonProperty("builder", NullValueHandling = NullValueHandling.Ignore,
            ItemConverterType = typeof(PolymorphicTypeConverter))]
        public BuilderOp[] Builder { get; set; }

        /// <summary>
        /// Cache type to put the result. The BOC itself returned if no cache type provided.
        /// </summary>
        [JsonProperty("boc_cache", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public BocCacheType BocCache { get; set; }
    }

    public class ResultOfEncodeBoc
    {
        /// <summary>
        /// Encoded cell BOC or BOC cache key.
        /// </summary>
        [JsonProperty("boc", NullValueHandling = NullValueHandling.Ignore)]
        public string Boc { get; set; }
    }

    public class ParamsOfGetCodeSalt
    {
        /// <summary>
        /// Contract code BOC encoded as base64 or code BOC handle
        /// </summary>
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        /// <summary>
        /// Cache type to put the result. The BOC itself returned if no cache type provided.
        /// </summary>
        [JsonProperty("boc_cache", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public BocCacheType BocCache { get; set; }
    }

    public class ResultOfGetCodeSalt
    {
        /// <summary>
        /// BOC encoded as base64 or BOC handle
        /// </summary>
        [JsonProperty("salt", NullValueHandling = NullValueHandling.Ignore)]
        public string Salt { get; set; }
    }

    public class ParamsOfSetCodeSalt
    {
        /// <summary>
        /// Contract code BOC encoded as base64 or code BOC handle
        /// </summary>
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        /// <summary>
        /// BOC encoded as base64 or BOC handle
        /// </summary>
        [JsonProperty("salt", NullValueHandling = NullValueHandling.Ignore)]
        public string Salt { get; set; }

        /// <summary>
        /// Cache type to put the result. The BOC itself returned if no cache type provided.
        /// </summary>
        [JsonProperty("boc_cache", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public BocCacheType BocCache { get; set; }
    }

    public class ResultOfSetCodeSalt
    {
        /// <summary>
        /// BOC encoded as base64 or BOC handle
        /// </summary>
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }
    }

    public class ParamsOfDecodeTvc
    {
        /// <summary>
        /// Contract TVC image BOC encoded as base64 or BOC handle
        /// </summary>
        [JsonProperty("tvc", NullValueHandling = NullValueHandling.Ignore)]
        public string Tvc { get; set; }

        /// <summary>
        /// Cache type to put the result. The BOC itself returned if no cache type provided.
        /// </summary>
        [JsonProperty("boc_cache", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public BocCacheType BocCache { get; set; }
    }

    public class ResultOfDecodeTvc
    {
        /// <summary>
        /// Contract code BOC encoded as base64 or BOC handle
        /// </summary>
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        /// <summary>
        /// Contract code hash
        /// </summary>
        [JsonProperty("code_hash", NullValueHandling = NullValueHandling.Ignore)]
        public string CodeHash { get; set; }

        /// <summary>
        /// Contract code depth
        /// </summary>
        [JsonProperty("code_depth", NullValueHandling = NullValueHandling.Ignore)]
        public uint? CodeDepth { get; set; }

        /// <summary>
        /// Contract data BOC encoded as base64 or BOC handle
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public string Data { get; set; }

        /// <summary>
        /// Contract data hash
        /// </summary>
        [JsonProperty("data_hash", NullValueHandling = NullValueHandling.Ignore)]
        public string DataHash { get; set; }

        /// <summary>
        /// Contract data depth
        /// </summary>
        [JsonProperty("data_depth", NullValueHandling = NullValueHandling.Ignore)]
        public uint? DataDepth { get; set; }

        /// <summary>
        /// Contract library BOC encoded as base64 or BOC handle
        /// </summary>
        [JsonProperty("library", NullValueHandling = NullValueHandling.Ignore)]
        public string Library { get; set; }

        /// <summary>
        /// Specifies the contract ability to handle tick transactions
        /// </summary>
        [JsonProperty("tick", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Tick { get; set; }

        /// <summary>
        /// Specifies the contract ability to handle tock transactions
        /// </summary>
        [JsonProperty("tock", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Tock { get; set; }

        /// <summary>
        /// Is present and non-zero only in instances of large smart contracts
        /// </summary>
        [JsonProperty("split_depth", NullValueHandling = NullValueHandling.Ignore)]
        public uint? SplitDepth { get; set; }

        /// <summary>
        /// Compiler version, for example 'sol 0.49.0'
        /// </summary>
        [JsonProperty("compiler_version", NullValueHandling = NullValueHandling.Ignore)]
        public string CompilerVersion { get; set; }
    }

    public class ParamsOfEncodeTvc
    {
        /// <summary>
        /// Contract code BOC encoded as base64 or BOC handle
        /// </summary>
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        /// <summary>
        /// Contract data BOC encoded as base64 or BOC handle
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public string Data { get; set; }

        /// <summary>
        /// Contract library BOC encoded as base64 or BOC handle
        /// </summary>
        [JsonProperty("library", NullValueHandling = NullValueHandling.Ignore)]
        public string Library { get; set; }

        /// <summary>
        /// Specifies the contract ability to handle tick transactions
        /// </summary>
        [JsonProperty("tick", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Tick { get; set; }

        /// <summary>
        /// Specifies the contract ability to handle tock transactions
        /// </summary>
        [JsonProperty("tock", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Tock { get; set; }

        /// <summary>
        /// Is present and non-zero only in instances of large smart contracts
        /// </summary>
        [JsonProperty("split_depth", NullValueHandling = NullValueHandling.Ignore)]
        public uint? SplitDepth { get; set; }

        /// <summary>
        /// Cache type to put the result. The BOC itself returned if no cache type provided.
        /// </summary>
        [JsonProperty("boc_cache", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public BocCacheType BocCache { get; set; }
    }

    public class ResultOfEncodeTvc
    {
        /// <summary>
        /// Contract TVC image BOC encoded as base64 or BOC handle of boc_cache parameter was specified
        /// </summary>
        [JsonProperty("tvc", NullValueHandling = NullValueHandling.Ignore)]
        public string Tvc { get; set; }
    }

    public class ParamsOfGetCompilerVersion
    {
        /// <summary>
        /// Contract code BOC encoded as base64 or code BOC handle
        /// </summary>
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }
    }

    public class ResultOfGetCompilerVersion
    {
        /// <summary>
        /// Compiler version, for example 'sol 0.49.0'
        /// </summary>
        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }
    }

    /// <summary>
    /// BOC manipulation module.
    /// </summary>
    public interface IBocModule
    {
        /// <summary>
        /// JSON structure is compatible with GraphQL API message object
        /// </summary>
        Task<ResultOfParse> ParseMessageAsync(ParamsOfParse @params);

        /// <summary>
        /// JSON structure is compatible with GraphQL API transaction object
        /// </summary>
        Task<ResultOfParse> ParseTransactionAsync(ParamsOfParse @params);

        /// <summary>
        /// JSON structure is compatible with GraphQL API account object
        /// </summary>
        Task<ResultOfParse> ParseAccountAsync(ParamsOfParse @params);

        /// <summary>
        /// JSON structure is compatible with GraphQL API block object
        /// </summary>
        Task<ResultOfParse> ParseBlockAsync(ParamsOfParse @params);

        /// <summary>
        /// JSON structure is compatible with GraphQL API shardstate object
        /// </summary>
        Task<ResultOfParse> ParseShardstateAsync(ParamsOfParseShardstate @params);

        /// <summary>
        /// Extract blockchain configuration from key block and also from zerostate.
        /// </summary>
        Task<ResultOfGetBlockchainConfig> GetBlockchainConfigAsync(ParamsOfGetBlockchainConfig @params);

        /// <summary>
        /// Calculates BOC root hash
        /// </summary>
        Task<ResultOfGetBocHash> GetBocHashAsync(ParamsOfGetBocHash @params);

        /// <summary>
        /// Calculates BOC depth
        /// </summary>
        Task<ResultOfGetBocDepth> GetBocDepthAsync(ParamsOfGetBocDepth @params);

        /// <summary>
        /// Extracts code from TVC contract image
        /// </summary>
        Task<ResultOfGetCodeFromTvc> GetCodeFromTvcAsync(ParamsOfGetCodeFromTvc @params);

        /// <summary>
        /// Get BOC from cache
        /// </summary>
        Task<ResultOfBocCacheGet> CacheGetAsync(ParamsOfBocCacheGet @params);

        /// <summary>
        /// Save BOC into cache
        /// </summary>
        Task<ResultOfBocCacheSet> CacheSetAsync(ParamsOfBocCacheSet @params);

        /// <summary>
        /// BOCs which don't have another pins will be removed from cache
        /// </summary>
        Task CacheUnpinAsync(ParamsOfBocCacheUnpin @params);

        /// <summary>
        /// Encodes bag of cells (BOC) with builder operations. This method provides the same functionality as
        /// Solidity TvmBuilder. Resulting BOC of this method can be passed into Solidity and C++ contracts as
        /// TvmCell type
        /// </summary>
        Task<ResultOfEncodeBoc> EncodeBocAsync(ParamsOfEncodeBoc @params);

        /// <summary>
        /// Returns the contract code's salt if it is present.
        /// </summary>
        Task<ResultOfGetCodeSalt> GetCodeSaltAsync(ParamsOfGetCodeSalt @params);

        /// <summary>
        /// Returns the new contract code with salt.
        /// </summary>
        Task<ResultOfSetCodeSalt> SetCodeSaltAsync(ParamsOfSetCodeSalt @params);

        /// <summary>
        /// Decodes tvc into code, data, libraries and special options.
        /// </summary>
        Task<ResultOfDecodeTvc> DecodeTvcAsync(ParamsOfDecodeTvc @params);

        /// <summary>
        /// Encodes tvc from code, data, libraries ans special options (see input params)
        /// </summary>
        Task<ResultOfEncodeTvc> EncodeTvcAsync(ParamsOfEncodeTvc @params);

        /// <summary>
        /// Returns the compiler version used to compile the code.
        /// </summary>
        Task<ResultOfGetCompilerVersion> GetCompilerVersionAsync(ParamsOfGetCompilerVersion @params);
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

        public async Task<ResultOfGetBocDepth> GetBocDepthAsync(ParamsOfGetBocDepth @params)
        {
            return await _client.CallFunctionAsync<ResultOfGetBocDepth>("boc.get_boc_depth", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfGetCodeFromTvc> GetCodeFromTvcAsync(ParamsOfGetCodeFromTvc @params)
        {
            return await _client.CallFunctionAsync<ResultOfGetCodeFromTvc>("boc.get_code_from_tvc", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfBocCacheGet> CacheGetAsync(ParamsOfBocCacheGet @params)
        {
            return await _client.CallFunctionAsync<ResultOfBocCacheGet>("boc.cache_get", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfBocCacheSet> CacheSetAsync(ParamsOfBocCacheSet @params)
        {
            return await _client.CallFunctionAsync<ResultOfBocCacheSet>("boc.cache_set", @params).ConfigureAwait(false);
        }

        public async Task CacheUnpinAsync(ParamsOfBocCacheUnpin @params)
        {
            await _client.CallFunctionAsync("boc.cache_unpin", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfEncodeBoc> EncodeBocAsync(ParamsOfEncodeBoc @params)
        {
            return await _client.CallFunctionAsync<ResultOfEncodeBoc>("boc.encode_boc", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfGetCodeSalt> GetCodeSaltAsync(ParamsOfGetCodeSalt @params)
        {
            return await _client.CallFunctionAsync<ResultOfGetCodeSalt>("boc.get_code_salt", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfSetCodeSalt> SetCodeSaltAsync(ParamsOfSetCodeSalt @params)
        {
            return await _client.CallFunctionAsync<ResultOfSetCodeSalt>("boc.set_code_salt", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfDecodeTvc> DecodeTvcAsync(ParamsOfDecodeTvc @params)
        {
            return await _client.CallFunctionAsync<ResultOfDecodeTvc>("boc.decode_tvc", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfEncodeTvc> EncodeTvcAsync(ParamsOfEncodeTvc @params)
        {
            return await _client.CallFunctionAsync<ResultOfEncodeTvc>("boc.encode_tvc", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfGetCompilerVersion> GetCompilerVersionAsync(ParamsOfGetCompilerVersion @params)
        {
            return await _client.CallFunctionAsync<ResultOfGetCompilerVersion>("boc.get_compiler_version", @params).ConfigureAwait(false);
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

