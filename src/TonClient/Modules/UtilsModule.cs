using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Numerics;
using System.Threading.Tasks;
using TonSdk.Modules;

/*
* TON API version 1.26.0, utils module.
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

    public enum AccountAddressType
    {
        AccountId,
        Hex,
        Base64,
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

    public class ParamsOfGetAddressType
    {
        /// <summary>
        /// Account address in any TON format.
        /// </summary>
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }
    }

    public class ResultOfGetAddressType
    {
        /// <summary>
        /// Account address type.
        /// </summary>
        [JsonProperty("address_type", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public AccountAddressType AddressType { get; set; }
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
        /// Address types are the following
        /// 
        /// `0:919db8e740d50bf349df2eea03fa30c385d846b991ff5542e67098ee833fc7f7` - standard TON address most
        /// commonly used in all cases. Also called as hex address
        /// `919db8e740d50bf349df2eea03fa30c385d846b991ff5542e67098ee833fc7f7` - account ID. A part of full
        /// address. Identifies account inside particular workchain
        /// `EQCRnbjnQNUL80nfLuoD+jDDhdhGuZH/VULmcJjugz/H9wam` - base64 address. Also called "user-friendly".
        /// Was used at the beginning of TON. Now it is supported for compatibility
        /// </summary>
        Task<ResultOfGetAddressType> GetAddressTypeAsync(ParamsOfGetAddressType @params);

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

        public async Task<ResultOfGetAddressType> GetAddressTypeAsync(ParamsOfGetAddressType @params)
        {
            return await _client.CallFunctionAsync<ResultOfGetAddressType>("utils.get_address_type", @params).ConfigureAwait(false);
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

