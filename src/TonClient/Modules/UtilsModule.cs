using Newtonsoft.Json;
using System;
using System.Numerics;
using System.Threading.Tasks;
using TonSdk.Modules;

/*
* TON API version 1.11.0, utils module.
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
        [JsonConverter(typeof(PolymorphicConcreteTypeConverter))]
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

