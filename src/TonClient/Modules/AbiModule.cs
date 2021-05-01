using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Numerics;
using System.Threading.Tasks;
using TonSdk.Modules;

/*
* TON API version 1.13.0, abi module.
* THIS FILE WAS GENERATED AUTOMATICALLY.
*/

namespace TonSdk.Modules
{
    public enum AbiErrorCode
    {
        RequiredAddressMissingForEncodeMessage = 301,
        RequiredCallSetMissingForEncodeMessage = 302,
        InvalidJson = 303,
        InvalidMessage = 304,
        EncodeDeployMessageFailed = 305,
        EncodeRunMessageFailed = 306,
        AttachSignatureFailed = 307,
        InvalidTvcImage = 308,
        RequiredPublicKeyMissingForFunctionHeader = 309,
        InvalidSigner = 310,
        InvalidAbi = 311,
        InvalidFunctionId = 312,
    }

    public abstract class Abi
    {
        public class Contract : Abi
        {
            [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
            public AbiContract Value { get; set; }
        }

        public class Json : Abi
        {
            [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
            public string Value { get; set; }
        }

        public class Handle : Abi
        {
            [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
            public uint Value { get; set; }
        }

        public class Serialized : Abi
        {
            [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
            public AbiContract Value { get; set; }
        }
    }

    /// <summary>
    /// Includes several hidden function parameters that contract
    /// uses for security, message delivery monitoring and replay protection reasons.
    /// 
    /// The actual set of header fields depends on the contract's ABI.
    /// If a contract's ABI does not include some headers, then they are not filled.
    /// </summary>
    public class FunctionHeader
    {
        /// <summary>
        /// Message expiration time in seconds. If not specified - calculated automatically from
        /// message_expiration_timeout(), try_index and message_expiration_timeout_grow_factor() (if ABI
        /// includes `expire` header).
        /// </summary>
        [JsonProperty("expire", NullValueHandling = NullValueHandling.Ignore)]
        public uint? Expire { get; set; }

        /// <summary>
        /// If not specified, `now` is used (if ABI includes `time` header).
        /// </summary>
        [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        public BigInteger Time { get; set; }

        /// <summary>
        /// Encoded in `hex`. If not specified, method fails with exception (if ABI includes `pubkey` header)..
        /// </summary>
        [JsonProperty("pubkey", NullValueHandling = NullValueHandling.Ignore)]
        public string Pubkey { get; set; }
    }

    public class CallSet
    {
        /// <summary>
        /// Function name that is being called. Or function id encoded as string in hex (starting with 0x).
        /// </summary>
        [JsonProperty("function_name", NullValueHandling = NullValueHandling.Ignore)]
        public string FunctionName { get; set; }

        /// <summary>
        /// If an application omits some header parameters required by the
        /// contract's ABI, the library will set the default values for
        /// them.
        /// </summary>
        [JsonProperty("header", NullValueHandling = NullValueHandling.Ignore)]
        public FunctionHeader Header { get; set; }

        /// <summary>
        /// Function input parameters according to ABI.
        /// </summary>
        [JsonProperty("input", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Input { get; set; }
    }

    public class DeploySet
    {
        /// <summary>
        /// Content of TVC file encoded in `base64`.
        /// </summary>
        [JsonProperty("tvc", NullValueHandling = NullValueHandling.Ignore)]
        public string Tvc { get; set; }

        /// <summary>
        /// Default is `0`.
        /// </summary>
        [JsonProperty("workchain_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? WorkchainId { get; set; }

        /// <summary>
        /// List of initial values for contract's public variables.
        /// </summary>
        [JsonProperty("initial_data", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken InitialData { get; set; }

        /// <summary>
        /// Public key resolving priority:
        /// 1. Public key from deploy set.
        /// 2. Public key, specified in TVM file.
        /// 3. Public key, provided by Signer.
        /// </summary>
        [JsonProperty("initial_pubkey", NullValueHandling = NullValueHandling.Ignore)]
        public string InitialPubkey { get; set; }
    }

    public abstract class Signer
    {
        /// <summary>
        /// Creates an unsigned message.
        /// </summary>
        public class None : Signer
        {
        }

        /// <summary>
        /// Only public key is provided in unprefixed hex string format to generate unsigned message and
        /// `data_to_sign` which can be signed later.
        /// </summary>
        public class External : Signer
        {
            [JsonProperty("public_key", NullValueHandling = NullValueHandling.Ignore)]
            public string PublicKey { get; set; }
        }

        /// <summary>
        /// Key pair is provided for signing
        /// </summary>
        public class Keys : Signer
        {
            [JsonProperty("keys", NullValueHandling = NullValueHandling.Ignore)]
            public KeyPair KeysProperty { get; set; }
        }

        /// <summary>
        /// Signing Box interface is provided for signing, allows Dapps to sign messages using external APIs,
        /// such as HSM, cold wallet, etc.
        /// </summary>
        public class SigningBox : Signer
        {
            [JsonProperty("handle", NullValueHandling = NullValueHandling.Ignore)]
            public uint Handle { get; set; }
        }
    }

    public enum MessageBodyType
    {
        /// <summary>
        /// Message contains the input of the ABI function.
        /// </summary>
        Input,
        /// <summary>
        /// Message contains the output of the ABI function.
        /// </summary>
        Output,
        /// <summary>
        /// Occurs when contract sends an internal message to other
        /// contract.
        /// </summary>
        InternalOutput,
        /// <summary>
        /// Message contains the input of the ABI event.
        /// </summary>
        Event,
    }

    public abstract class StateInitSource
    {
        /// <summary>
        /// Deploy message.
        /// </summary>
        public class Message : StateInitSource
        {
            [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
            [JsonConverter(typeof(PolymorphicTypeConverter))]
            public MessageSource Source { get; set; }
        }

        /// <summary>
        /// State init data.
        /// </summary>
        public class StateInit : StateInitSource
        {
            /// <summary>
            /// Encoded in `base64`.
            /// </summary>
            [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
            public string Code { get; set; }

            /// <summary>
            /// Encoded in `base64`.
            /// </summary>
            [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
            public string Data { get; set; }

            /// <summary>
            /// Encoded in `base64`.
            /// </summary>
            [JsonProperty("library", NullValueHandling = NullValueHandling.Ignore)]
            public string Library { get; set; }
        }

        /// <summary>
        /// Encoded in `base64`.
        /// </summary>
        public class Tvc : StateInitSource
        {
            [JsonProperty("tvc", NullValueHandling = NullValueHandling.Ignore)]
            public string TvcProperty { get; set; }

            [JsonProperty("public_key", NullValueHandling = NullValueHandling.Ignore)]
            public string PublicKey { get; set; }

            [JsonProperty("init_params", NullValueHandling = NullValueHandling.Ignore)]
            public StateInitParams InitParams { get; set; }
        }
    }

    public class StateInitParams
    {
        [JsonProperty("abi", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public Abi Abi { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Value { get; set; }
    }

    public abstract class MessageSource
    {
        public class Encoded : MessageSource
        {
            [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
            public string Message { get; set; }

            [JsonProperty("abi", NullValueHandling = NullValueHandling.Ignore)]
            [JsonConverter(typeof(PolymorphicTypeConverter))]
            public Abi Abi { get; set; }
        }

        public class EncodingParams : MessageSource
        {
            /// <summary>
            /// Contract ABI.
            /// </summary>
            [JsonProperty("abi", NullValueHandling = NullValueHandling.Ignore)]
            [JsonConverter(typeof(PolymorphicTypeConverter))]
            public Abi Abi { get; set; }

            /// <summary>
            /// Must be specified in case of non-deploy message.
            /// </summary>
            [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
            public string Address { get; set; }

            /// <summary>
            /// Must be specified in case of deploy message.
            /// </summary>
            [JsonProperty("deploy_set", NullValueHandling = NullValueHandling.Ignore)]
            public DeploySet DeploySet { get; set; }

            /// <summary>
            /// Must be specified in case of non-deploy message.
            /// 
            /// In case of deploy message it is optional and contains parameters
            /// of the functions that will to be called upon deploy transaction.
            /// </summary>
            [JsonProperty("call_set", NullValueHandling = NullValueHandling.Ignore)]
            public CallSet CallSet { get; set; }

            /// <summary>
            /// Signing parameters.
            /// </summary>
            [JsonProperty("signer", NullValueHandling = NullValueHandling.Ignore)]
            [JsonConverter(typeof(PolymorphicTypeConverter))]
            public Signer Signer { get; set; }

            /// <summary>
            /// Used in message processing with retries (if contract's ABI includes "expire" header).
            /// 
            /// Encoder uses the provided try index to calculate message
            /// expiration time. The 1st message expiration time is specified in
            /// Client config.
            /// 
            /// Expiration timeouts will grow with every retry.
            /// Retry grow factor is set in Client config:
            /// <.....add config parameter with default value here>
            /// 
            /// Default value is 0.
            /// </summary>
            [JsonProperty("processing_try_index", NullValueHandling = NullValueHandling.Ignore)]
            public byte? ProcessingTryIndex { get; set; }
        }
    }

    public class AbiParam
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("components", NullValueHandling = NullValueHandling.Ignore)]
        public AbiParam[] Components { get; set; }
    }

    public class AbiEvent
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("inputs", NullValueHandling = NullValueHandling.Ignore)]
        public AbiParam[] Inputs { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
    }

    public class AbiData
    {
        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public BigInteger Key { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("components", NullValueHandling = NullValueHandling.Ignore)]
        public AbiParam[] Components { get; set; }
    }

    public class AbiFunction
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("inputs", NullValueHandling = NullValueHandling.Ignore)]
        public AbiParam[] Inputs { get; set; }

        [JsonProperty("outputs", NullValueHandling = NullValueHandling.Ignore)]
        public AbiParam[] Outputs { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
    }

    public class AbiContract
    {
        [JsonProperty("ABI version", NullValueHandling = NullValueHandling.Ignore)]
        public uint? ABIVersion { get; set; } = TonClient.DefaultAbiVersion;

        [JsonProperty("abi_version", NullValueHandling = NullValueHandling.Ignore)]
        public uint? AbiVersion { get; set; } = TonClient.DefaultAbiVersion;

        [JsonProperty("header", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Header { get; set; }

        [JsonProperty("functions", NullValueHandling = NullValueHandling.Ignore)]
        public AbiFunction[] Functions { get; set; }

        [JsonProperty("events", NullValueHandling = NullValueHandling.Ignore)]
        public AbiEvent[] Events { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public AbiData[] Data { get; set; }
    }

    public class ParamsOfEncodeMessageBody
    {
        /// <summary>
        /// Contract ABI.
        /// </summary>
        [JsonProperty("abi", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public Abi Abi { get; set; }

        /// <summary>
        /// Must be specified in non deploy message.
        /// 
        /// In case of deploy message contains parameters of constructor.
        /// </summary>
        [JsonProperty("call_set", NullValueHandling = NullValueHandling.Ignore)]
        public CallSet CallSet { get; set; }

        /// <summary>
        /// True if internal message body must be encoded.
        /// </summary>
        [JsonProperty("is_internal", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsInternal { get; set; }

        /// <summary>
        /// Signing parameters.
        /// </summary>
        [JsonProperty("signer", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public Signer Signer { get; set; }

        /// <summary>
        /// Used in message processing with retries.
        /// 
        /// Encoder uses the provided try index to calculate message
        /// expiration time.
        /// 
        /// Expiration timeouts will grow with every retry.
        /// 
        /// Default value is 0.
        /// </summary>
        [JsonProperty("processing_try_index", NullValueHandling = NullValueHandling.Ignore)]
        public byte? ProcessingTryIndex { get; set; }
    }

    public class ResultOfEncodeMessageBody
    {
        /// <summary>
        /// Message body BOC encoded with `base64`.
        /// </summary>
        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public string Body { get; set; }

        /// <summary>
        /// Encoded with `base64`. 
        /// Presents when `message` is unsigned. Can be used for external
        /// message signing. Is this case you need to sing this data and
        /// produce signed message using `abi.attach_signature`.
        /// </summary>
        [JsonProperty("data_to_sign", NullValueHandling = NullValueHandling.Ignore)]
        public string DataToSign { get; set; }
    }

    public class ParamsOfAttachSignatureToMessageBody
    {
        /// <summary>
        /// Contract ABI
        /// </summary>
        [JsonProperty("abi", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public Abi Abi { get; set; }

        /// <summary>
        /// Must be encoded with `hex`.
        /// </summary>
        [JsonProperty("public_key", NullValueHandling = NullValueHandling.Ignore)]
        public string PublicKey { get; set; }

        /// <summary>
        /// Must be encoded with `base64`.
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        /// Must be encoded with `hex`.
        /// </summary>
        [JsonProperty("signature", NullValueHandling = NullValueHandling.Ignore)]
        public string Signature { get; set; }
    }

    public class ResultOfAttachSignatureToMessageBody
    {
        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public string Body { get; set; }
    }

    public class ParamsOfEncodeMessage
    {
        /// <summary>
        /// Contract ABI.
        /// </summary>
        [JsonProperty("abi", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public Abi Abi { get; set; }

        /// <summary>
        /// Must be specified in case of non-deploy message.
        /// </summary>
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        /// <summary>
        /// Must be specified in case of deploy message.
        /// </summary>
        [JsonProperty("deploy_set", NullValueHandling = NullValueHandling.Ignore)]
        public DeploySet DeploySet { get; set; }

        /// <summary>
        /// Must be specified in case of non-deploy message.
        /// 
        /// In case of deploy message it is optional and contains parameters
        /// of the functions that will to be called upon deploy transaction.
        /// </summary>
        [JsonProperty("call_set", NullValueHandling = NullValueHandling.Ignore)]
        public CallSet CallSet { get; set; }

        /// <summary>
        /// Signing parameters.
        /// </summary>
        [JsonProperty("signer", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public Signer Signer { get; set; }

        /// <summary>
        /// Used in message processing with retries (if contract's ABI includes "expire" header).
        /// 
        /// Encoder uses the provided try index to calculate message
        /// expiration time. The 1st message expiration time is specified in
        /// Client config.
        /// 
        /// Expiration timeouts will grow with every retry.
        /// Retry grow factor is set in Client config:
        /// <.....add config parameter with default value here>
        /// 
        /// Default value is 0.
        /// </summary>
        [JsonProperty("processing_try_index", NullValueHandling = NullValueHandling.Ignore)]
        public byte? ProcessingTryIndex { get; set; }
    }

    public class ResultOfEncodeMessage
    {
        /// <summary>
        /// Message BOC encoded with `base64`.
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        /// Returned in case of `Signer::External`. Can be used for external
        /// message signing. Is this case you need to use this data to create signature and
        /// then produce signed message using `abi.attach_signature`.
        /// </summary>
        [JsonProperty("data_to_sign", NullValueHandling = NullValueHandling.Ignore)]
        public string DataToSign { get; set; }

        /// <summary>
        /// Destination address.
        /// </summary>
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        /// <summary>
        /// Message id.
        /// </summary>
        [JsonProperty("message_id", NullValueHandling = NullValueHandling.Ignore)]
        public string MessageId { get; set; }
    }

    public class ParamsOfEncodeInternalMessage
    {
        /// <summary>
        /// Can be None if both deploy_set and call_set are None.
        /// </summary>
        [JsonProperty("abi", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public Abi Abi { get; set; }

        /// <summary>
        /// Must be specified in case of non-deploy message.
        /// </summary>
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        /// <summary>
        /// Source address of the message.
        /// </summary>
        [JsonProperty("src_address", NullValueHandling = NullValueHandling.Ignore)]
        public string SrcAddress { get; set; }

        /// <summary>
        /// Must be specified in case of deploy message.
        /// </summary>
        [JsonProperty("deploy_set", NullValueHandling = NullValueHandling.Ignore)]
        public DeploySet DeploySet { get; set; }

        /// <summary>
        /// Must be specified in case of non-deploy message.
        /// 
        /// In case of deploy message it is optional and contains parameters
        /// of the functions that will to be called upon deploy transaction.
        /// </summary>
        [JsonProperty("call_set", NullValueHandling = NullValueHandling.Ignore)]
        public CallSet CallSet { get; set; }

        /// <summary>
        /// Value in nanotokens to be sent with message.
        /// </summary>
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        /// <summary>
        /// Default is true.
        /// </summary>
        [JsonProperty("bounce", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Bounce { get; set; }

        /// <summary>
        /// Default is false.
        /// </summary>
        [JsonProperty("enable_ihr", NullValueHandling = NullValueHandling.Ignore)]
        public bool? EnableIhr { get; set; }
    }

    public class ResultOfEncodeInternalMessage
    {
        /// <summary>
        /// Message BOC encoded with `base64`.
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        /// Destination address.
        /// </summary>
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        /// <summary>
        /// Message id.
        /// </summary>
        [JsonProperty("message_id", NullValueHandling = NullValueHandling.Ignore)]
        public string MessageId { get; set; }
    }

    public class ParamsOfAttachSignature
    {
        /// <summary>
        /// Contract ABI
        /// </summary>
        [JsonProperty("abi", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public Abi Abi { get; set; }

        /// <summary>
        /// Public key encoded in `hex`.
        /// </summary>
        [JsonProperty("public_key", NullValueHandling = NullValueHandling.Ignore)]
        public string PublicKey { get; set; }

        /// <summary>
        /// Unsigned message BOC encoded in `base64`.
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        /// Signature encoded in `hex`.
        /// </summary>
        [JsonProperty("signature", NullValueHandling = NullValueHandling.Ignore)]
        public string Signature { get; set; }
    }

    public class ResultOfAttachSignature
    {
        /// <summary>
        /// Signed message BOC
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        /// Message ID
        /// </summary>
        [JsonProperty("message_id", NullValueHandling = NullValueHandling.Ignore)]
        public string MessageId { get; set; }
    }

    public class ParamsOfDecodeMessage
    {
        /// <summary>
        /// contract ABI
        /// </summary>
        [JsonProperty("abi", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public Abi Abi { get; set; }

        /// <summary>
        /// Message BOC
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }

    public class DecodedMessageBody
    {
        /// <summary>
        /// Type of the message body content.
        /// </summary>
        [JsonProperty("body_type", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageBodyType BodyType { get; set; }

        /// <summary>
        /// Function or event name.
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Parameters or result value.
        /// </summary>
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Value { get; set; }

        /// <summary>
        /// Function header.
        /// </summary>
        [JsonProperty("header", NullValueHandling = NullValueHandling.Ignore)]
        public FunctionHeader Header { get; set; }
    }

    public class ParamsOfDecodeMessageBody
    {
        /// <summary>
        /// Contract ABI used to decode.
        /// </summary>
        [JsonProperty("abi", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public Abi Abi { get; set; }

        /// <summary>
        /// Message body BOC encoded in `base64`.
        /// </summary>
        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public string Body { get; set; }

        /// <summary>
        /// True if the body belongs to the internal message.
        /// </summary>
        [JsonProperty("is_internal", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsInternal { get; set; }
    }

    public class ParamsOfEncodeAccount
    {
        /// <summary>
        /// Source of the account state init.
        /// </summary>
        [JsonProperty("state_init", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public StateInitSource StateInit { get; set; }

        /// <summary>
        /// Initial balance.
        /// </summary>
        [JsonProperty("balance", NullValueHandling = NullValueHandling.Ignore)]
        public BigInteger Balance { get; set; }

        /// <summary>
        /// Initial value for the `last_trans_lt`.
        /// </summary>
        [JsonProperty("last_trans_lt", NullValueHandling = NullValueHandling.Ignore)]
        public BigInteger LastTransLt { get; set; }

        /// <summary>
        /// Initial value for the `last_paid`.
        /// </summary>
        [JsonProperty("last_paid", NullValueHandling = NullValueHandling.Ignore)]
        public uint? LastPaid { get; set; }

        /// <summary>
        /// The BOC itself returned if no cache type provided
        /// </summary>
        [JsonProperty("boc_cache", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public BocCacheType BocCache { get; set; }
    }

    public class ResultOfEncodeAccount
    {
        /// <summary>
        /// Account BOC encoded in `base64`.
        /// </summary>
        [JsonProperty("account", NullValueHandling = NullValueHandling.Ignore)]
        public string Account { get; set; }

        /// <summary>
        /// Account ID  encoded in `hex`.
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
    }

    /// <summary>
    /// Provides message encoding and decoding according to the ABI specification.
    /// </summary>
    public interface IAbiModule
    {
        /// <summary>
        /// Encodes message body according to ABI function call.
        /// </summary>
        Task<ResultOfEncodeMessageBody> EncodeMessageBodyAsync(ParamsOfEncodeMessageBody @params);

        Task<ResultOfAttachSignatureToMessageBody> AttachSignatureToMessageBodyAsync(ParamsOfAttachSignatureToMessageBody @params);

        /// <summary>
        /// Allows to encode deploy and function call messages,
        /// both signed and unsigned.
        /// 
        /// Use cases include messages of any possible type:
        /// - deploy with initial function call (i.e. `constructor` or any other function that is used for some
        /// kind
        /// of initialization);
        /// - deploy without initial function call;
        /// - signed/unsigned + data for signing.
        /// 
        /// `Signer` defines how the message should or shouldn't be signed:
        /// 
        /// `Signer::None` creates an unsigned message. This may be needed in case of some public methods,
        /// that do not require authorization by pubkey.
        /// 
        /// `Signer::External` takes public key and returns `data_to_sign` for later signing.
        /// Use `attach_signature` method with the result signature to get the signed message.
        /// 
        /// `Signer::Keys` creates a signed message with provided key pair.
        /// 
        /// [SOON] `Signer::SigningBox` Allows using a special interface to implement signing
        /// without private key disclosure to SDK. For instance, in case of using a cold wallet or HSM,
        /// when application calls some API to sign data.
        /// 
        /// There is an optional public key can be provided in deploy set in order to substitute one
        /// in TVM file.
        /// 
        /// Public key resolving priority:
        /// 1. Public key from deploy set.
        /// 2. Public key, specified in TVM file.
        /// 3. Public key, provided by signer.
        /// </summary>
        Task<ResultOfEncodeMessage> EncodeMessageAsync(ParamsOfEncodeMessage @params);

        /// <summary>
        /// Allows to encode deploy and function call messages.
        /// 
        /// Use cases include messages of any possible type:
        /// - deploy with initial function call (i.e. `constructor` or any other function that is used for some
        /// kind
        /// of initialization);
        /// - deploy without initial function call;
        /// - simple function call
        /// 
        /// There is an optional public key can be provided in deploy set in order to substitute one
        /// in TVM file.
        /// 
        /// Public key resolving priority:
        /// 1. Public key from deploy set.
        /// 2. Public key, specified in TVM file.
        /// </summary>
        Task<ResultOfEncodeInternalMessage> EncodeInternalMessageAsync(ParamsOfEncodeInternalMessage @params);

        /// <summary>
        /// Combines `hex`-encoded `signature` with `base64`-encoded `unsigned_message`. Returns signed message
        /// encoded in `base64`.
        /// </summary>
        Task<ResultOfAttachSignature> AttachSignatureAsync(ParamsOfAttachSignature @params);

        /// <summary>
        /// Decodes message body using provided message BOC and ABI.
        /// </summary>
        Task<DecodedMessageBody> DecodeMessageAsync(ParamsOfDecodeMessage @params);

        /// <summary>
        /// Decodes message body using provided body BOC and ABI.
        /// </summary>
        Task<DecodedMessageBody> DecodeMessageBodyAsync(ParamsOfDecodeMessageBody @params);

        /// <summary>
        /// Creates account state provided with one of these sets of data :
        /// 1. BOC of code, BOC of data, BOC of library
        /// 2. TVC (string in `base64`), keys, init params
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

        public async Task<ResultOfEncodeInternalMessage> EncodeInternalMessageAsync(ParamsOfEncodeInternalMessage @params)
        {
            return await _client.CallFunctionAsync<ResultOfEncodeInternalMessage>("abi.encode_internal_message", @params).ConfigureAwait(false);
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

