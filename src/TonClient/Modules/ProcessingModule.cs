using Newtonsoft.Json;
using System;
using System.Numerics;
using System.Threading.Tasks;
using TonSdk.Modules;

/*
* TON API version 1.35.1, processing module.
* THIS FILE WAS GENERATED AUTOMATICALLY.
*/

namespace TonSdk.Modules
{
    public enum ProcessingErrorCode
    {
        MessageAlreadyExpired = 501,
        MessageHasNotDestinationAddress = 502,
        CanNotBuildMessageCell = 503,
        FetchBlockFailed = 504,
        SendMessageFailed = 505,
        InvalidMessageBoc = 506,
        MessageExpired = 507,
        TransactionWaitTimeout = 508,
        InvalidBlockReceived = 509,
        CanNotCheckBlockShard = 510,
        BlockNotFound = 511,
        InvalidData = 512,
        ExternalSignerMustNotBeUsed = 513,
        MessageRejected = 514,
        InvalidRempStatus = 515,
        NextRempStatusTimeout = 516,
    }

    public abstract class ProcessingEvent
    {
        /// <summary>
        /// Fetched block will be used later in waiting phase.
        /// </summary>
        public class WillFetchFirstBlock : ProcessingEvent
        {
        }

        /// <summary>
        /// This may happen due to the network issues. Receiving this event means that message processing will
        /// not proceed -
        /// message was not sent, and Developer can try to run `process_message` again,
        /// in the hope that the connection is restored.
        /// </summary>
        public class FetchFirstBlockFailed : ProcessingEvent
        {
            [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
            public ClientError Error { get; set; }
        }

        /// <summary>
        /// Notifies the app that the message will be sent to the network. This event means that the account's
        /// current shard block was successfully fetched and the message was successfully created
        /// (`abi.encode_message` function was executed successfully).
        /// </summary>
        public class WillSend : ProcessingEvent
        {
            [JsonProperty("shard_block_id", NullValueHandling = NullValueHandling.Ignore)]
            public string ShardBlockId { get; set; }

            [JsonProperty("message_id", NullValueHandling = NullValueHandling.Ignore)]
            public string MessageId { get; set; }

            [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
            public string Message { get; set; }
        }

        /// <summary>
        /// Do not forget to specify abi of your contract as well, it is crucial for proccessing. See
        /// `processing.wait_for_transaction` documentation.
        /// </summary>
        public class DidSend : ProcessingEvent
        {
            [JsonProperty("shard_block_id", NullValueHandling = NullValueHandling.Ignore)]
            public string ShardBlockId { get; set; }

            [JsonProperty("message_id", NullValueHandling = NullValueHandling.Ignore)]
            public string MessageId { get; set; }

            [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
            public string Message { get; set; }
        }

        /// <summary>
        /// Nevertheless the processing will be continued at the waiting
        /// phase because the message possibly has been delivered to the
        /// node.
        /// If Application exits at this phase, Developer needs to proceed with processing
        /// after the application is restored with `wait_for_transaction` function, passing
        /// shard_block_id and message from this event. Do not forget to specify abi of your contract
        /// as well, it is crucial for proccessing. See `processing.wait_for_transaction` documentation.
        /// </summary>
        public class SendFailed : ProcessingEvent
        {
            [JsonProperty("shard_block_id", NullValueHandling = NullValueHandling.Ignore)]
            public string ShardBlockId { get; set; }

            [JsonProperty("message_id", NullValueHandling = NullValueHandling.Ignore)]
            public string MessageId { get; set; }

            [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
            public string Message { get; set; }

            [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
            public ClientError Error { get; set; }
        }

        /// <summary>
        /// Event can occurs more than one time due to block walking
        /// procedure.
        /// If Application exits at this phase, Developer needs to proceed with processing
        /// after the application is restored with `wait_for_transaction` function, passing
        /// shard_block_id and message from this event. Do not forget to specify abi of your contract
        /// as well, it is crucial for proccessing. See `processing.wait_for_transaction` documentation.
        /// </summary>
        public class WillFetchNextBlock : ProcessingEvent
        {
            [JsonProperty("shard_block_id", NullValueHandling = NullValueHandling.Ignore)]
            public string ShardBlockId { get; set; }

            [JsonProperty("message_id", NullValueHandling = NullValueHandling.Ignore)]
            public string MessageId { get; set; }

            [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
            public string Message { get; set; }
        }

        /// <summary>
        /// If no block was fetched within `NetworkConfig.wait_for_timeout` then processing stops.
        /// This may happen when the shard stops, or there are other network issues.
        /// In this case Developer should resume message processing with `wait_for_transaction`, passing
        /// shard_block_id,
        /// message and contract abi to it. Note that passing ABI is crucial, because it will influence the
        /// processing strategy.
        /// 
        /// Another way to tune this is to specify long timeout in `NetworkConfig.wait_for_timeout`
        /// </summary>
        public class FetchNextBlockFailed : ProcessingEvent
        {
            [JsonProperty("shard_block_id", NullValueHandling = NullValueHandling.Ignore)]
            public string ShardBlockId { get; set; }

            [JsonProperty("message_id", NullValueHandling = NullValueHandling.Ignore)]
            public string MessageId { get; set; }

            [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
            public string Message { get; set; }

            [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
            public ClientError Error { get; set; }
        }

        /// <summary>
        /// This event occurs only for the contracts which ABI includes "expire" header.
        /// 
        /// If Application specifies `NetworkConfig.message_retries_count` > 0, then `process_message`
        /// will perform retries: will create a new message and send it again and repeat it untill it reaches
        /// the maximum retries count or receives a successful result.  All the processing
        /// events will be repeated.
        /// </summary>
        public class MessageExpired : ProcessingEvent
        {
            [JsonProperty("message_id", NullValueHandling = NullValueHandling.Ignore)]
            public string MessageId { get; set; }

            [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
            public string Message { get; set; }

            [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
            public ClientError Error { get; set; }
        }

        /// <summary>
        /// Notifies the app that the message has been delivered to the thread's validators
        /// </summary>
        public class RempSentToValidators : ProcessingEvent
        {
            [JsonProperty("message_id", NullValueHandling = NullValueHandling.Ignore)]
            public string MessageId { get; set; }

            [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
            public BigInteger Timestamp { get; set; }

            [JsonProperty("json", NullValueHandling = NullValueHandling.Ignore)]
            public Newtonsoft.Json.Linq.JToken Json { get; set; }
        }

        /// <summary>
        /// Notifies the app that the message has been successfully included into a block candidate by the
        /// thread's collator
        /// </summary>
        public class RempIncludedIntoBlock : ProcessingEvent
        {
            [JsonProperty("message_id", NullValueHandling = NullValueHandling.Ignore)]
            public string MessageId { get; set; }

            [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
            public BigInteger Timestamp { get; set; }

            [JsonProperty("json", NullValueHandling = NullValueHandling.Ignore)]
            public Newtonsoft.Json.Linq.JToken Json { get; set; }
        }

        /// <summary>
        /// Notifies the app that the block candicate with the message has been accepted by the thread's
        /// validators
        /// </summary>
        public class RempIncludedIntoAcceptedBlock : ProcessingEvent
        {
            [JsonProperty("message_id", NullValueHandling = NullValueHandling.Ignore)]
            public string MessageId { get; set; }

            [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
            public BigInteger Timestamp { get; set; }

            [JsonProperty("json", NullValueHandling = NullValueHandling.Ignore)]
            public Newtonsoft.Json.Linq.JToken Json { get; set; }
        }

        /// <summary>
        /// Notifies the app about some other minor REMP statuses occurring during message processing
        /// </summary>
        public class RempOther : ProcessingEvent
        {
            [JsonProperty("message_id", NullValueHandling = NullValueHandling.Ignore)]
            public string MessageId { get; set; }

            [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
            public BigInteger Timestamp { get; set; }

            [JsonProperty("json", NullValueHandling = NullValueHandling.Ignore)]
            public Newtonsoft.Json.Linq.JToken Json { get; set; }
        }

        /// <summary>
        /// Notifies the app about any problem that has occured in REMP processing - in this case library
        /// switches to the fallback transaction awaiting scenario (sequential block reading).
        /// </summary>
        public class RempError : ProcessingEvent
        {
            [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
            public ClientError Error { get; set; }
        }
    }

    public class ResultOfProcessMessage
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
        /// Transaction fees
        /// </summary>
        [JsonProperty("fees", NullValueHandling = NullValueHandling.Ignore)]
        public TransactionFees Fees { get; set; }
    }

    public class DecodedOutput
    {
        /// <summary>
        /// If the message can't be decoded, then `None` will be stored in
        /// the appropriate position.
        /// </summary>
        [JsonProperty("out_messages", NullValueHandling = NullValueHandling.Ignore)]
        public DecodedMessageBody[] OutMessages { get; set; }

        /// <summary>
        /// Decoded body of the function output message.
        /// </summary>
        [JsonProperty("output", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Output { get; set; }
    }

    public class ParamsOfSendMessage
    {
        /// <summary>
        /// Message BOC.
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        /// If this parameter is specified and the message has the
        /// `expire` header then expiration time will be checked against
        /// the current time to prevent unnecessary sending of already expired message.
        /// 
        /// The `message already expired` error will be returned in this
        /// case.
        /// 
        /// Note, that specifying `abi` for ABI compliant contracts is
        /// strongly recommended, so that proper processing strategy can be
        /// chosen.
        /// </summary>
        [JsonProperty("abi", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public Abi Abi { get; set; }

        /// <summary>
        /// Flag for requesting events sending
        /// </summary>
        [JsonProperty("send_events", NullValueHandling = NullValueHandling.Ignore)]
        public bool SendEvents { get; set; }
    }

    public class ResultOfSendMessage
    {
        /// <summary>
        /// This block id must be used as a parameter of the
        /// `wait_for_transaction`.
        /// </summary>
        [JsonProperty("shard_block_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ShardBlockId { get; set; }

        /// <summary>
        /// This list id must be used as a parameter of the
        /// `wait_for_transaction`.
        /// </summary>
        [JsonProperty("sending_endpoints", NullValueHandling = NullValueHandling.Ignore)]
        public string[] SendingEndpoints { get; set; }
    }

    public class ParamsOfWaitForTransaction
    {
        /// <summary>
        /// If it is specified, then the output messages' bodies will be
        /// decoded according to this ABI.
        /// 
        /// The `abi_decoded` result field will be filled out.
        /// </summary>
        [JsonProperty("abi", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public Abi Abi { get; set; }

        /// <summary>
        /// Encoded with `base64`.
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        /// You must provide the same value as the `send_message` has returned.
        /// </summary>
        [JsonProperty("shard_block_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ShardBlockId { get; set; }

        /// <summary>
        /// Flag that enables/disables intermediate events
        /// </summary>
        [JsonProperty("send_events", NullValueHandling = NullValueHandling.Ignore)]
        public bool SendEvents { get; set; }

        /// <summary>
        /// Use this field to get more informative errors.
        /// Provide the same value as the `send_message` has returned.
        /// If the message was not delivered (expired), SDK will log the endpoint URLs, used for its sending.
        /// </summary>
        [JsonProperty("sending_endpoints", NullValueHandling = NullValueHandling.Ignore)]
        public string[] SendingEndpoints { get; set; }
    }

    public class ParamsOfProcessMessage
    {
        /// <summary>
        /// Message encode parameters.
        /// </summary>
        [JsonProperty("message_encode_params", NullValueHandling = NullValueHandling.Ignore)]
        public ParamsOfEncodeMessage MessageEncodeParams { get; set; }

        /// <summary>
        /// Flag for requesting events sending
        /// </summary>
        [JsonProperty("send_events", NullValueHandling = NullValueHandling.Ignore)]
        public bool SendEvents { get; set; }
    }

    /// <summary>
    /// This module incorporates functions related to complex message
    /// processing scenarios.
    /// </summary>
    public interface IProcessingModule
    {
        /// <summary>
        /// Sends message to the network and returns the last generated shard block of the destination account
        /// before the message was sent. It will be required later for message processing.
        /// </summary>
        Task<ResultOfSendMessage> SendMessageAsync(ParamsOfSendMessage @params, Func<ProcessingEvent, int, Task> callback = null);

        /// <summary>
        /// `send_events` enables intermediate events, such as `WillFetchNextBlock`,
        /// `FetchNextBlockFailed` that may be useful for logging of new shard blocks creation
        /// during message processing.
        /// 
        /// Note, that presence of the `abi` parameter is critical for ABI
        /// compliant contracts. Message processing uses drastically
        /// different strategy for processing message for contracts which
        /// ABI includes "expire" header.
        /// 
        /// When the ABI header `expire` is present, the processing uses
        /// `message expiration` strategy:
        /// - The maximum block gen time is set to
        ///   `message_expiration_timeout + transaction_wait_timeout`.
        /// - When maximum block gen time is reached, the processing will
        ///   be finished with `MessageExpired` error.
        /// 
        /// When the ABI header `expire` isn't present or `abi` parameter
        /// isn't specified, the processing uses `transaction waiting`
        /// strategy:
        /// - The maximum block gen time is set to
        ///   `now() + transaction_wait_timeout`.
        /// 
        /// - If maximum block gen time is reached and no result transaction is found,
        /// the processing will exit with an error.
        /// </summary>
        Task<ResultOfProcessMessage> WaitForTransactionAsync(ParamsOfWaitForTransaction @params, Func<ProcessingEvent, int, Task> callback = null);

        /// <summary>
        /// Creates ABI-compatible message,
        /// sends it to the network and monitors for the result transaction.
        /// Decodes the output messages' bodies.
        /// 
        /// If contract's ABI includes "expire" header, then
        /// SDK implements retries in case of unsuccessful message delivery within the expiration
        /// timeout: SDK recreates the message, sends it and processes it again.
        /// 
        /// The intermediate events, such as `WillFetchFirstBlock`, `WillSend`, `DidSend`,
        /// `WillFetchNextBlock`, etc - are switched on/off by `send_events` flag
        /// and logged into the supplied callback function.
        /// 
        /// The retry configuration parameters are defined in the client's `NetworkConfig` and `AbiConfig`.
        /// 
        /// If contract's ABI does not include "expire" header
        /// then, if no transaction is found within the network timeout (see config parameter ), exits with
        /// error.
        /// </summary>
        Task<ResultOfProcessMessage> ProcessMessageAsync(ParamsOfProcessMessage @params, Func<ProcessingEvent, int, Task> request = null);
    }

    internal class ProcessingModule : IProcessingModule
    {
        private readonly TonClient _client;

        internal ProcessingModule(TonClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<ResultOfSendMessage> SendMessageAsync(ParamsOfSendMessage @params, Func<ProcessingEvent, int, Task> callback = null)
        {
            return await _client.CallFunctionAsync<ResultOfSendMessage, ProcessingEvent>("processing.send_message", @params, callback).ConfigureAwait(false);
        }

        public async Task<ResultOfProcessMessage> WaitForTransactionAsync(ParamsOfWaitForTransaction @params, Func<ProcessingEvent, int, Task> callback = null)
        {
            return await _client.CallFunctionAsync<ResultOfProcessMessage, ProcessingEvent>("processing.wait_for_transaction", @params, callback).ConfigureAwait(false);
        }

        public async Task<ResultOfProcessMessage> ProcessMessageAsync(ParamsOfProcessMessage @params, Func<ProcessingEvent, int, Task> request = null)
        {
            return await _client.CallFunctionAsync<ResultOfProcessMessage, ProcessingEvent>("processing.process_message", @params, request).ConfigureAwait(false);
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

