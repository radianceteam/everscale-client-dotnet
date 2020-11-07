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
        public uint? BlockTime { get; set; }

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

    public abstract class AccountForExecutor
    {
        /// <summary>
        ///  Non-existing account to run a creation internal message.
        ///  Should be used with `skip_transaction_check = true` if the message has no deploy data
        ///  since transactions on the uninitialized account are always aborted
        /// </summary>
        public class None : AccountForExecutor
        {
        }

        /// <summary>
        ///  Emulate uninitialized account to run deploy message
        /// </summary>
        public class Uninit : AccountForExecutor
        {
        }

        /// <summary>
        ///  Account state to run message
        /// </summary>
        public class Account : AccountForExecutor
        {
            /// <summary>
            ///  Account BOC. Encoded as base64.
            /// </summary>
            [JsonProperty("boc")]
            public string Boc { get; set; }

            /// <summary>
            ///  Flag for running account with the unlimited balance. Can be used to calculate
            ///  transaction fees without balance check
            /// </summary>
            [JsonProperty("unlimited_balance")]
            public bool? UnlimitedBalance { get; set; }
        }
    }

    public class TransactionFees
    {
        [JsonProperty("in_msg_fwd_fee")]
        public BigInteger InMsgFwdFee { get; set; }

        [JsonProperty("storage_fee")]
        public BigInteger StorageFee { get; set; }

        [JsonProperty("gas_fee")]
        public BigInteger GasFee { get; set; }

        [JsonProperty("out_msgs_fwd_fee")]
        public BigInteger OutMsgsFwdFee { get; set; }

        [JsonProperty("total_account_fees")]
        public BigInteger TotalAccountFees { get; set; }

        [JsonProperty("total_output")]
        public BigInteger TotalOutput { get; set; }
    }

    public class ParamsOfRunExecutor
    {
        /// <summary>
        ///  Input message BOC. Must be encoded as base64.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        ///  Account to run on executor
        /// </summary>
        [JsonProperty("account")]
        public AccountForExecutor Account { get; set; }

        /// <summary>
        ///  Execution options.
        /// </summary>
        [JsonProperty("execution_options")]
        public ExecutionOptions ExecutionOptions { get; set; }

        /// <summary>
        ///  Contract ABI for decoding output messages
        /// </summary>
        [JsonProperty("abi")]
        public Abi Abi { get; set; }

        /// <summary>
        ///  Skip transaction check flag
        /// </summary>
        [JsonProperty("skip_transaction_check")]
        public bool? SkipTransactionCheck { get; set; }
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
        public Newtonsoft.Json.Linq.JToken Transaction { get; set; }

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
        public TransactionFees Fees { get; set; }
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
        public Newtonsoft.Json.Linq.JToken Input { get; set; }

        [JsonProperty("execution_options")]
        public ExecutionOptions ExecutionOptions { get; set; }
    }

    public class ResultOfRunGet
    {
        /// <summary>
        ///  Values returned by getmethod on stack
        /// </summary>
        [JsonProperty("output")]
        public Newtonsoft.Json.Linq.JToken Output { get; set; }
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

