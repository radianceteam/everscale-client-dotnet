using Newtonsoft.Json;
using System;
using System.Numerics;
using System.Threading.Tasks;
using TonSdk.Modules;

/*
* TON API version 1.22.0, tvm module.
* THIS FILE WAS GENERATED AUTOMATICALLY.
*/

namespace TonSdk.Modules
{
    public enum TvmErrorCode
    {
        CanNotReadTransaction = 401,
        CanNotReadBlockchainConfig = 402,
        TransactionAborted = 403,
        InternalError = 404,
        ActionPhaseFailed = 405,
        AccountCodeMissing = 406,
        LowBalance = 407,
        AccountFrozenOrDeleted = 408,
        AccountMissing = 409,
        UnknownExecutionError = 410,
        InvalidInputStack = 411,
        InvalidAccountBoc = 412,
        InvalidMessageType = 413,
        ContractExecutionError = 414,
    }

    public class ExecutionOptions
    {
        /// <summary>
        /// boc with config
        /// </summary>
        [JsonProperty("blockchain_config", NullValueHandling = NullValueHandling.Ignore)]
        public string BlockchainConfig { get; set; }

        /// <summary>
        /// time that is used as transaction time
        /// </summary>
        [JsonProperty("block_time", NullValueHandling = NullValueHandling.Ignore)]
        public uint? BlockTime { get; set; }

        /// <summary>
        /// block logical time
        /// </summary>
        [JsonProperty("block_lt", NullValueHandling = NullValueHandling.Ignore)]
        public BigInteger BlockLt { get; set; }

        /// <summary>
        /// transaction logical time
        /// </summary>
        [JsonProperty("transaction_lt", NullValueHandling = NullValueHandling.Ignore)]
        public BigInteger TransactionLt { get; set; }
    }

    public abstract class AccountForExecutor
    {
        /// <summary>
        /// Non-existing account to run a creation internal message. Should be used with `skip_transaction_check
        /// = true` if the message has no deploy data since transactions on the uninitialized account are always
        /// aborted
        /// </summary>
        public class None : AccountForExecutor
        {
        }

        /// <summary>
        /// Emulate uninitialized account to run deploy message
        /// </summary>
        public class Uninit : AccountForExecutor
        {
        }

        /// <summary>
        /// Account state to run message
        /// </summary>
        public class Account : AccountForExecutor
        {
            /// <summary>
            /// Encoded as base64.
            /// </summary>
            [JsonProperty("boc", NullValueHandling = NullValueHandling.Ignore)]
            public string Boc { get; set; }

            /// <summary>
            /// Can be used to calculate transaction fees without balance check
            /// </summary>
            [JsonProperty("unlimited_balance", NullValueHandling = NullValueHandling.Ignore)]
            public bool? UnlimitedBalance { get; set; }
        }
    }

    public class TransactionFees
    {
        [JsonProperty("in_msg_fwd_fee", NullValueHandling = NullValueHandling.Ignore)]
        public BigInteger InMsgFwdFee { get; set; }

        [JsonProperty("storage_fee", NullValueHandling = NullValueHandling.Ignore)]
        public BigInteger StorageFee { get; set; }

        [JsonProperty("gas_fee", NullValueHandling = NullValueHandling.Ignore)]
        public BigInteger GasFee { get; set; }

        [JsonProperty("out_msgs_fwd_fee", NullValueHandling = NullValueHandling.Ignore)]
        public BigInteger OutMsgsFwdFee { get; set; }

        [JsonProperty("total_account_fees", NullValueHandling = NullValueHandling.Ignore)]
        public BigInteger TotalAccountFees { get; set; }

        [JsonProperty("total_output", NullValueHandling = NullValueHandling.Ignore)]
        public BigInteger TotalOutput { get; set; }
    }

    public class ParamsOfRunExecutor
    {
        /// <summary>
        /// Must be encoded as base64.
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        /// Account to run on executor
        /// </summary>
        [JsonProperty("account", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public AccountForExecutor Account { get; set; }

        /// <summary>
        /// Execution options.
        /// </summary>
        [JsonProperty("execution_options", NullValueHandling = NullValueHandling.Ignore)]
        public ExecutionOptions ExecutionOptions { get; set; }

        /// <summary>
        /// Contract ABI for decoding output messages
        /// </summary>
        [JsonProperty("abi", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public Abi Abi { get; set; }

        /// <summary>
        /// Skip transaction check flag
        /// </summary>
        [JsonProperty("skip_transaction_check", NullValueHandling = NullValueHandling.Ignore)]
        public bool? SkipTransactionCheck { get; set; }

        /// <summary>
        /// The BOC itself returned if no cache type provided
        /// </summary>
        [JsonProperty("boc_cache", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public BocCacheType BocCache { get; set; }

        /// <summary>
        /// Empty string is returned if the flag is `false`
        /// </summary>
        [JsonProperty("return_updated_account", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ReturnUpdatedAccount { get; set; }
    }

    public class ResultOfRunExecutor
    {
        /// <summary>
        /// In addition to the regular transaction fields there is a
        /// `boc` field encoded with `base64` which contains source
        /// transaction BOC.
        /// </summary>
        [JsonProperty("transaction", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Transaction { get; set; }

        /// <summary>
        /// Encoded as `base64`
        /// </summary>
        [JsonProperty("out_messages", NullValueHandling = NullValueHandling.Ignore)]
        public string[] OutMessages { get; set; }

        /// <summary>
        /// Optional decoded message bodies according to the optional `abi` parameter.
        /// </summary>
        [JsonProperty("decoded", NullValueHandling = NullValueHandling.Ignore)]
        public DecodedOutput Decoded { get; set; }

        /// <summary>
        /// Encoded as `base64`
        /// </summary>
        [JsonProperty("account", NullValueHandling = NullValueHandling.Ignore)]
        public string Account { get; set; }

        /// <summary>
        /// Transaction fees
        /// </summary>
        [JsonProperty("fees", NullValueHandling = NullValueHandling.Ignore)]
        public TransactionFees Fees { get; set; }
    }

    public class ParamsOfRunTvm
    {
        /// <summary>
        /// Must be encoded as base64.
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        /// Must be encoded as base64.
        /// </summary>
        [JsonProperty("account", NullValueHandling = NullValueHandling.Ignore)]
        public string Account { get; set; }

        /// <summary>
        /// Execution options.
        /// </summary>
        [JsonProperty("execution_options", NullValueHandling = NullValueHandling.Ignore)]
        public ExecutionOptions ExecutionOptions { get; set; }

        /// <summary>
        /// Contract ABI for decoding output messages
        /// </summary>
        [JsonProperty("abi", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public Abi Abi { get; set; }

        /// <summary>
        /// The BOC itself returned if no cache type provided
        /// </summary>
        [JsonProperty("boc_cache", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public BocCacheType BocCache { get; set; }

        /// <summary>
        /// Empty string is returned if the flag is `false`
        /// </summary>
        [JsonProperty("return_updated_account", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ReturnUpdatedAccount { get; set; }
    }

    public class ResultOfRunTvm
    {
        /// <summary>
        /// Encoded as `base64`
        /// </summary>
        [JsonProperty("out_messages", NullValueHandling = NullValueHandling.Ignore)]
        public string[] OutMessages { get; set; }

        /// <summary>
        /// Optional decoded message bodies according to the optional `abi` parameter.
        /// </summary>
        [JsonProperty("decoded", NullValueHandling = NullValueHandling.Ignore)]
        public DecodedOutput Decoded { get; set; }

        /// <summary>
        /// Encoded as `base64`. Attention! Only `account_state.storage.state.data` part of the BOC is updated.
        /// </summary>
        [JsonProperty("account", NullValueHandling = NullValueHandling.Ignore)]
        public string Account { get; set; }
    }

    public class ParamsOfRunGet
    {
        /// <summary>
        /// Account BOC in `base64`
        /// </summary>
        [JsonProperty("account", NullValueHandling = NullValueHandling.Ignore)]
        public string Account { get; set; }

        /// <summary>
        /// Function name
        /// </summary>
        [JsonProperty("function_name", NullValueHandling = NullValueHandling.Ignore)]
        public string FunctionName { get; set; }

        /// <summary>
        /// Input parameters
        /// </summary>
        [JsonProperty("input", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Input { get; set; }

        /// <summary>
        /// Execution options
        /// </summary>
        [JsonProperty("execution_options", NullValueHandling = NullValueHandling.Ignore)]
        public ExecutionOptions ExecutionOptions { get; set; }

        /// <summary>
        /// Default is `false`. Input parameters may use any of lists representations
        /// If you receive this error on Web: "Runtime error. Unreachable code should not be executed...",
        /// set this flag to true.
        /// This may happen, for example, when elector contract contains too many participants
        /// </summary>
        [JsonProperty("tuple_list_as_array", NullValueHandling = NullValueHandling.Ignore)]
        public bool? TupleListAsArray { get; set; }
    }

    public class ResultOfRunGet
    {
        /// <summary>
        /// Values returned by get-method on stack
        /// </summary>
        [JsonProperty("output", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Output { get; set; }
    }

    public interface ITvmModule
    {
        /// <summary>
        /// Performs all the phases of contract execution on Transaction Executor -
        /// the same component that is used on Validator Nodes.
        /// 
        /// Can be used for contract debugging, to find out the reason why a message was not delivered
        /// successfully.
        /// Validators throw away the failed external inbound messages (if they failed bedore `ACCEPT`) in the
        /// real network.
        /// This is why these messages are impossible to debug in the real network.
        /// With the help of run_executor you can do that. In fact, `process_message` function
        /// performs local check with `run_executor` if there was no transaction as a result of processing
        /// and returns the error, if there is one.
        /// 
        /// Another use case to use `run_executor` is to estimate fees for message execution.
        /// Set  `AccountForExecutor::Account.unlimited_balance`
        /// to `true` so that emulation will not depend on the actual balance.
        /// This may be needed to calculate deploy fees for an account that does not exist yet.
        /// JSON with fees is in `fees` field of the result.
        /// 
        /// One more use case - you can produce the sequence of operations,
        /// thus emulating the sequential contract calls locally.
        /// And so on.
        /// 
        /// Transaction executor requires account BOC (bag of cells) as a parameter.
        /// To get the account BOC - use `net.query` method to download it from GraphQL API
        /// (field `boc` of `account`) or generate it with `abi.encode_account` method.
        /// 
        /// Also it requires message BOC. To get the message BOC - use `abi.encode_message` or
        /// `abi.encode_internal_message`.
        /// 
        /// If you need this emulation to be as precise as possible (for instance - emulate transaction
        /// with particular lt in particular block or use particular blockchain config,
        /// downloaded from a particular key block - then specify `execution_options` parameter.
        /// 
        /// If you need to see the aborted transaction as a result, not as an error, set
        /// `skip_transaction_check` to `true`.
        /// </summary>
        Task<ResultOfRunExecutor> RunExecutorAsync(ParamsOfRunExecutor @params);

        /// <summary>
        /// Performs only a part of compute phase of transaction execution
        /// that is used to run get-methods of ABI-compatible contracts.
        /// 
        /// If you try to run get-methods with `run_executor` you will get an error, because it checks ACCEPT
        /// and exits
        /// if there is none, which is actually true for get-methods.
        /// 
        ///  To get the account BOC (bag of cells) - use `net.query` method to download it from GraphQL API
        /// (field `boc` of `account`) or generate it with `abi.encode_account method`.
        /// To get the message BOC - use `abi.encode_message` or prepare it any other way, for instance, with
        /// FIFT script.
        /// 
        /// Attention! Updated account state is produces as well, but only
        /// `account_state.storage.state.data`  part of the BOC is updated.
        /// </summary>
        Task<ResultOfRunTvm> RunTvmAsync(ParamsOfRunTvm @params);

        /// <summary>
        /// Executes a get-method of FIFT contract that fulfills the smc-guidelines
        /// https://test.ton.org/smc-guidelines.txt
        /// and returns the result data from TVM's stack
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

