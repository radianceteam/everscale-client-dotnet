using Newtonsoft.Json;
using System;
using System.Numerics;
using System.Threading.Tasks;
using TonSdk.Modules;

/*
* TON API version 1.0.0, processing module.
* THIS FILE WAS GENERATED AUTOMATICALLY.
*/

namespace TonSdk.Modules
{
    public abstract class ProcessingEvent
    {
        /// <summary>
        ///  Notifies the app that the current shard block will be fetched
        ///  from the network.
        /// 
        ///  Fetched block will be used later in waiting phase.
        /// </summary>
        public class WillFetchFirstBlock : ProcessingEvent
        {
        }

        /// <summary>
        ///  Notifies the app that the client has failed to fetch current
        ///  shard block.
        /// 
        ///  Message processing has finished.
        /// </summary>
        public class FetchFirstBlockFailed : ProcessingEvent
        {
            [JsonProperty("error")]
            public ClientError Error { get; set; }
        }

        /// <summary>
        ///  Notifies the app that the message will be sent to the
        ///  network.
        /// </summary>
        public class WillSend : ProcessingEvent
        {
            [JsonProperty("shard_block_id")]
            public string ShardBlockId { get; set; }

            [JsonProperty("message_id")]
            public string MessageId { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }
        }

        /// <summary>
        ///  Notifies the app that the message was sent to the network.
        /// </summary>
        public class DidSend : ProcessingEvent
        {
            [JsonProperty("shard_block_id")]
            public string ShardBlockId { get; set; }

            [JsonProperty("message_id")]
            public string MessageId { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }
        }

        /// <summary>
        ///  Notifies the app that the sending operation was failed with
        ///  network error.
        /// 
        ///  Nevertheless the processing will be continued at the waiting
        ///  phase because the message possibly has been delivered to the
        ///  node.
        /// </summary>
        public class SendFailed : ProcessingEvent
        {
            [JsonProperty("shard_block_id")]
            public string ShardBlockId { get; set; }

            [JsonProperty("message_id")]
            public string MessageId { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("error")]
            public ClientError Error { get; set; }
        }

        /// <summary>
        ///  Notifies the app that the next shard block will be fetched
        ///  from the network.
        /// 
        ///  Event can occurs more than one time due to block walking
        ///  procedure.
        /// </summary>
        public class WillFetchNextBlock : ProcessingEvent
        {
            [JsonProperty("shard_block_id")]
            public string ShardBlockId { get; set; }

            [JsonProperty("message_id")]
            public string MessageId { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }
        }

        /// <summary>
        ///  Notifies the app that the next block can't be fetched due to
        ///  error.
        /// 
        ///  Processing will be continued after `network_resume_timeout`.
        /// </summary>
        public class FetchNextBlockFailed : ProcessingEvent
        {
            [JsonProperty("shard_block_id")]
            public string ShardBlockId { get; set; }

            [JsonProperty("message_id")]
            public string MessageId { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("error")]
            public ClientError Error { get; set; }
        }

        /// <summary>
        ///  Notifies the app that the message was expired.
        /// 
        ///  Event occurs for contracts which ABI includes header "expire"
        /// 
        ///  Processing will be continued from encoding phase after
        ///  `expiration_retries_timeout`.
        /// </summary>
        public class MessageExpired : ProcessingEvent
        {
            [JsonProperty("message_id")]
            public string MessageId { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("error")]
            public ClientError Error { get; set; }
        }
    }

    public class ResultOfProcessMessage
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
        ///  Transaction fees
        /// </summary>
        [JsonProperty("fees")]
        public Newtonsoft.Json.Linq.JToken Fees { get; set; }
    }

    public class DecodedOutput
    {
        /// <summary>
        ///  Decoded bodies of the out messages.
        /// 
        ///  If the message can't be decoded then `None` will be stored in
        ///  the appropriate position.
        /// </summary>
        [JsonProperty("out_messages")]
        public DecodedMessageBody[] OutMessages { get; set; }

        /// <summary>
        ///  Decoded body of the function output message.
        /// </summary>
        [JsonProperty("output")]
        public Newtonsoft.Json.Linq.JToken Output { get; set; }
    }

    public class ParamsOfSendMessage
    {
        /// <summary>
        ///  Message BOC.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        ///  Optional message ABI.
        /// 
        ///  If this parameter is specified and the message has the
        ///  `expire` header then expiration time will be checked against
        ///  the current time to prevent an unnecessary sending of already expired message.
        /// 
        ///  The `message already expired` error will be returned in this
        ///  case.
        /// 
        ///  Note that specifying `abi` for ABI compliant contracts is
        ///  strongly recommended due to choosing proper processing
        ///  strategy.
        /// </summary>
        [JsonProperty("abi")]
        public Abi Abi { get; set; }

        /// <summary>
        ///  Flag for requesting events sending
        /// </summary>
        [JsonProperty("send_events")]
        public bool SendEvents { get; set; }
    }

    public class ResultOfSendMessage
    {
        /// <summary>
        ///  The last generated shard block of the message destination account before the
        ///  message was sent.
        /// 
        ///  This block id must be used as a parameter of the
        ///  `wait_for_transaction`.
        /// </summary>
        [JsonProperty("shard_block_id")]
        public string ShardBlockId { get; set; }
    }

    public class ParamsOfWaitForTransaction
    {
        /// <summary>
        ///  Optional ABI for decoding the transaction result.
        /// 
        ///  If it is specified then the output messages' bodies will be
        ///  decoded according to this ABI.
        /// 
        ///  The `abi_decoded` result field will be filled out.
        /// </summary>
        [JsonProperty("abi")]
        public Abi Abi { get; set; }

        /// <summary>
        ///  Message BOC. Encoded with `base64`.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        ///  The last generated block id of the destination account shard before the message was sent.
        /// 
        ///  You must provide the same value as the `send_message` has returned.
        /// </summary>
        [JsonProperty("shard_block_id")]
        public string ShardBlockId { get; set; }

        /// <summary>
        ///  Flag that enables/disables intermediate events
        /// </summary>
        [JsonProperty("send_events")]
        public bool SendEvents { get; set; }
    }

    public class ParamsOfProcessMessage
    {
        /// <summary>
        ///  Message encode parameters.
        /// </summary>
        [JsonProperty("message_encode_params")]
        public ParamsOfEncodeMessage MessageEncodeParams { get; set; }

        /// <summary>
        ///  Flag for requesting events sending
        /// </summary>
        [JsonProperty("send_events")]
        public bool SendEvents { get; set; }
    }

    /// <summary>
    ///  Message processing module.
    /// 
    ///  This module incorporates functions related to complex message
    ///  processing scenarios.
    /// </summary>
    public interface IProcessingModule
    {
        /// <summary>
        ///  Sends message to the network
        ///  
        ///  Sends message to the network and returns the last generated shard block of the destination account
        ///  before the message was sent. It will be required later for message processing.
        /// </summary>
        Task<ResultOfSendMessage> SendMessageAsync(ParamsOfSendMessage @params);

        /// <summary>
        ///  Performs monitoring of the network for the result transaction
        ///  of the external inbound message processing.
        ///  
        ///  `send_events` enables intermediate events, such as `WillFetchNextBlock`,
        ///  `FetchNextBlockFailed` that may be useful for logging of new shard blocks creation 
        ///  during message processing.
        /// 
        ///  Note that presence of the `abi` parameter is critical for ABI
        ///  compliant contracts. Message processing uses drastically
        ///  different strategy for processing message for contracts which
        ///  ABI includes "expire" header.
        /// 
        ///  When the ABI header `expire` is present, the processing uses
        ///  `message expiration` strategy:
        ///  - The maximum block gen time is set to
        ///    `message_expiration_time + transaction_wait_timeout`.
        ///  - When maximum block gen time is reached the processing will
        ///    be finished with `MessageExpired` error.
        /// 
        ///  When the ABI header `expire` isn't present or `abi` parameter
        ///  isn't specified, the processing uses `transaction waiting`
        ///  strategy:
        ///  - The maximum block gen time is set to
        ///    `now() + transaction_wait_timeout`.
        ///  
        ///  - If maximum block gen time is reached and no result transaction is found 
        ///  the processing will exit with an error.
        /// </summary>
        Task<ResultOfProcessMessage> WaitForTransactionAsync(ParamsOfWaitForTransaction @params);

        /// <summary>
        ///  Creates message, sends it to the network and monitors its processing.
        ///  
        ///  Creates ABI-compatible message,
        ///  sends it to the network and monitors for the result transaction.
        ///  Decodes the output messages's bodies.
        ///  
        ///  If contract's ABI includes "expire" header then
        ///  SDK implements retries in case of unsuccessful message delivery within the expiration
        ///  timeout: SDK recreates the message, sends it and processes it again. 
        ///  
        ///  The intermediate events, such as `WillFetchFirstBlock`, `WillSend`, `DidSend`,
        ///  `WillFetchNextBlock`, etc - are switched on/off by `send_events` flag 
        ///  and logged into the supplied callback function.
        ///  The retry configuration parameters are defined in config:
        ///  <add correct config params here>
        ///  pub const DEFAULT_EXPIRATION_RETRIES_LIMIT: i8 = 3; - max number of retries
        ///  pub const DEFAULT_EXPIRATION_TIMEOUT: u32 = 40000;  - message expiration timeout in ms.
        /// pub const DEFAULT_....expiration_timeout_grow_factor... = 1.5 - factor that increases the
        /// expiration timeout for each retry
        ///  
        ///  If contract's ABI does not include "expire" header
        /// then if no transaction is found within the network timeout (see config parameter ), exits with
        /// error.
        /// </summary>
        Task<ResultOfProcessMessage> ProcessMessageAsync(ParamsOfProcessMessage @params);
    }

    internal class ProcessingModule : IProcessingModule
    {
        private readonly TonClient _client;

        internal ProcessingModule(TonClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<ResultOfSendMessage> SendMessageAsync(ParamsOfSendMessage @params)
        {
            return await _client.CallFunctionAsync<ResultOfSendMessage>("processing.send_message").ConfigureAwait(false);
        }

        public async Task<ResultOfProcessMessage> WaitForTransactionAsync(ParamsOfWaitForTransaction @params)
        {
            return await _client.CallFunctionAsync<ResultOfProcessMessage>("processing.wait_for_transaction").ConfigureAwait(false);
        }

        public async Task<ResultOfProcessMessage> ProcessMessageAsync(ParamsOfProcessMessage @params)
        {
            return await _client.CallFunctionAsync<ResultOfProcessMessage>("processing.process_message").ConfigureAwait(false);
        }
    }
}

namespace TonSdk
{
    public partial interface ITonClient
    {
        IProcessingModule Processing { get; }
    }

    public partial class TonClient
    {
        private ProcessingModule _processingModule;

        public IProcessingModule Processing
        {
            get
            {
                return _processingModule ?? (_processingModule = new ProcessingModule(this));
            }
        }
    }
}

