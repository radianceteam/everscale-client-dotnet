using Newtonsoft.Json;
using System;
using System.Numerics;
using System.Threading.Tasks;
using TonSdk.Modules;

/*
* TON API version 1.0.0, net module.
* THIS FILE WAS GENERATED AUTOMATICALLY.
*/

namespace TonSdk.Modules
{
    public class OrderBy
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("direction")]
        public SortDirection Direction { get; set; }
    }

    public enum SortDirection
    {
        ASC,
        DESC,
    }

    public class ParamsOfQueryCollection
    {
        /// <summary>
        ///  Collection name (accounts, blocks, transactions, messages, block_signatures)
        /// </summary>
        [JsonProperty("collection")]
        public string Collection { get; set; }

        /// <summary>
        ///  Collection filter
        /// </summary>
        [JsonProperty("filter")]
        public Newtonsoft.Json.Linq.JRaw Filter { get; set; }

        /// <summary>
        ///  Projection (result) string
        /// </summary>
        [JsonProperty("result")]
        public string Result { get; set; }

        /// <summary>
        ///  Sorting order
        /// </summary>
        [JsonProperty("order")]
        public OrderBy[] Order { get; set; }

        /// <summary>
        ///  Number of documents to return
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }
    }

    public class ResultOfQueryCollection
    {
        /// <summary>
        ///  Objects that match the provided criteria
        /// </summary>
        [JsonProperty("result")]
        public Newtonsoft.Json.Linq.JRaw[] Result { get; set; }
    }

    public class ParamsOfWaitForCollection
    {
        /// <summary>
        ///  Collection name (accounts, blocks, transactions, messages, block_signatures)
        /// </summary>
        [JsonProperty("collection")]
        public string Collection { get; set; }

        /// <summary>
        ///  Collection filter
        /// </summary>
        [JsonProperty("filter")]
        public Newtonsoft.Json.Linq.JRaw Filter { get; set; }

        /// <summary>
        ///  Projection (result) string
        /// </summary>
        [JsonProperty("result")]
        public string Result { get; set; }

        /// <summary>
        ///  Query timeout
        /// </summary>
        [JsonProperty("timeout")]
        public int? Timeout { get; set; }
    }

    public class ResultOfWaitForCollection
    {
        /// <summary>
        ///  First found object that matches the provided criteria
        /// </summary>
        [JsonProperty("result")]
        public Newtonsoft.Json.Linq.JRaw Result { get; set; }
    }

    public class ResultOfSubscribeCollection
    {
        /// <summary>
        ///  Subscription handle. Must be closed with `unsubscribe`
        /// </summary>
        [JsonProperty("handle")]
        public int Handle { get; set; }
    }

    public class ParamsOfSubscribeCollection
    {
        /// <summary>
        ///  Collection name (accounts, blocks, transactions, messages, block_signatures)
        /// </summary>
        [JsonProperty("collection")]
        public string Collection { get; set; }

        /// <summary>
        ///  Collection filter
        /// </summary>
        [JsonProperty("filter")]
        public Newtonsoft.Json.Linq.JRaw Filter { get; set; }

        /// <summary>
        ///  Projection (result) string
        /// </summary>
        [JsonProperty("result")]
        public string Result { get; set; }
    }

    /// <summary>
    ///  Network access.
    /// </summary>
    public interface INetModule
    {
        /// <summary>
        ///  Queries collection data
        ///  
        ///  Queries data that satisfies the `filter` conditions, 
        ///  limits the number of returned records and orders them.
        ///  The projection fields are limited to  `result` fields
        /// </summary>
        Task<ResultOfQueryCollection> QueryCollectionAsync(ParamsOfQueryCollection @params);

        /// <summary>
        ///  Returns an object that fulfills the conditions or waits for its appearance
        ///  
        ///  Triggers only once. 
        ///  If object that satisfies the `filter` conditions 
        ///  already exists - returns it immediately. 
        ///  If not - waits for insert/update of data withing the specified `timeout`,
        ///  and returns it. 
        ///  The projection fields are limited to  `result` fields
        /// </summary>
        Task<ResultOfWaitForCollection> WaitForCollectionAsync(ParamsOfWaitForCollection @params);

        /// <summary>
        ///  Cancels a subscription
        ///  
        ///  Cancels a subscription specified by its handle.
        /// </summary>
        Task UnsubscribeAsync(ResultOfSubscribeCollection @params);

        /// <summary>
        ///  Creates a subscription
        ///  
        ///  Triggers for each insert/update of data
        ///  that satisfies the `filter` conditions.
        ///  The projection fields are limited to  `result` fields.
        /// </summary>
        Task<ResultOfSubscribeCollection> SubscribeCollectionAsync(ParamsOfSubscribeCollection @params);
    }

    internal class NetModule : INetModule
    {
        private readonly TonClient _client;

        internal NetModule(TonClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<ResultOfQueryCollection> QueryCollectionAsync(ParamsOfQueryCollection @params)
        {
            return await _client.CallFunctionAsync<ResultOfQueryCollection>("net.query_collection", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfWaitForCollection> WaitForCollectionAsync(ParamsOfWaitForCollection @params)
        {
            return await _client.CallFunctionAsync<ResultOfWaitForCollection>("net.wait_for_collection", @params).ConfigureAwait(false);
        }

        public async Task UnsubscribeAsync(ResultOfSubscribeCollection @params)
        {
            await _client.CallFunctionAsync("net.unsubscribe", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfSubscribeCollection> SubscribeCollectionAsync(ParamsOfSubscribeCollection @params)
        {
            return await _client.CallFunctionAsync<ResultOfSubscribeCollection>("net.subscribe_collection").ConfigureAwait(false);
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

