using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Numerics;
using System.Threading.Tasks;
using TonSdk.Modules;

/*
* TON API version 1.36.0, abi module.
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
        InvalidData = 313,
        EncodeInitialDataFailed = 314,
        InvalidFunctionName = 315,
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
        public uint Key { get; set; }

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

        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }

        [JsonProperty("header", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Header { get; set; }

        [JsonProperty("functions", NullValueHandling = NullValueHandling.Ignore)]
        public AbiFunction[] Functions { get; set; }

        [JsonProperty("events", NullValueHandling = NullValueHandling.Ignore)]
        public AbiEvent[] Events { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public AbiData[] Data { get; set; }

        [JsonProperty("fields", NullValueHandling = NullValueHandling.Ignore)]
        public AbiParam[] Fields { get; set; }
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

        /// <summary>
        /// Since ABI version 2.3 destination address of external inbound message is used in message
        /// body signature calculation. Should be provided when signed external inbound message body is
        /// created. Otherwise can be omitted.
        /// </summary>
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }
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

        /// <summary>
        /// Flag allowing partial BOC decoding when ABI doesn't describe the full body BOC. Controls decoder
        /// behaviour when after decoding all described in ABI params there are some data left in BOC: `true` -
        /// return decoded values `false` - return error of incomplete BOC deserialization (default)
        /// </summary>
        [JsonProperty("allow_partial", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowPartial { get; set; }
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

        /// <summary>
        /// Flag allowing partial BOC decoding when ABI doesn't describe the full body BOC. Controls decoder
        /// behaviour when after decoding all described in ABI params there are some data left in BOC: `true` -
        /// return decoded values `false` - return error of incomplete BOC deserialization (default)
        /// </summary>
        [JsonProperty("allow_partial", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowPartial { get; set; }
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

    public class ParamsOfDecodeAccountData
    {
        /// <summary>
        /// Contract ABI
        /// </summary>
        [JsonProperty("abi", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public Abi Abi { get; set; }

        /// <summary>
        /// Data BOC or BOC handle
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public string Data { get; set; }

        /// <summary>
        /// Flag allowing partial BOC decoding when ABI doesn't describe the full body BOC. Controls decoder
        /// behaviour when after decoding all described in ABI params there are some data left in BOC: `true` -
        /// return decoded values `false` - return error of incomplete BOC deserialization (default)
        /// </summary>
        [JsonProperty("allow_partial", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowPartial { get; set; }
    }

    public class ResultOfDecodeAccountData
    {
        /// <summary>
        /// Decoded data as a JSON structure.
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Data { get; set; }
    }

    public class ParamsOfUpdateInitialData
    {
        /// <summary>
        /// Contract ABI
        /// </summary>
        [JsonProperty("abi", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public Abi Abi { get; set; }

        /// <summary>
        /// Data BOC or BOC handle
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public string Data { get; set; }

        /// <summary>
        /// `abi` parameter should be provided to set initial data
        /// </summary>
        [JsonProperty("initial_data", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken InitialData { get; set; }

        /// <summary>
        /// Initial account owner's public key to set into account data
        /// </summary>
        [JsonProperty("initial_pubkey", NullValueHandling = NullValueHandling.Ignore)]
        public string InitialPubkey { get; set; }

        /// <summary>
        /// Cache type to put the result. The BOC itself returned if no cache type provided.
        /// </summary>
        [JsonProperty("boc_cache", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public BocCacheType BocCache { get; set; }
    }

    public class ResultOfUpdateInitialData
    {
        /// <summary>
        /// Updated data BOC or BOC handle
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public string Data { get; set; }
    }

    public class ParamsOfEncodeInitialData
    {
        /// <summary>
        /// Contract ABI
        /// </summary>
        [JsonProperty("abi", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public Abi Abi { get; set; }

        /// <summary>
        /// `abi` parameter should be provided to set initial data
        /// </summary>
        [JsonProperty("initial_data", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken InitialData { get; set; }

        /// <summary>
        /// Initial account owner's public key to set into account data
        /// </summary>
        [JsonProperty("initial_pubkey", NullValueHandling = NullValueHandling.Ignore)]
        public string InitialPubkey { get; set; }

        /// <summary>
        /// Cache type to put the result. The BOC itself returned if no cache type provided.
        /// </summary>
        [JsonProperty("boc_cache", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public BocCacheType BocCache { get; set; }
    }

    public class ResultOfEncodeInitialData
    {
        /// <summary>
        /// Updated data BOC or BOC handle
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public string Data { get; set; }
    }

    public class ParamsOfDecodeInitialData
    {
        /// <summary>
        /// Initial data is decoded if this parameter is provided
        /// </summary>
        [JsonProperty("abi", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public Abi Abi { get; set; }

        /// <summary>
        /// Data BOC or BOC handle
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public string Data { get; set; }

        /// <summary>
        /// Flag allowing partial BOC decoding when ABI doesn't describe the full body BOC. Controls decoder
        /// behaviour when after decoding all described in ABI params there are some data left in BOC: `true` -
        /// return decoded values `false` - return error of incomplete BOC deserialization (default)
        /// </summary>
        [JsonProperty("allow_partial", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowPartial { get; set; }
    }

    public class ResultOfDecodeInitialData
    {
        /// <summary>
        /// Initial data is decoded if `abi` input parameter is provided
        /// </summary>
        [JsonProperty("initial_data", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken InitialData { get; set; }

        /// <summary>
        /// Initial account owner's public key
        /// </summary>
        [JsonProperty("initial_pubkey", NullValueHandling = NullValueHandling.Ignore)]
        public string InitialPubkey { get; set; }
    }

    public class ParamsOfDecodeBoc
    {
        /// <summary>
        /// Parameters to decode from BOC
        /// </summary>
        [JsonProperty("params", NullValueHandling = NullValueHandling.Ignore)]
        public AbiParam[] Params { get; set; }

        /// <summary>
        /// Data BOC or BOC handle
        /// </summary>
        [JsonProperty("boc", NullValueHandling = NullValueHandling.Ignore)]
        public string Boc { get; set; }

        [JsonProperty("allow_partial", NullValueHandling = NullValueHandling.Ignore)]
        public bool AllowPartial { get; set; }
    }

    public class ResultOfDecodeBoc
    {
        /// <summary>
        /// Decoded data as a JSON structure.
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Data { get; set; }
    }

    public class ParamsOfAbiEncodeBoc
    {
        /// <summary>
        /// Parameters to encode into BOC
        /// </summary>
        [JsonProperty("params", NullValueHandling = NullValueHandling.Ignore)]
        public AbiParam[] Params { get; set; }

        /// <summary>
        /// Parameters and values as a JSON structure
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Data { get; set; }

        /// <summary>
        /// The BOC itself returned if no cache type provided
        /// </summary>
        [JsonProperty("boc_cache", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public BocCacheType BocCache { get; set; }
    }

    public class ResultOfAbiEncodeBoc
    {
        /// <summary>
        /// BOC encoded as base64
        /// </summary>
        [JsonProperty("boc", NullValueHandling = NullValueHandling.Ignore)]
        public string Boc { get; set; }
    }

    public class ParamsOfCalcFunctionId
    {
        /// <summary>
        /// Contract ABI.
        /// </summary>
        [JsonProperty("abi", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public Abi Abi { get; set; }

        /// <summary>
        /// Contract function name
        /// </summary>
        [JsonProperty("function_name", NullValueHandling = NullValueHandling.Ignore)]
        public string FunctionName { get; set; }

        /// <summary>
        /// If set to `true` output function ID will be returned which is used in contract response. Default is
        /// `false`
        /// </summary>
        [JsonProperty("output", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Output { get; set; }
    }

    public class ResultOfCalcFunctionId
    {
        /// <summary>
        /// Contract function ID
        /// </summary>
        [JsonProperty("function_id", NullValueHandling = NullValueHandling.Ignore)]
        public uint FunctionId { get; set; }
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

        /// <summary>
        /// Note: this feature requires ABI 2.1 or higher.
        /// </summary>
        Task<ResultOfDecodeAccountData> DecodeAccountDataAsync(ParamsOfDecodeAccountData @params);

        /// <summary>
        /// Updates initial account data with initial values for the contract's static variables and owner's
        /// public key. This operation is applicable only for initial account data (before deploy). If the
        /// contract is already deployed, its data doesn't contain this data section any more.
        /// </summary>
        Task<ResultOfUpdateInitialData> UpdateInitialDataAsync(ParamsOfUpdateInitialData @params);

        /// <summary>
        /// This function is analogue of `tvm.buildDataInit` function in Solidity.
        /// </summary>
        Task<ResultOfEncodeInitialData> EncodeInitialDataAsync(ParamsOfEncodeInitialData @params);

        /// <summary>
        /// Decodes initial values of a contract's static variables and owner's public key from account initial
        /// data This operation is applicable only for initial account data (before deploy). If the contract is
        /// already deployed, its data doesn't contain this data section any more.
        /// </summary>
        Task<ResultOfDecodeInitialData> DecodeInitialDataAsync(ParamsOfDecodeInitialData @params);

        /// <summary>
        /// Solidity functions use ABI types for [builder
        /// encoding](https://github.com/tonlabs/TON-Solidity-Compiler/blob/master/API.md#tvmbuilderstore).
        /// The simplest way to decode such a BOC is to use ABI decoding.
        /// ABI has it own rules for fields layout in cells so manually encoded
        /// BOC can not be described in terms of ABI rules.
        /// 
        /// To solve this problem we introduce a new ABI type `Ref(<ParamType>)`
        /// which allows to store `ParamType` ABI parameter in cell reference and, thus,
        /// decode manually encoded BOCs. This type is available only in `decode_boc` function
        /// and will not be available in ABI messages encoding until it is included into some ABI revision.
        /// 
        /// Such BOC descriptions covers most users needs. If someone wants to decode some BOC which
        /// can not be described by these rules (i.e. BOC with TLB containing constructors of flags
        /// defining some parsing conditions) then they can decode the fields up to fork condition,
        /// check the parsed data manually, expand the parsing schema and then decode the whole BOC
        /// with the full schema.
        /// </summary>
        Task<ResultOfDecodeBoc> DecodeBocAsync(ParamsOfDecodeBoc @params);

        /// <summary>
        /// Encodes given parameters in JSON into a BOC using param types from ABI.
        /// </summary>
        Task<ResultOfAbiEncodeBoc> EncodeBocAsync(ParamsOfAbiEncodeBoc @params);

        /// <summary>
        /// Calculates contract function ID by contract ABI
        /// </summary>
        Task<ResultOfCalcFunctionId> CalcFunctionIdAsync(ParamsOfCalcFunctionId @params);
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

        public async Task<ResultOfDecodeAccountData> DecodeAccountDataAsync(ParamsOfDecodeAccountData @params)
        {
            return await _client.CallFunctionAsync<ResultOfDecodeAccountData>("abi.decode_account_data", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfUpdateInitialData> UpdateInitialDataAsync(ParamsOfUpdateInitialData @params)
        {
            return await _client.CallFunctionAsync<ResultOfUpdateInitialData>("abi.update_initial_data", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfEncodeInitialData> EncodeInitialDataAsync(ParamsOfEncodeInitialData @params)
        {
            return await _client.CallFunctionAsync<ResultOfEncodeInitialData>("abi.encode_initial_data", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfDecodeInitialData> DecodeInitialDataAsync(ParamsOfDecodeInitialData @params)
        {
            return await _client.CallFunctionAsync<ResultOfDecodeInitialData>("abi.decode_initial_data", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfDecodeBoc> DecodeBocAsync(ParamsOfDecodeBoc @params)
        {
            return await _client.CallFunctionAsync<ResultOfDecodeBoc>("abi.decode_boc", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfAbiEncodeBoc> EncodeBocAsync(ParamsOfAbiEncodeBoc @params)
        {
            return await _client.CallFunctionAsync<ResultOfAbiEncodeBoc>("abi.encode_boc", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfCalcFunctionId> CalcFunctionIdAsync(ParamsOfCalcFunctionId @params)
        {
            return await _client.CallFunctionAsync<ResultOfCalcFunctionId>("abi.calc_function_id", @params).ConfigureAwait(false);
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

