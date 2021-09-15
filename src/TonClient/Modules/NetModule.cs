using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Numerics;
using System.Threading.Tasks;
using TonSdk.Modules;

/*
* TON API version 1.21.5, net module.
* THIS FILE WAS GENERATED AUTOMATICALLY.
*/

namespace TonSdk.Modules
{
    public enum NetErrorCode
    {
        QueryFailed = 601,
        SubscribeFailed = 602,
        WaitForFailed = 603,
        GetSubscriptionResultFailed = 604,
        InvalidServerResponse = 605,
        ClockOutOfSync = 606,
        WaitForTimeout = 607,
        GraphqlError = 608,
        NetworkModuleSuspended = 609,
        WebsocketDisconnected = 610,
        NotSupported = 611,
        NoEndpointsProvided = 612,
        GraphqlWebsocketInitError = 613,
        NetworkModuleResumed = 614,
    }

    public class OrderBy
    {
        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string Path { get; set; }

        [JsonProperty("direction", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public SortDirection Direction { get; set; }
    }

    public enum SortDirection
    {
        ASC,
        DESC,
    }

    public abstract class ParamsOfQueryOperation
    {
        public class QueryCollection : ParamsOfQueryOperation
        {
            /// <summary>
            /// Collection name (accounts, blocks, transactions, messages, block_signatures)
            /// </summary>
            [JsonProperty("collection", NullValueHandling = NullValueHandling.Ignore)]
            public string Collection { get; set; }

            /// <summary>
            /// Collection filter
            /// </summary>
            [JsonProperty("filter", NullValueHandling = NullValueHandling.Ignore)]
            public Newtonsoft.Json.Linq.JToken Filter { get; set; }

            /// <summary>
            /// Projection (result) string
            /// </summary>
            [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
            public string Result { get; set; }

            /// <summary>
            /// Sorting order
            /// </summary>
            [JsonProperty("order", NullValueHandling = NullValueHandling.Ignore)]
            public OrderBy[] Order { get; set; }

            /// <summary>
            /// Number of documents to return
            /// </summary>
            [JsonProperty("limit", NullValueHandling = NullValueHandling.Ignore)]
            public uint? Limit { get; set; }
        }

        public class WaitForCollection : ParamsOfQueryOperation
        {
            /// <summary>
            /// Collection name (accounts, blocks, transactions, messages, block_signatures)
            /// </summary>
            [JsonProperty("collection", NullValueHandling = NullValueHandling.Ignore)]
            public string Collection { get; set; }

            /// <summary>
            /// Collection filter
            /// </summary>
            [JsonProperty("filter", NullValueHandling = NullValueHandling.Ignore)]
            public Newtonsoft.Json.Linq.JToken Filter { get; set; }

            /// <summary>
            /// Projection (result) string
            /// </summary>
            [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
            public string Result { get; set; }

            /// <summary>
            /// Query timeout
            /// </summary>
            [JsonProperty("timeout", NullValueHandling = NullValueHandling.Ignore)]
            public uint? Timeout { get; set; }
        }

        public class AggregateCollection : ParamsOfQueryOperation
        {
            /// <summary>
            /// Collection name (accounts, blocks, transactions, messages, block_signatures)
            /// </summary>
            [JsonProperty("collection", NullValueHandling = NullValueHandling.Ignore)]
            public string Collection { get; set; }

            /// <summary>
            /// Collection filter
            /// </summary>
            [JsonProperty("filter", NullValueHandling = NullValueHandling.Ignore)]
            public Newtonsoft.Json.Linq.JToken Filter { get; set; }

            /// <summary>
            /// Projection (result) string
            /// </summary>
            [JsonProperty("fields", NullValueHandling = NullValueHandling.Ignore)]
            public FieldAggregation[] Fields { get; set; }
        }

        public class QueryCounterparties : ParamsOfQueryOperation
        {
            /// <summary>
            /// Account address
            /// </summary>
            [JsonProperty("account", NullValueHandling = NullValueHandling.Ignore)]
            public string Account { get; set; }

            /// <summary>
            /// Projection (result) string
            /// </summary>
            [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
            public string Result { get; set; }

            /// <summary>
            /// Number of counterparties to return
            /// </summary>
            [JsonProperty("first", NullValueHandling = NullValueHandling.Ignore)]
            public uint? First { get; set; }

            /// <summary>
            /// `cursor` field of the last received result
            /// </summary>
            [JsonProperty("after", NullValueHandling = NullValueHandling.Ignore)]
            public string After { get; set; }
        }
    }

    public class FieldAggregation
    {
        /// <summary>
        /// Dot separated path to the field
        /// </summary>
        [JsonProperty("field", NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; set; }

        /// <summary>
        /// Aggregation function that must be applied to field values
        /// </summary>
        [JsonProperty("fn", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public AggregationFn Fn { get; set; }
    }

    public enum AggregationFn
    {
        /// <summary>
        /// Returns count of filtered record
        /// </summary>
        COUNT,
        /// <summary>
        /// Returns the minimal value for a field in filtered records
        /// </summary>
        MIN,
        /// <summary>
        /// Returns the maximal value for a field in filtered records
        /// </summary>
        MAX,
        /// <summary>
        /// Returns a sum of values for a field in filtered records
        /// </summary>
        SUM,
        /// <summary>
        /// Returns an average value for a field in filtered records
        /// </summary>
        AVERAGE,
    }

    public class TransactionNode
    {
        /// <summary>
        /// Transaction id.
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        /// In message id.
        /// </summary>
        [JsonProperty("in_msg", NullValueHandling = NullValueHandling.Ignore)]
        public string InMsg { get; set; }

        /// <summary>
        /// Out message ids.
        /// </summary>
        [JsonProperty("out_msgs", NullValueHandling = NullValueHandling.Ignore)]
        public string[] OutMsgs { get; set; }

        /// <summary>
        /// Account address.
        /// </summary>
        [JsonProperty("account_addr", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountAddr { get; set; }

        /// <summary>
        /// Transactions total fees.
        /// </summary>
        [JsonProperty("total_fees", NullValueHandling = NullValueHandling.Ignore)]
        public string TotalFees { get; set; }

        /// <summary>
        /// Aborted flag.
        /// </summary>
        [JsonProperty("aborted", NullValueHandling = NullValueHandling.Ignore)]
        public bool Aborted { get; set; }

        /// <summary>
        /// Compute phase exit code.
        /// </summary>
        [JsonProperty("exit_code", NullValueHandling = NullValueHandling.Ignore)]
        public uint? ExitCode { get; set; }
    }

    public class MessageNode
    {
        /// <summary>
        /// Message id.
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        /// This field is missing for an external inbound messages.
        /// </summary>
        [JsonProperty("src_transaction_id", NullValueHandling = NullValueHandling.Ignore)]
        public string SrcTransactionId { get; set; }

        /// <summary>
        /// This field is missing for an external outbound messages.
        /// </summary>
        [JsonProperty("dst_transaction_id", NullValueHandling = NullValueHandling.Ignore)]
        public string DstTransactionId { get; set; }

        /// <summary>
        /// Source address.
        /// </summary>
        [JsonProperty("src", NullValueHandling = NullValueHandling.Ignore)]
        public string Src { get; set; }

        /// <summary>
        /// Destination address.
        /// </summary>
        [JsonProperty("dst", NullValueHandling = NullValueHandling.Ignore)]
        public string Dst { get; set; }

        /// <summary>
        /// Transferred tokens value.
        /// </summary>
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        /// <summary>
        /// Bounce flag.
        /// </summary>
        [JsonProperty("bounce", NullValueHandling = NullValueHandling.Ignore)]
        public bool Bounce { get; set; }

        /// <summary>
        /// Library tries to decode message body using provided `params.abi_registry`.
        /// This field will be missing if none of the provided abi can be used to decode.
        /// </summary>
        [JsonProperty("decoded_body", NullValueHandling = NullValueHandling.Ignore)]
        public DecodedMessageBody DecodedBody { get; set; }
    }

    public class ParamsOfQuery
    {
        /// <summary>
        /// GraphQL query text.
        /// </summary>
        [JsonProperty("query", NullValueHandling = NullValueHandling.Ignore)]
        public string Query { get; set; }

        /// <summary>
        /// Must be a map with named values that can be used in query.
        /// </summary>
        [JsonProperty("variables", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Variables { get; set; }
    }

    public class ResultOfQuery
    {
        /// <summary>
        /// Result provided by DAppServer.
        /// </summary>
        [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Result { get; set; }
    }

    public class ParamsOfBatchQuery
    {
        /// <summary>
        /// List of query operations that must be performed per single fetch.
        /// </summary>
        [JsonProperty("operations", NullValueHandling = NullValueHandling.Ignore,
            ItemConverterType = typeof(PolymorphicTypeConverter))]
        public ParamsOfQueryOperation[] Operations { get; set; }
    }

    public class ResultOfBatchQuery
    {
        /// <summary>
        /// Returns an array of values. Each value corresponds to `queries` item.
        /// </summary>
        [JsonProperty("results", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken[] Results { get; set; }
    }

    public class ParamsOfQueryCollection
    {
        /// <summary>
        /// Collection name (accounts, blocks, transactions, messages, block_signatures)
        /// </summary>
        [JsonProperty("collection", NullValueHandling = NullValueHandling.Ignore)]
        public string Collection { get; set; }

        /// <summary>
        /// Collection filter
        /// </summary>
        [JsonProperty("filter", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Filter { get; set; }

        /// <summary>
        /// Projection (result) string
        /// </summary>
        [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
        public string Result { get; set; }

        /// <summary>
        /// Sorting order
        /// </summary>
        [JsonProperty("order", NullValueHandling = NullValueHandling.Ignore)]
        public OrderBy[] Order { get; set; }

        /// <summary>
        /// Number of documents to return
        /// </summary>
        [JsonProperty("limit", NullValueHandling = NullValueHandling.Ignore)]
        public uint? Limit { get; set; }
    }

    public class ResultOfQueryCollection
    {
        /// <summary>
        /// Objects that match the provided criteria
        /// </summary>
        [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken[] Result { get; set; }
    }

    public class ParamsOfAggregateCollection
    {
        /// <summary>
        /// Collection name (accounts, blocks, transactions, messages, block_signatures)
        /// </summary>
        [JsonProperty("collection", NullValueHandling = NullValueHandling.Ignore)]
        public string Collection { get; set; }

        /// <summary>
        /// Collection filter
        /// </summary>
        [JsonProperty("filter", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Filter { get; set; }

        /// <summary>
        /// Projection (result) string
        /// </summary>
        [JsonProperty("fields", NullValueHandling = NullValueHandling.Ignore)]
        public FieldAggregation[] Fields { get; set; }
    }

    public class ResultOfAggregateCollection
    {
        /// <summary>
        /// Returns an array of strings. Each string refers to the corresponding `fields` item.
        /// Numeric value is returned as a decimal string representations.
        /// </summary>
        [JsonProperty("values", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Values { get; set; }
    }

    public class ParamsOfWaitForCollection
    {
        /// <summary>
        /// Collection name (accounts, blocks, transactions, messages, block_signatures)
        /// </summary>
        [JsonProperty("collection", NullValueHandling = NullValueHandling.Ignore)]
        public string Collection { get; set; }

        /// <summary>
        /// Collection filter
        /// </summary>
        [JsonProperty("filter", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Filter { get; set; }

        /// <summary>
        /// Projection (result) string
        /// </summary>
        [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
        public string Result { get; set; }

        /// <summary>
        /// Query timeout
        /// </summary>
        [JsonProperty("timeout", NullValueHandling = NullValueHandling.Ignore)]
        public uint? Timeout { get; set; }
    }

    public class ResultOfWaitForCollection
    {
        /// <summary>
        /// First found object that matches the provided criteria
        /// </summary>
        [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Result { get; set; }
    }

    public class ResultOfSubscribeCollection
    {
        /// <summary>
        /// Must be closed with `unsubscribe`
        /// </summary>
        [JsonProperty("handle", NullValueHandling = NullValueHandling.Ignore)]
        public uint Handle { get; set; }
    }

    public class ParamsOfSubscribeCollection
    {
        /// <summary>
        /// Collection name (accounts, blocks, transactions, messages, block_signatures)
        /// </summary>
        [JsonProperty("collection", NullValueHandling = NullValueHandling.Ignore)]
        public string Collection { get; set; }

        /// <summary>
        /// Collection filter
        /// </summary>
        [JsonProperty("filter", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Filter { get; set; }

        /// <summary>
        /// Projection (result) string
        /// </summary>
        [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
        public string Result { get; set; }
    }

    public class ParamsOfFindLastShardBlock
    {
        /// <summary>
        /// Account address
        /// </summary>
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }
    }

    public class ResultOfFindLastShardBlock
    {
        /// <summary>
        /// Account shard last block ID
        /// </summary>
        [JsonProperty("block_id", NullValueHandling = NullValueHandling.Ignore)]
        public string BlockId { get; set; }
    }

    public class EndpointsSet
    {
        /// <summary>
        /// List of endpoints provided by server
        /// </summary>
        [JsonProperty("endpoints", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Endpoints { get; set; }
    }

    public class ResultOfGetEndpoints
    {
        /// <summary>
        /// Current query endpoint
        /// </summary>
        [JsonProperty("query", NullValueHandling = NullValueHandling.Ignore)]
        public string Query { get; set; }

        /// <summary>
        /// List of all endpoints used by client
        /// </summary>
        [JsonProperty("endpoints", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Endpoints { get; set; }
    }

    public class ParamsOfQueryCounterparties
    {
        /// <summary>
        /// Account address
        /// </summary>
        [JsonProperty("account", NullValueHandling = NullValueHandling.Ignore)]
        public string Account { get; set; }

        /// <summary>
        /// Projection (result) string
        /// </summary>
        [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
        public string Result { get; set; }

        /// <summary>
        /// Number of counterparties to return
        /// </summary>
        [JsonProperty("first", NullValueHandling = NullValueHandling.Ignore)]
        public uint? First { get; set; }

        /// <summary>
        /// `cursor` field of the last received result
        /// </summary>
        [JsonProperty("after", NullValueHandling = NullValueHandling.Ignore)]
        public string After { get; set; }
    }

    public class ParamsOfQueryTransactionTree
    {
        /// <summary>
        /// Input message id.
        /// </summary>
        [JsonProperty("in_msg", NullValueHandling = NullValueHandling.Ignore)]
        public string InMsg { get; set; }

        /// <summary>
        /// List of contract ABIs that will be used to decode message bodies. Library will try to decode each
        /// returned message body using any ABI from the registry.
        /// </summary>
        [JsonProperty("abi_registry", NullValueHandling = NullValueHandling.Ignore,
            ItemConverterType = typeof(PolymorphicTypeConverter))]
        public Abi[] AbiRegistry { get; set; }

        /// <summary>
        /// If some of the following messages and transactions are missing yet
        /// The maximum waiting time is regulated by this option.
        /// 
        /// Default value is 60000 (1 min).
        /// </summary>
        [JsonProperty("timeout", NullValueHandling = NullValueHandling.Ignore)]
        public uint? Timeout { get; set; }
    }

    public class ResultOfQueryTransactionTree
    {
        /// <summary>
        /// Messages.
        /// </summary>
        [JsonProperty("messages", NullValueHandling = NullValueHandling.Ignore)]
        public MessageNode[] Messages { get; set; }

        /// <summary>
        /// Transactions.
        /// </summary>
        [JsonProperty("transactions", NullValueHandling = NullValueHandling.Ignore)]
        public TransactionNode[] Transactions { get; set; }
    }

    public class ParamsOfCreateBlockIterator
    {
        /// <summary>
        /// If the application specifies this parameter then the iteration
        /// includes blocks with `gen_utime` >= `start_time`.
        /// Otherwise the iteration starts from zero state.
        /// 
        /// Must be specified in seconds.
        /// </summary>
        [JsonProperty("start_time", NullValueHandling = NullValueHandling.Ignore)]
        public uint? StartTime { get; set; }

        /// <summary>
        /// If the application specifies this parameter then the iteration
        /// includes blocks with `gen_utime` < `end_time`.
        /// Otherwise the iteration never stops.
        /// 
        /// Must be specified in seconds.
        /// </summary>
        [JsonProperty("end_time", NullValueHandling = NullValueHandling.Ignore)]
        public uint? EndTime { get; set; }

        /// <summary>
        /// If the application specifies this parameter and it is not the empty array
        /// then the iteration will include items related to accounts that belongs to
        /// the specified shard prefixes.
        /// Shard prefix must be represented as a string "workchain:prefix".
        /// Where `workchain` is a signed integer and the `prefix` if a hexadecimal
        /// representation if the 64-bit unsigned integer with tagged shard prefix.
        /// For example: "0:3800000000000000".
        /// </summary>
        [JsonProperty("shard_filter", NullValueHandling = NullValueHandling.Ignore)]
        public string[] ShardFilter { get; set; }

        /// <summary>
        /// List of the fields that must be returned for iterated items.
        /// This field is the same as the `result` parameter of
        /// the `query_collection` function.
        /// Note that iterated items can contains additional fields that are
        /// not requested in the `result`.
        /// </summary>
        [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
        public string Result { get; set; }
    }

    public class RegisteredIterator
    {
        /// <summary>
        /// Must be removed using `remove_iterator`
        /// when it is no more needed for the application.
        /// </summary>
        [JsonProperty("handle", NullValueHandling = NullValueHandling.Ignore)]
        public uint Handle { get; set; }
    }

    public class ParamsOfResumeBlockIterator
    {
        /// <summary>
        /// Same as value returned from `iterator_next`.
        /// </summary>
        [JsonProperty("resume_state", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken ResumeState { get; set; }
    }

    public class ParamsOfCreateTransactionIterator
    {
        /// <summary>
        /// If the application specifies this parameter then the iteration
        /// includes blocks with `gen_utime` >= `start_time`.
        /// Otherwise the iteration starts from zero state.
        /// 
        /// Must be specified in seconds.
        /// </summary>
        [JsonProperty("start_time", NullValueHandling = NullValueHandling.Ignore)]
        public uint? StartTime { get; set; }

        /// <summary>
        /// If the application specifies this parameter then the iteration
        /// includes blocks with `gen_utime` < `end_time`.
        /// Otherwise the iteration never stops.
        /// 
        /// Must be specified in seconds.
        /// </summary>
        [JsonProperty("end_time", NullValueHandling = NullValueHandling.Ignore)]
        public uint? EndTime { get; set; }

        /// <summary>
        /// If the application specifies this parameter and it is not an empty array
        /// then the iteration will include items related to accounts that belongs to
        /// the specified shard prefixes.
        /// Shard prefix must be represented as a string "workchain:prefix".
        /// Where `workchain` is a signed integer and the `prefix` if a hexadecimal
        /// representation if the 64-bit unsigned integer with tagged shard prefix.
        /// For example: "0:3800000000000000".
        /// Account address conforms to the shard filter if
        /// it belongs to the filter workchain and the first bits of address match to
        /// the shard prefix. Only transactions with suitable account addresses are iterated.
        /// </summary>
        [JsonProperty("shard_filter", NullValueHandling = NullValueHandling.Ignore)]
        public string[] ShardFilter { get; set; }

        /// <summary>
        /// Application can specify the list of accounts for which
        /// it wants to iterate transactions.
        /// 
        /// If this parameter is missing or an empty list then the library iterates
        /// transactions for all accounts that pass the shard filter.
        /// 
        /// Note that the library doesn't detect conflicts between the account filter and the shard filter
        /// if both are specified.
        /// So it is an application responsibility to specify the correct filter combination.
        /// </summary>
        [JsonProperty("accounts_filter", NullValueHandling = NullValueHandling.Ignore)]
        public string[] AccountsFilter { get; set; }

        /// <summary>
        /// List of the fields that must be returned for iterated items.
        /// This field is the same as the `result` parameter of
        /// the `query_collection` function.
        /// Note that iterated items can contain additional fields that are
        /// not requested in the `result`.
        /// </summary>
        [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
        public string Result { get; set; }

        /// <summary>
        /// If this parameter is `true` then each transaction contains field
        /// `transfers` with list of transfer. See more about this structure in function description.
        /// </summary>
        [JsonProperty("include_transfers", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IncludeTransfers { get; set; }
    }

    public class ParamsOfResumeTransactionIterator
    {
        /// <summary>
        /// Same as value returned from `iterator_next`.
        /// </summary>
        [JsonProperty("resume_state", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken ResumeState { get; set; }

        /// <summary>
        /// Application can specify the list of accounts for which
        /// it wants to iterate transactions.
        /// 
        /// If this parameter is missing or an empty list then the library iterates
        /// transactions for all accounts that passes the shard filter.
        /// 
        /// Note that the library doesn't detect conflicts between the account filter and the shard filter
        /// if both are specified.
        /// So it is the application's responsibility to specify the correct filter combination.
        /// </summary>
        [JsonProperty("accounts_filter", NullValueHandling = NullValueHandling.Ignore)]
        public string[] AccountsFilter { get; set; }
    }

    public class ParamsOfIteratorNext
    {
        /// <summary>
        /// Iterator handle
        /// </summary>
        [JsonProperty("iterator", NullValueHandling = NullValueHandling.Ignore)]
        public uint Iterator { get; set; }

        /// <summary>
        /// If value is missing or is less than 1 the library uses 1.
        /// </summary>
        [JsonProperty("limit", NullValueHandling = NullValueHandling.Ignore)]
        public uint? Limit { get; set; }

        /// <summary>
        /// Indicates that function must return the iterator state that can be used for resuming iteration.
        /// </summary>
        [JsonProperty("return_resume_state", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ReturnResumeState { get; set; }
    }

    public class ResultOfIteratorNext
    {
        /// <summary>
        /// Note that `iterator_next` can return an empty items and `has_more` equals to `true`.
        /// In this case the application have to continue iteration.
        /// Such situation can take place when there is no data yet but
        /// the requested `end_time` is not reached.
        /// </summary>
        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken[] Items { get; set; }

        /// <summary>
        /// Indicates that there are more available items in iterated range.
        /// </summary>
        [JsonProperty("has_more", NullValueHandling = NullValueHandling.Ignore)]
        public bool HasMore { get; set; }

        /// <summary>
        /// This field is returned only if the `return_resume_state` parameter
        /// is specified.
        /// 
        /// Note that `resume_state` corresponds to the iteration position
        /// after the returned items.
        /// </summary>
        [JsonProperty("resume_state", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken ResumeState { get; set; }
    }

    /// <summary>
    /// Network access.
    /// </summary>
    public interface INetModule
    {
        /// <summary>
        /// Performs DAppServer GraphQL query.
        /// </summary>
        Task<ResultOfQuery> QueryAsync(ParamsOfQuery @params);

        /// <summary>
        /// Performs multiple queries per single fetch.
        /// </summary>
        Task<ResultOfBatchQuery> BatchQueryAsync(ParamsOfBatchQuery @params);

        /// <summary>
        /// Queries data that satisfies the `filter` conditions,
        /// limits the number of returned records and orders them.
        /// The projection fields are limited to `result` fields
        /// </summary>
        Task<ResultOfQueryCollection> QueryCollectionAsync(ParamsOfQueryCollection @params);

        /// <summary>
        /// Aggregates values from the specified `fields` for records
        /// that satisfies the `filter` conditions,
        /// </summary>
        Task<ResultOfAggregateCollection> AggregateCollectionAsync(ParamsOfAggregateCollection @params);

        /// <summary>
        /// Triggers only once.
        /// If object that satisfies the `filter` conditions
        /// already exists - returns it immediately.
        /// If not - waits for insert/update of data within the specified `timeout`,
        /// and returns it.
        /// The projection fields are limited to `result` fields
        /// </summary>
        Task<ResultOfWaitForCollection> WaitForCollectionAsync(ParamsOfWaitForCollection @params);

        /// <summary>
        /// Cancels a subscription specified by its handle.
        /// </summary>
        Task UnsubscribeAsync(ResultOfSubscribeCollection @params);

        /// <summary>
        /// Triggers for each insert/update of data that satisfies
        /// the `filter` conditions.
        /// The projection fields are limited to `result` fields.
        /// 
        /// The subscription is a persistent communication channel between
        /// client and Free TON Network.
        /// All changes in the blockchain will be reflected in realtime.
        /// Changes means inserts and updates of the blockchain entities.
        /// 
        /// ### Important Notes on Subscriptions
        /// 
        /// Unfortunately sometimes the connection with the network brakes down.
        /// In this situation the library attempts to reconnect to the network.
        /// This reconnection sequence can take significant time.
        /// All of this time the client is disconnected from the network.
        /// 
        /// Bad news is that all blockchain changes that happened while
        /// the client was disconnected are lost.
        /// 
        /// Good news is that the client report errors to the callback when
        /// it loses and resumes connection.
        /// 
        /// So, if the lost changes are important to the application then
        /// the application must handle these error reports.
        /// 
        /// Library reports errors with `responseType` == 101
        /// and the error object passed via `params`.
        /// 
        /// When the library has successfully reconnected
        /// the application receives callback with
        /// `responseType` == 101 and `params.code` == 614 (NetworkModuleResumed).
        /// 
        /// Application can use several ways to handle this situation:
        /// - If application monitors changes for the single blockchain
        /// object (for example specific account):  application
        /// can perform a query for this object and handle actual data as a
        /// regular data from the subscription.
        /// - If application monitors sequence of some blockchain objects
        /// (for example transactions of the specific account): application must
        /// refresh all cached (or visible to user) lists where this sequences presents.
        /// </summary>
        Task<ResultOfSubscribeCollection> SubscribeCollectionAsync(ParamsOfSubscribeCollection @params, Func<Newtonsoft.Json.Linq.JToken, int, Task> callback = null);

        /// <summary>
        /// Suspends network module to stop any network activity
        /// </summary>
        Task SuspendAsync();

        /// <summary>
        /// Resumes network module to enable network activity
        /// </summary>
        Task ResumeAsync();

        /// <summary>
        /// Returns ID of the last block in a specified account shard
        /// </summary>
        Task<ResultOfFindLastShardBlock> FindLastShardBlockAsync(ParamsOfFindLastShardBlock @params);

        /// <summary>
        /// Requests the list of alternative endpoints from server
        /// </summary>
        Task<EndpointsSet> FetchEndpointsAsync();

        /// <summary>
        /// Sets the list of endpoints to use on reinit
        /// </summary>
        Task SetEndpointsAsync(EndpointsSet @params);

        /// <summary>
        /// Requests the list of alternative endpoints from server
        /// </summary>
        Task<ResultOfGetEndpoints> GetEndpointsAsync();

        /// <summary>
        /// *Attention* this query retrieves data from 'Counterparties' service which is not supported in
        /// the opensource version of DApp Server (and will not be supported) as well as in TON OS SE (will be
        /// supported in SE in future),
        /// but is always accessible via [TON OS Devnet/Mainnet
        /// Clouds](https://docs.ton.dev/86757ecb2/p/85c869-networks)
        /// </summary>
        Task<ResultOfQueryCollection> QueryCounterpartiesAsync(ParamsOfQueryCounterparties @params);

        /// <summary>
        /// Performs recursive retrieval of the transactions tree produced by the specific message:
        /// in_msg -> dst_transaction -> out_messages -> dst_transaction -> ...
        /// 
        /// All retrieved messages and transactions will be included
        /// into `result.messages` and `result.transactions` respectively.
        /// 
        /// The retrieval process will stop when the retrieved transaction count is more than 50.
        /// 
        /// It is guaranteed that each message in `result.messages` has the corresponding transaction
        /// in the `result.transactions`.
        /// 
        /// But there are no guaranties that all messages from transactions `out_msgs` are
        /// presented in `result.messages`.
        /// So the application have to continue retrieval for missing messages if it requires.
        /// </summary>
        Task<ResultOfQueryTransactionTree> QueryTransactionTreeAsync(ParamsOfQueryTransactionTree @params);

        /// <summary>
        /// Block iterator uses robust iteration methods that guaranties that every
        /// block in the specified range isn't missed or iterated twice.
        /// 
        /// Iterated range can be reduced with some filters:
        /// - `start_time` – the bottom time range. Only blocks with `gen_utime`
        /// more or equal to this value is iterated. If this parameter is omitted then there is
        /// no bottom time edge, so all blocks since zero state is iterated.
        /// - `end_time` – the upper time range. Only blocks with `gen_utime`
        /// less then this value is iterated. If this parameter is omitted then there is
        /// no upper time edge, so iterator never finishes.
        /// - `shard_filter` – workchains and shard prefixes that reduce the set of interesting
        /// blocks. Block conforms to the shard filter if it belongs to the filter workchain
        /// and the first bits of block's `shard` fields matches to the shard prefix.
        /// Only blocks with suitable shard are iterated.
        /// 
        /// Items iterated is a JSON objects with block data. The minimal set of returned
        /// fields is:
        /// ```text
        /// id
        /// gen_utime
        /// workchain_id
        /// shard
        /// after_split
        /// after_merge
        /// prev_ref {
        ///     root_hash
        /// }
        /// prev_alt_ref {
        ///     root_hash
        /// }
        /// ```
        /// Application can request additional fields in the `result` parameter.
        /// 
        /// Application should call the `remove_iterator` when iterator is no longer required.
        /// </summary>
        Task<RegisteredIterator> CreateBlockIteratorAsync(ParamsOfCreateBlockIterator @params);

        /// <summary>
        /// The iterator stays exactly at the same position where the `resume_state` was catched.
        /// 
        /// Application should call the `remove_iterator` when iterator is no longer required.
        /// </summary>
        Task<RegisteredIterator> ResumeBlockIteratorAsync(ParamsOfResumeBlockIterator @params);

        /// <summary>
        /// Transaction iterator uses robust iteration methods that guaranty that every
        /// transaction in the specified range isn't missed or iterated twice.
        /// 
        /// Iterated range can be reduced with some filters:
        /// - `start_time` – the bottom time range. Only transactions with `now`
        /// more or equal to this value are iterated. If this parameter is omitted then there is
        /// no bottom time edge, so all the transactions since zero state are iterated.
        /// - `end_time` – the upper time range. Only transactions with `now`
        /// less then this value are iterated. If this parameter is omitted then there is
        /// no upper time edge, so iterator never finishes.
        /// - `shard_filter` – workchains and shard prefixes that reduce the set of interesting
        /// accounts. Account address conforms to the shard filter if
        /// it belongs to the filter workchain and the first bits of address match to
        /// the shard prefix. Only transactions with suitable account addresses are iterated.
        /// - `accounts_filter` – set of account addresses whose transactions must be iterated.
        /// Note that accounts filter can conflict with shard filter so application must combine
        /// these filters carefully.
        /// 
        /// Iterated item is a JSON objects with transaction data. The minimal set of returned
        /// fields is:
        /// ```text
        /// id
        /// account_addr
        /// now
        /// balance_delta(format:DEC)
        /// bounce { bounce_type }
        /// in_message {
        ///     id
        ///     value(format:DEC)
        ///     msg_type
        ///     src
        /// }
        /// out_messages {
        ///     id
        ///     value(format:DEC)
        ///     msg_type
        ///     dst
        /// }
        /// ```
        /// Application can request an additional fields in the `result` parameter.
        /// 
        /// Another parameter that affects on the returned fields is the `include_transfers`.
        /// When this parameter is `true` the iterator computes and adds `transfer` field containing
        /// list of the useful `TransactionTransfer` objects.
        /// Each transfer is calculated from the particular message related to the transaction
        /// and has the following structure:
        /// - message – source message identifier.
        /// - isBounced – indicates that the transaction is bounced, which means the value will be returned back
        /// to the sender.
        /// - isDeposit – indicates that this transfer is the deposit (true) or withdraw (false).
        /// - counterparty – account address of the transfer source or destination depending on `isDeposit`.
        /// - value – amount of nano tokens transferred. The value is represented as a decimal string
        /// because the actual value can be more precise than the JSON number can represent. Application
        /// must use this string carefully – conversion to number can follow to loose of precision.
        /// 
        /// Application should call the `remove_iterator` when iterator is no longer required.
        /// </summary>
        Task<RegisteredIterator> CreateTransactionIteratorAsync(ParamsOfCreateTransactionIterator @params);

        /// <summary>
        /// The iterator stays exactly at the same position where the `resume_state` was caught.
        /// Note that `resume_state` doesn't store the account filter. If the application requires
        /// to use the same account filter as it was when the iterator was created then the application
        /// must pass the account filter again in `accounts_filter` parameter.
        /// 
        /// Application should call the `remove_iterator` when iterator is no longer required.
        /// </summary>
        Task<RegisteredIterator> ResumeTransactionIteratorAsync(ParamsOfResumeTransactionIterator @params);

        /// <summary>
        /// In addition to available items this function returns the `has_more` flag
        /// indicating that the iterator isn't reach the end of the iterated range yet.
        /// 
        /// This function can return the empty list of available items but
        /// indicates that there are more items is available.
        /// This situation appears when the iterator doesn't reach iterated range
        /// but database doesn't contains available items yet.
        /// 
        /// If application requests resume state in `return_resume_state` parameter
        /// then this function returns `resume_state` that can be used later to
        /// resume the iteration from the position after returned items.
        /// 
        /// The structure of the items returned depends on the iterator used.
        /// See the description to the appropriated iterator creation function.
        /// </summary>
        Task<ResultOfIteratorNext> IteratorNextAsync(ParamsOfIteratorNext @params);

        /// <summary>
        /// Frees all resources allocated in library to serve iterator.
        /// 
        /// Application always should call the `remove_iterator` when iterator
        /// is no longer required.
        /// </summary>
        Task RemoveIteratorAsync(RegisteredIterator @params);
    }

    internal class NetModule : INetModule
    {
        private readonly TonClient _client;

        internal NetModule(TonClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<ResultOfQuery> QueryAsync(ParamsOfQuery @params)
        {
            return await _client.CallFunctionAsync<ResultOfQuery>("net.query", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfBatchQuery> BatchQueryAsync(ParamsOfBatchQuery @params)
        {
            return await _client.CallFunctionAsync<ResultOfBatchQuery>("net.batch_query", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfQueryCollection> QueryCollectionAsync(ParamsOfQueryCollection @params)
        {
            return await _client.CallFunctionAsync<ResultOfQueryCollection>("net.query_collection", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfAggregateCollection> AggregateCollectionAsync(ParamsOfAggregateCollection @params)
        {
            return await _client.CallFunctionAsync<ResultOfAggregateCollection>("net.aggregate_collection", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfWaitForCollection> WaitForCollectionAsync(ParamsOfWaitForCollection @params)
        {
            return await _client.CallFunctionAsync<ResultOfWaitForCollection>("net.wait_for_collection", @params).ConfigureAwait(false);
        }

        public async Task UnsubscribeAsync(ResultOfSubscribeCollection @params)
        {
            await _client.CallFunctionAsync("net.unsubscribe", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfSubscribeCollection> SubscribeCollectionAsync(ParamsOfSubscribeCollection @params, Func<Newtonsoft.Json.Linq.JToken, int, Task> callback = null)
        {
            return await _client.CallFunctionAsync<ResultOfSubscribeCollection, Newtonsoft.Json.Linq.JToken>("net.subscribe_collection", @params, callback).ConfigureAwait(false);
        }

        public async Task SuspendAsync()
        {
            await _client.CallFunctionAsync("net.suspend").ConfigureAwait(false);
        }

        public async Task ResumeAsync()
        {
            await _client.CallFunctionAsync("net.resume").ConfigureAwait(false);
        }

        public async Task<ResultOfFindLastShardBlock> FindLastShardBlockAsync(ParamsOfFindLastShardBlock @params)
        {
            return await _client.CallFunctionAsync<ResultOfFindLastShardBlock>("net.find_last_shard_block", @params).ConfigureAwait(false);
        }

        public async Task<EndpointsSet> FetchEndpointsAsync()
        {
            return await _client.CallFunctionAsync<EndpointsSet>("net.fetch_endpoints").ConfigureAwait(false);
        }

        public async Task SetEndpointsAsync(EndpointsSet @params)
        {
            await _client.CallFunctionAsync("net.set_endpoints", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfGetEndpoints> GetEndpointsAsync()
        {
            return await _client.CallFunctionAsync<ResultOfGetEndpoints>("net.get_endpoints").ConfigureAwait(false);
        }

        public async Task<ResultOfQueryCollection> QueryCounterpartiesAsync(ParamsOfQueryCounterparties @params)
        {
            return await _client.CallFunctionAsync<ResultOfQueryCollection>("net.query_counterparties", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfQueryTransactionTree> QueryTransactionTreeAsync(ParamsOfQueryTransactionTree @params)
        {
            return await _client.CallFunctionAsync<ResultOfQueryTransactionTree>("net.query_transaction_tree", @params).ConfigureAwait(false);
        }

        public async Task<RegisteredIterator> CreateBlockIteratorAsync(ParamsOfCreateBlockIterator @params)
        {
            return await _client.CallFunctionAsync<RegisteredIterator>("net.create_block_iterator", @params).ConfigureAwait(false);
        }

        public async Task<RegisteredIterator> ResumeBlockIteratorAsync(ParamsOfResumeBlockIterator @params)
        {
            return await _client.CallFunctionAsync<RegisteredIterator>("net.resume_block_iterator", @params).ConfigureAwait(false);
        }

        public async Task<RegisteredIterator> CreateTransactionIteratorAsync(ParamsOfCreateTransactionIterator @params)
        {
            return await _client.CallFunctionAsync<RegisteredIterator>("net.create_transaction_iterator", @params).ConfigureAwait(false);
        }

        public async Task<RegisteredIterator> ResumeTransactionIteratorAsync(ParamsOfResumeTransactionIterator @params)
        {
            return await _client.CallFunctionAsync<RegisteredIterator>("net.resume_transaction_iterator", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfIteratorNext> IteratorNextAsync(ParamsOfIteratorNext @params)
        {
            return await _client.CallFunctionAsync<ResultOfIteratorNext>("net.iterator_next", @params).ConfigureAwait(false);
        }

        public async Task RemoveIteratorAsync(RegisteredIterator @params)
        {
            await _client.CallFunctionAsync("net.remove_iterator", @params).ConfigureAwait(false);
        }
    }
}

namespace TonSdk
{
    public partial interface ITonClient
    {
        INetModule Net { get; }
    }

    public partial class TonClient
    {
        private NetModule _netModule;

        public INetModule Net
        {
            get
            {
                return _netModule ?? (_netModule = new NetModule(this));
            }
        }
    }
}

