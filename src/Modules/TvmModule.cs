using Newtonsoft.Json;
using System;
using System.Numerics;
using System.Threading.Tasks;
using TonSdk.Modules;

/*
* TON API version 1.0.0, tvm module.
* THIS FILE WAS GENERATED AUTOMATICALLY.
*/

namespace TonSdk.Modules
{
    public class ExecutionOptions
    {
        /// <summary>
        ///  boc with config
        /// </summary>
        [JsonProperty("blockchain_config")]
        public string BlockchainConfig { get; set; }

        /// <summary>
        ///  time that is used as transaction time
        /// </summary>
        [JsonProperty("block_time")]
        public int? BlockTime { get; set; }

        /// <summary>
        ///  block logical time
        /// </summary>
        [JsonProperty("block_lt")]
        public BigInteger BlockLt { get; set; }

        /// <summary>
        ///  transaction logical time
        /// </summary>
        [JsonProperty("transaction_lt")]
        public BigInteger TransactionLt { get; set; }
    }

    public class ParamsOfRunExecutor
    {
        /// <summary>
        ///  Input message BOC. Must be encoded as base64.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        ///  Account BOC. Must be encoded as base64.
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }

        /// <summary>
        ///  Execution options.
        /// </summary>
        [JsonProperty("execution_options")]
        public ExecutionOptions ExecutionOptions { get; set; }

        /// <summary>
        ///  Contract ABI for dedcoding output messages
        /// </summary>
        [JsonProperty("abi")]
        public Abi Abi { get; set; }
    }

    public class ResultOfRunExecutor
    {
        /// <summary>
        ///  Parsed transaction.
        /// 
        ///  In addition to the regular transaction fields there is a
        ///  `boc` field encoded with `base64` which contains source
        ///  transaction BOC.
        /// </summary>
        [JsonProperty("transaction")]
        public Newtonsoft.Json.Linq.JRaw Transaction { get; set; }

        /// <summary>
        ///  List of output messages' BOCs. Encoded as `base64`
        /// </summary>
        [JsonProperty("out_messages")]
        public string[] OutMessages { get; set; }

        /// <summary>
        ///  Optional decoded message bodies according to the optional
        ///  `abi` parameter.
        /// </summary>
        [JsonProperty("decoded")]
        public DecodedOutput Decoded { get; set; }

        /// <summary>
        ///  Updated account state BOC. Encoded as `base64`
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }

        /// <summary>
        ///  Transaction fees
        /// </summary>
        [JsonProperty("fees")]
        public Newtonsoft.Json.Linq.JRaw Fees { get; set; }
    }

    public class ParamsOfRunTvm
    {
        /// <summary>
        ///  Input message BOC. Must be encoded as base64.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        ///  Account BOC. Must be encoded as base64.
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }

        /// <summary>
        ///  Execution options.
        /// </summary>
        [JsonProperty("execution_options")]
        public ExecutionOptions ExecutionOptions { get; set; }

        /// <summary>
        ///  Contract ABI for dedcoding output messages
        /// </summary>
        [JsonProperty("abi")]
        public Abi Abi { get; set; }
    }

    public class ResultOfRunTvm
    {
        /// <summary>
        ///  List of output messages' BOCs. Encoded as `base64`
        /// </summary>
        [JsonProperty("out_messages")]
        public string[] OutMessages { get; set; }

        /// <summary>
        ///  Optional decoded message bodies according to the optional
        ///  `abi` parameter.
        /// </summary>
        [JsonProperty("decoded")]
        public DecodedOutput Decoded { get; set; }

        /// <summary>
        ///  Updated account state BOC. Encoded as `base64`.
        ///  Attention! Only data in account state is updated.
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }
    }

    public class ParamsOfRunGet
    {
        /// <summary>
        ///  Account BOC in `base64`
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }

        /// <summary>
        ///  Function name
        /// </summary>
        [JsonProperty("function_name")]
        public string FunctionName { get; set; }

        /// <summary>
        ///  Input parameters
        /// </summary>
        [JsonProperty("input")]
        public Newtonsoft.Json.Linq.JRaw Input { get; set; }

        [JsonProperty("execution_options")]
        public ExecutionOptions ExecutionOptions { get; set; }
    }

    public class ResultOfRunGet
    {
        /// <summary>
        ///  Values returned by getmethod on stack
        /// </summary>
        [JsonProperty("output")]
        public Newtonsoft.Json.Linq.JRaw Output { get; set; }
    }

    public interface ITvmModule
    {
        Task<ResultOfRunExecutor> RunExecutorAsync(ParamsOfRunExecutor @params);

        Task<ResultOfRunTvm> RunTvmAsync(ParamsOfRunTvm @params);

        /// <summary>
        ///  Executes getmethod and returns data from TVM stack
        /// </summary>
        Task<ResultOfRunGet> RunGetAsync(ParamsOfRunGet @params);
    }

    internal class TvmModule : ITvmModule
    {
        private readonly TonClient _client;

        internal TvmModule(TonClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<ResultOfRunExecutor> RunExecutorAsync(ParamsOfRunExecutor @params)
        {
            return await _client.CallFunctionAsync<ResultOfRunExecutor>("tvm.run_executor", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfRunTvm> RunTvmAsync(ParamsOfRunTvm @params)
        {
            return await _client.CallFunctionAsync<ResultOfRunTvm>("tvm.run_tvm", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfRunGet> RunGetAsync(ParamsOfRunGet @params)
        {
            return await _client.CallFunctionAsync<ResultOfRunGet>("tvm.run_get", @params).ConfigureAwait(false);
        }
    }
}

namespace TonSdk
{
    public partial interface ITonClient
    {
        ITvmModule Tvm { get; }
    }

    public partial class TonClient
    {
        private TvmModule _tvmModule;

        public ITvmModule Tvm
        {
            get
            {
                return _tvmModule ?? (_tvmModule = new TvmModule(this));
            }
        }
    }
}

