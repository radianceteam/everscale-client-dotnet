using Newtonsoft.Json;
using System;
using System.Numerics;
using System.Threading.Tasks;
using TonSdk.Modules;

/*
* TON API version 1.0.0, abi module.
* THIS FILE WAS GENERATED AUTOMATICALLY.
*/

namespace TonSdk.Modules
{
    public abstract class Abi
    {

    }

    /// <summary>
    ///  The ABI function header.
    /// 
    ///  Includes several hidden function parameters that contract
    ///  uses for security and replay protection reasons.
    /// 
    ///  The actual set of header fields depends on the contract's ABI.
    /// </summary>
    public class FunctionHeader
    {
        /// <summary>
        ///  Message expiration time in seconds.
        /// </summary>
        [JsonProperty("expire")]
        public int? Expire { get; set; }

        /// <summary>
        ///  Message creation time in milliseconds.
        /// </summary>
        [JsonProperty("time")]
        public BigInteger Time { get; set; }

        /// <summary>
        ///  Public key used to sign message. Encoded with `hex`.
        /// </summary>
        [JsonProperty("pubkey")]
        public string Pubkey { get; set; }
    }

    public class CallSet
    {
        /// <summary>
        ///  Function name that is being called.
        /// </summary>
        [JsonProperty("function_name")]
        public string FunctionName { get; set; }

        /// <summary>
        ///  Function header.
        /// 
        ///  If an application omits some header parameters required by the
        ///  contract's ABI, the library will set the default values for
        ///  them.
        /// </summary>
        [JsonProperty("header")]
        public FunctionHeader Header { get; set; }

        /// <summary>
        ///  Function input parameters according to ABI.
        /// </summary>
        [JsonProperty("input")]
        public Newtonsoft.Json.Linq.JToken Input { get; set; }
    }

    public class DeploySet
    {
        /// <summary>
        ///  Content of TVC file encoded in `base64`.
        /// </summary>
        [JsonProperty("tvc")]
        public string Tvc { get; set; }

        /// <summary>
        ///  Target workchain for destination address. Default is `0`.
        /// </summary>
        [JsonProperty("workchain_id")]
        public int? WorkchainId { get; set; }

        /// <summary>
        ///  List of initial values for contract's public variables.
        /// </summary>
        [JsonProperty("initial_data")]
        public Newtonsoft.Json.Linq.JToken InitialData { get; set; }
    }

    public abstract class Signer
    {
        /// <summary>
        ///  No keys are provided. Creates an unsigned message. 
        /// </summary>
        public class None : Signer
        {
        }

        /// <summary>
        ///  Only public key is provided to generate unsigned message and `data_to_sign`
        ///  which can be signed later.  
        /// </summary>
        public class External : Signer
        {
            [JsonProperty("public_key")]
            public string PublicKey { get; set; }
        }

        /// <summary>
        ///  Key pair is provided for signing
        /// </summary>
        public class Keys : Signer
        {
            [JsonProperty("keys")]
            public KeyPair KeysProperty { get; set; }
        }

        /// <summary>
        ///  Signing Box interface is provided for signing, allows Dapps to sign messages using external APIs,
        ///  such as HSM, cold wallet, etc.
        /// </summary>
        public class SigningBox : Signer
        {
            [JsonProperty("handle")]
            public decimal Handle { get; set; }
        }
    }

    public enum MessageBodyType
    {
        /// <summary>
        ///  Message contains the input of the ABI function.
        /// </summary>
        Input,
        /// <summary>
        ///  Message contains the output of the ABI function.
        /// </summary>
        Output,
        /// <summary>
        ///  Message contains the input of the imported ABI function.
        /// 
        ///  Occurs when contract sends an internal message to other
        ///  contract.
        /// </summary>
        InternalOutput,
        /// <summary>
        ///  Message contains the input of the ABI event.
        /// </summary>
        Event,
    }

    public abstract class StateInitSource
    {
        /// <summary>
        ///  Deploy message.
        /// </summary>
        public class Message : StateInitSource
        {
            [JsonProperty("source")]
            public MessageSource Source { get; set; }
        }

        /// <summary>
        ///  State init data.
        /// </summary>
        public class StateInit : StateInitSource
        {
            /// <summary>
            ///  Code BOC. Encoded in `base64`.
            /// </summary>
            [JsonProperty("code")]
            public string Code { get; set; }

            /// <summary>
            ///  Data BOC. Encoded in `base64`.
            /// </summary>
            [JsonProperty("data")]
            public string Data { get; set; }

            /// <summary>
            ///  Library BOC. Encoded in `base64`.
            /// </summary>
            [JsonProperty("library")]
            public string Library { get; set; }
        }

        /// <summary>
        ///  Content of the TVC file. Encoded in `base64`.
        /// </summary>
        public class Tvc : StateInitSource
        {
            [JsonProperty("tvc")]
            public string TvcProperty { get; set; }

            [JsonProperty("public_key")]
            public string PublicKey { get; set; }

            [JsonProperty("init_params")]
            public StateInitParams InitParams { get; set; }
        }
    }

    public class StateInitParams
    {
        [JsonProperty("abi")]
        public Abi Abi { get; set; }

        [JsonProperty("value")]
        public Newtonsoft.Json.Linq.JToken Value { get; set; }
    }

    public abstract class MessageSource
    {
        public class Encoded : MessageSource
        {
            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("abi")]
            public Abi Abi { get; set; }
        }

    }

    public class ParamsOfEncodeMessageBody
    {
        /// <summary>
        ///  Contract ABI.
        /// </summary>
        [JsonProperty("abi")]
        public Abi Abi { get; set; }

        /// <summary>
        ///  Function call parameters.
        /// 
        ///  Must be specified in non deploy message.
        /// 
        ///  In case of deploy message contains parameters of constructor.
        /// </summary>
        [JsonProperty("call_set")]
        public CallSet CallSet { get; set; }

        /// <summary>
        ///  True if internal message body must be encoded.
        /// </summary>
        [JsonProperty("is_internal")]
        public bool IsInternal { get; set; }

        /// <summary>
        ///  Signing parameters.
        /// </summary>
        [JsonProperty("signer")]
        public Signer Signer { get; set; }

        /// <summary>
        ///  Processing try index.
        /// 
        ///  Used in message processing with retries.
        /// 
        ///  Encoder uses the provided try index to calculate message
        ///  expiration time.
        /// 
        ///  Expiration timeouts will grow with every retry.
        /// 
        ///  Default value is 0.
        /// </summary>
        [JsonProperty("processing_try_index")]
        public int? ProcessingTryIndex { get; set; }
    }

    public class ResultOfEncodeMessageBody
    {
        /// <summary>
        ///  Message body BOC encoded with `base64`.
        /// </summary>
        [JsonProperty("body")]
        public string Body { get; set; }

        /// <summary>
        ///  Optional data to sign. Encoded with `base64`.
        /// 
        ///  Presents when `message` is unsigned. Can be used for external
        ///  message signing. Is this case you need to sing this data and
        ///  produce signed message using `abi.attach_signature`.
        /// </summary>
        [JsonProperty("data_to_sign")]
        public string DataToSign { get; set; }
    }

    public class ParamsOfAttachSignatureToMessageBody
    {
        /// <summary>
        ///  Contract ABI
        /// </summary>
        [JsonProperty("abi")]
        public Abi Abi { get; set; }

        /// <summary>
        ///  Public key. Must be encoded with `hex`.
        /// </summary>
        [JsonProperty("public_key")]
        public string PublicKey { get; set; }

        /// <summary>
        ///  Unsigned message BOC. Must be encoded with `base64`.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        ///  Signature. Must be encoded with `hex`.
        /// </summary>
        [JsonProperty("signature")]
        public string Signature { get; set; }
    }

    public class ResultOfAttachSignatureToMessageBody
    {
        [JsonProperty("body")]
        public string Body { get; set; }
    }

    public class ParamsOfEncodeMessage
    {
        /// <summary>
        ///  Contract ABI.
        /// </summary>
        [JsonProperty("abi")]
        public Abi Abi { get; set; }

        /// <summary>
        ///  Target address the message will be sent to.
        /// 
        ///  Must be specified in case of non-deploy message.
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }

        /// <summary>
        ///  Deploy parameters.
        /// 
        ///  Must be specified in case of deploy message.
        /// </summary>
        [JsonProperty("deploy_set")]
        public DeploySet DeploySet { get; set; }

        /// <summary>
        ///  Function call parameters.
        /// 
        ///  Must be specified in case of non-deploy message.
        /// 
        ///  In case of deploy message it is optional and contains parameters
        ///  of the functions that will to be called upon deploy transaction.
        /// </summary>
        [JsonProperty("call_set")]
        public CallSet CallSet { get; set; }

        /// <summary>
        ///  Signing parameters.
        /// </summary>
        [JsonProperty("signer")]
        public Signer Signer { get; set; }

        /// <summary>
        ///  Processing try index.
        /// 
        ///  Used in message processing with retries (if contract's ABI includes "expire" header).
        /// 
        ///  Encoder uses the provided try index to calculate message
        ///  expiration time. The 1st message expiration time is specified in
        ///  Client config.
        /// 
        ///  Expiration timeouts will grow with every retry.
        ///  Retry grow factor is set in Client config:
        ///  <.....add config parameter with default value here>
        /// 
        ///  Default value is 0.
        /// </summary>
        [JsonProperty("processing_try_index")]
        public int? ProcessingTryIndex { get; set; }
    }

    public class ResultOfEncodeMessage
    {
        /// <summary>
        ///  Message BOC encoded with `base64`.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        ///  Optional data to be signed encoded in `base64`.
        /// 
        ///  Returned in case of `Signer::External`. Can be used for external
        ///  message signing. Is this case you need to use this data to create signature and
        ///  then produce signed message using `abi.attach_signature`.
        /// </summary>
        [JsonProperty("data_to_sign")]
        public string DataToSign { get; set; }

        /// <summary>
        ///  Destination address.
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }

        /// <summary>
        ///  Message id.
        /// </summary>
        [JsonProperty("message_id")]
        public string MessageId { get; set; }
    }

    public class ParamsOfAttachSignature
    {
        /// <summary>
        ///  Contract ABI
        /// </summary>
        [JsonProperty("abi")]
        public Abi Abi { get; set; }

        /// <summary>
        ///  Public key encoded in `hex`.
        /// </summary>
        [JsonProperty("public_key")]
        public string PublicKey { get; set; }

        /// <summary>
        ///  Unsigned message BOC encoded in `base64`.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        ///  Signature encoded in `hex`.
        /// </summary>
        [JsonProperty("signature")]
        public string Signature { get; set; }
    }

    public class ResultOfAttachSignature
    {
        /// <summary>
        ///  Signed message BOC
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        ///  Message ID
        /// </summary>
        [JsonProperty("message_id")]
        public string MessageId { get; set; }
    }

    public class ParamsOfDecodeMessage
    {
        /// <summary>
        ///  contract ABI
        /// </summary>
        [JsonProperty("abi")]
        public Abi Abi { get; set; }

        /// <summary>
        ///  Message BOC
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class DecodedMessageBody
    {
        /// <summary>
        ///  Type of the message body content.
        /// </summary>
        [JsonProperty("body_type")]
        public MessageBodyType BodyType { get; set; }

        /// <summary>
        ///  Function or event name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        ///  Parameters or result value.
        /// </summary>
        [JsonProperty("value")]
        public Newtonsoft.Json.Linq.JToken Value { get; set; }

        /// <summary>
        ///  Function header.
        /// </summary>
        [JsonProperty("header")]
        public FunctionHeader Header { get; set; }
    }

    public class ParamsOfDecodeMessageBody
    {
        /// <summary>
        ///  Contract ABI used to decode.
        /// </summary>
        [JsonProperty("abi")]
        public Abi Abi { get; set; }

        /// <summary>
        ///  Message body BOC encoded in `base64`.
        /// </summary>
        [JsonProperty("body")]
        public string Body { get; set; }

        /// <summary>
        ///  True if the body belongs to the internal message.
        /// </summary>
        [JsonProperty("is_internal")]
        public bool IsInternal { get; set; }
    }

    public class ParamsOfEncodeAccount
    {
        /// <summary>
        ///  Source of the account state init.
        /// </summary>
        [JsonProperty("state_init")]
        public StateInitSource StateInit { get; set; }

        /// <summary>
        ///  Initial balance.
        /// </summary>
        [JsonProperty("balance")]
        public BigInteger Balance { get; set; }

        /// <summary>
        ///  Initial value for the `last_trans_lt`.
        /// </summary>
        [JsonProperty("last_trans_lt")]
        public BigInteger LastTransLt { get; set; }

        /// <summary>
        ///  Initial value for the `last_paid`.
        /// </summary>
        [JsonProperty("last_paid")]
        public int? LastPaid { get; set; }
    }

    public class ResultOfEncodeAccount
    {
        /// <summary>
        ///  Account BOC encoded in `base64`.
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }

        /// <summary>
        ///  Account ID  encoded in `hex`.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    /// <summary>
    ///  Provides message encoding and decoding according to the ABI
    ///  specification.
    /// </summary>
    public interface IAbiModule
    {
        /// <summary>
        ///  Encodes message body according to ABI function call.
        /// </summary>
        Task<ResultOfEncodeMessageBody> EncodeMessageBodyAsync(ParamsOfEncodeMessageBody @params);

        Task<ResultOfAttachSignatureToMessageBody> AttachSignatureToMessageBodyAsync(ParamsOfAttachSignatureToMessageBody @params);

        /// <summary>
        ///  Encodes an ABI-compatible message
        /// 
        ///  Allows to encode deploy and function call messages,
        ///  both signed and unsigned.
        /// 
        ///  Use cases include messages of any possible type:
        /// - deploy with initial function call (i.e. `constructor` or any other function that is used for some
        /// kind
        ///  of initialization);
        ///  - deploy without initial function call;
        ///  - signed/unsigned + data for signing.
        /// 
        ///  `Signer` defines how the message should or shouldn't be signed:
        /// 
        ///  `Signer::None` creates an unsigned message. This may be needed in case of some public methods,
        ///  that do not require authorization by pubkey.
        /// 
        ///  `Signer::External` takes public key and returns `data_to_sign` for later signing.
        ///  Use `attach_signature` method with the result signature to get the signed message.
        /// 
        ///  `Signer::Keys` creates a signed message with provided key pair.
        /// 
        ///  [SOON] `Signer::SigningBox` Allows using a special interface to imlepement signing
        ///  without private key disclosure to SDK. For instance, in case of using a cold wallet or HSM,
        ///  when application calls some API to sign data.
        /// </summary>
        Task<ResultOfEncodeMessage> EncodeMessageAsync(ParamsOfEncodeMessage @params);

        /// <summary>
        ///  Combines `hex`-encoded `signature` with `base64`-encoded `unsigned_message`.
        ///  Returns signed message encoded in `base64`.
        /// </summary>
        Task<ResultOfAttachSignature> AttachSignatureAsync(ParamsOfAttachSignature @params);

        /// <summary>
        ///  Decodes message body using provided message BOC and ABI.
        /// </summary>
        Task<DecodedMessageBody> DecodeMessageAsync(ParamsOfDecodeMessage @params);

        /// <summary>
        ///  Decodes message body using provided body BOC and ABI.
        /// </summary>
        Task<DecodedMessageBody> DecodeMessageBodyAsync(ParamsOfDecodeMessageBody @params);

        /// <summary>
        ///  Creates account state BOC
        ///  
        ///  Creates account state provided with one of these sets of data :
        ///  1. BOC of code, BOC of data, BOC of library
        ///  2. TVC (string in `base64`), keys, init params
        /// </summary>
        Task<ResultOfEncodeAccount> EncodeAccountAsync(ParamsOfEncodeAccount @params);
    }

    internal class AbiModule : IAbiModule
    {
        private readonly TonClient _client;

        internal AbiModule(TonClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<ResultOfEncodeMessageBody> EncodeMessageBodyAsync(ParamsOfEncodeMessageBody @params)
        {
            return await _client.CallFunctionAsync<ResultOfEncodeMessageBody>("abi.encode_message_body", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfAttachSignatureToMessageBody> AttachSignatureToMessageBodyAsync(ParamsOfAttachSignatureToMessageBody @params)
        {
            return await _client.CallFunctionAsync<ResultOfAttachSignatureToMessageBody>("abi.attach_signature_to_message_body", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfEncodeMessage> EncodeMessageAsync(ParamsOfEncodeMessage @params)
        {
            return await _client.CallFunctionAsync<ResultOfEncodeMessage>("abi.encode_message", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfAttachSignature> AttachSignatureAsync(ParamsOfAttachSignature @params)
        {
            return await _client.CallFunctionAsync<ResultOfAttachSignature>("abi.attach_signature", @params).ConfigureAwait(false);
        }

        public async Task<DecodedMessageBody> DecodeMessageAsync(ParamsOfDecodeMessage @params)
        {
            return await _client.CallFunctionAsync<DecodedMessageBody>("abi.decode_message", @params).ConfigureAwait(false);
        }

        public async Task<DecodedMessageBody> DecodeMessageBodyAsync(ParamsOfDecodeMessageBody @params)
        {
            return await _client.CallFunctionAsync<DecodedMessageBody>("abi.decode_message_body", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfEncodeAccount> EncodeAccountAsync(ParamsOfEncodeAccount @params)
        {
            return await _client.CallFunctionAsync<ResultOfEncodeAccount>("abi.encode_account", @params).ConfigureAwait(false);
        }
    }
}

namespace TonSdk
{
    public partial interface ITonClient
    {
        IAbiModule Abi { get; }
    }

    public partial class TonClient
    {
        private AbiModule _abiModule;

        public IAbiModule Abi
        {
            get
            {
                return _abiModule ?? (_abiModule = new AbiModule(this));
            }
        }
    }
}

