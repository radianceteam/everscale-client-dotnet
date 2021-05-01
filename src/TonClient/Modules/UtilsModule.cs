using Newtonsoft.Json;
using System;
using System.Numerics;
using System.Threading.Tasks;
using TonSdk.Modules;

/*
* TON API version 1.13.0, utils module.
* THIS FILE WAS GENERATED AUTOMATICALLY.
*/

namespace TonSdk.Modules
{
    public abstract class AddressStringFormat
    {
        public class AccountId : AddressStringFormat
        {
        }

        public class Hex : AddressStringFormat
        {
        }

        public class Base64 : AddressStringFormat
        {
            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public bool Url { get; set; }

            [JsonProperty("test", NullValueHandling = NullValueHandling.Ignore)]
            public bool Test { get; set; }

            [JsonProperty("bounce", NullValueHandling = NullValueHandling.Ignore)]
            public bool Bounce { get; set; }
        }
    }

    public class ParamsOfConvertAddress
    {
        /// <summary>
        /// Account address in any TON format.
        /// </summary>
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        /// <summary>
        /// Specify the format to convert to.
        /// </summary>
        [JsonProperty("output_format", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public AddressStringFormat OutputFormat { get; set; }
    }

    public class ResultOfConvertAddress
    {
        /// <summary>
        /// Address in the specified format
        /// </summary>
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }
    }

    public class ParamsOfCalcStorageFee
    {
        [JsonProperty("account", NullValueHandling = NullValueHandling.Ignore)]
        public string Account { get; set; }

        [JsonProperty("period", NullValueHandling = NullValueHandling.Ignore)]
        public uint Period { get; set; }
    }

    public class ResultOfCalcStorageFee
    {
        [JsonProperty("fee", NullValueHandling = NullValueHandling.Ignore)]
        public string Fee { get; set; }
    }

    public class ParamsOfCompressZstd
    {
        /// <summary>
        /// Must be encoded as base64.
        /// </summary>
        [JsonProperty("uncompressed", NullValueHandling = NullValueHandling.Ignore)]
        public string Uncompressed { get; set; }

        /// <summary>
        /// Compression level, from 1 to 21. Where: 1 - lowest compression level (fastest compression); 21 -
        /// highest compression level (slowest compression). If level is omitted, the default compression level
        /// is used (currently `3`).
        /// </summary>
        [JsonProperty("level", NullValueHandling = NullValueHandling.Ignore)]
        public int? Level { get; set; }
    }

    public class ResultOfCompressZstd
    {
        /// <summary>
        /// Must be encoded as base64.
        /// </summary>
        [JsonProperty("compressed", NullValueHandling = NullValueHandling.Ignore)]
        public string Compressed { get; set; }
    }

    public class ParamsOfDecompressZstd
    {
        /// <summary>
        /// Must be encoded as base64.
        /// </summary>
        [JsonProperty("compressed", NullValueHandling = NullValueHandling.Ignore)]
        public string Compressed { get; set; }
    }

    public class ResultOfDecompressZstd
    {
        /// <summary>
        /// Must be encoded as base64.
        /// </summary>
        [JsonProperty("decompressed", NullValueHandling = NullValueHandling.Ignore)]
        public string Decompressed { get; set; }
    }

    /// <summary>
    /// Misc utility Functions.
    /// </summary>
    public interface IUtilsModule
    {
        /// <summary>
        /// Converts address from any TON format to any TON format
        /// </summary>
        Task<ResultOfConvertAddress> ConvertAddressAsync(ParamsOfConvertAddress @params);

        /// <summary>
        /// Calculates storage fee for an account over a specified time period
        /// </summary>
        Task<ResultOfCalcStorageFee> CalcStorageFeeAsync(ParamsOfCalcStorageFee @params);

        /// <summary>
        /// Compresses data using Zstandard algorithm
        /// </summary>
        Task<ResultOfCompressZstd> CompressZstdAsync(ParamsOfCompressZstd @params);

        /// <summary>
        /// Decompresses data using Zstandard algorithm
        /// </summary>
        Task<ResultOfDecompressZstd> DecompressZstdAsync(ParamsOfDecompressZstd @params);
    }

    internal class UtilsModule : IUtilsModule
    {
        private readonly TonClient _client;

        internal UtilsModule(TonClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<ResultOfConvertAddress> ConvertAddressAsync(ParamsOfConvertAddress @params)
        {
            return await _client.CallFunctionAsync<ResultOfConvertAddress>("utils.convert_address", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfCalcStorageFee> CalcStorageFeeAsync(ParamsOfCalcStorageFee @params)
        {
            return await _client.CallFunctionAsync<ResultOfCalcStorageFee>("utils.calc_storage_fee", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfCompressZstd> CompressZstdAsync(ParamsOfCompressZstd @params)
        {
            return await _client.CallFunctionAsync<ResultOfCompressZstd>("utils.compress_zstd", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfDecompressZstd> DecompressZstdAsync(ParamsOfDecompressZstd @params)
        {
            return await _client.CallFunctionAsync<ResultOfDecompressZstd>("utils.decompress_zstd", @params).ConfigureAwait(false);
        }
    }
}

namespace TonSdk
{
    public partial interface ITonClient
    {
        IUtilsModule Utils { get; }
    }

    public partial class TonClient
    {
        private UtilsModule _utilsModule;

        public IUtilsModule Utils
        {
            get
            {
                return _utilsModule ?? (_utilsModule = new UtilsModule(this));
            }
        }
    }
}

