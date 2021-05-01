using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Numerics;
using System.Threading.Tasks;
using TonSdk.Modules;

/*
* TON API version 1.13.0, net module.
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
        /// *Attention* this query retrieves data from 'Counterparties' service which is not supported in
        /// the opensource version of DApp Server (and will not be supported) as well as in TON OS SE (will be
        /// supported in SE in future),
        /// but is always accessible via [TON OS Devnet/Mainnet
        /// Clouds](https://docs.ton.dev/86757ecb2/p/85c869-networks)
        /// </summary>
        Task<ResultOfQueryCollection> QueryCounterpartiesAsync(ParamsOfQueryCounterparties @params);
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

        public async Task<ResultOfQueryCollection> QueryCounterpartiesAsync(ParamsOfQueryCounterparties @params)
        {
            return await _client.CallFunctionAsync<ResultOfQueryCollection>("net.query_counterparties", @params).ConfigureAwait(false);
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

