using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Numerics;
using System.Threading.Tasks;
using TonSdk.Modules;

/*
* TON API version 1.37.1, crypto module.
* THIS FILE WAS GENERATED AUTOMATICALLY.
*/

namespace TonSdk.Modules
{
    public enum CryptoErrorCode
    {
        InvalidPublicKey = 100,
        InvalidSecretKey = 101,
        InvalidKey = 102,
        InvalidFactorizeChallenge = 106,
        InvalidBigInt = 107,
        ScryptFailed = 108,
        InvalidKeySize = 109,
        NaclSecretBoxFailed = 110,
        NaclBoxFailed = 111,
        NaclSignFailed = 112,
        Bip39InvalidEntropy = 113,
        Bip39InvalidPhrase = 114,
        Bip32InvalidKey = 115,
        Bip32InvalidDerivePath = 116,
        Bip39InvalidDictionary = 117,
        Bip39InvalidWordCount = 118,
        MnemonicGenerationFailed = 119,
        MnemonicFromEntropyFailed = 120,
        SigningBoxNotRegistered = 121,
        InvalidSignature = 122,
        EncryptionBoxNotRegistered = 123,
        InvalidIvSize = 124,
        UnsupportedCipherMode = 125,
        CannotCreateCipher = 126,
        EncryptDataError = 127,
        DecryptDataError = 128,
        IvRequired = 129,
        CryptoBoxNotRegistered = 130,
        InvalidCryptoBoxType = 131,
        CryptoBoxSecretSerializationError = 132,
        CryptoBoxSecretDeserializationError = 133,
        InvalidNonceSize = 134,
    }

    /// <summary>
    /// Encryption box information.
    /// </summary>
    public class EncryptionBoxInfo
    {
        /// <summary>
        /// Derivation path, for instance "m/44'/396'/0'/0/0"
        /// </summary>
        [JsonProperty("hdpath", NullValueHandling = NullValueHandling.Ignore)]
        public string Hdpath { get; set; }

        /// <summary>
        /// Cryptographic algorithm, used by this encryption box
        /// </summary>
        [JsonProperty("algorithm", NullValueHandling = NullValueHandling.Ignore)]
        public string Algorithm { get; set; }

        /// <summary>
        /// Options, depends on algorithm and specific encryption box implementation
        /// </summary>
        [JsonProperty("options", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Options { get; set; }

        /// <summary>
        /// Public information, depends on algorithm
        /// </summary>
        [JsonProperty("public", NullValueHandling = NullValueHandling.Ignore)]
        public Newtonsoft.Json.Linq.JToken Public { get; set; }
    }

    public abstract class EncryptionAlgorithm
    {
        public class AES : EncryptionAlgorithm
        {
            [JsonProperty("mode", NullValueHandling = NullValueHandling.Ignore)]
            [JsonConverter(typeof(StringEnumConverter))]
            public CipherMode Mode { get; set; }

            [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
            public string Key { get; set; }

            [JsonProperty("iv", NullValueHandling = NullValueHandling.Ignore)]
            public string Iv { get; set; }
        }

        public class ChaCha20 : EncryptionAlgorithm
        {
            /// <summary>
            /// Must be encoded with `hex`.
            /// </summary>
            [JsonProperty("nonce", NullValueHandling = NullValueHandling.Ignore)]
            public string Nonce { get; set; }
        }

        public class NaclBox : EncryptionAlgorithm
        {
            /// <summary>
            /// Must be encoded with `hex`.
            /// </summary>
            [JsonProperty("their_public", NullValueHandling = NullValueHandling.Ignore)]
            public string TheirPublic { get; set; }

            /// <summary>
            /// Must be encoded with `hex`.
            /// </summary>
            [JsonProperty("nonce", NullValueHandling = NullValueHandling.Ignore)]
            public string Nonce { get; set; }
        }

        public class NaclSecretBox : EncryptionAlgorithm
        {
            /// <summary>
            /// Nonce in `hex`
            /// </summary>
            [JsonProperty("nonce", NullValueHandling = NullValueHandling.Ignore)]
            public string Nonce { get; set; }
        }
    }

    public enum CipherMode
    {
        CBC,
        CFB,
        CTR,
        ECB,
        OFB,
    }

    public class AesParamsEB
    {
        [JsonProperty("mode", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public CipherMode Mode { get; set; }

        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public string Key { get; set; }

        [JsonProperty("iv", NullValueHandling = NullValueHandling.Ignore)]
        public string Iv { get; set; }
    }

    public class AesInfo
    {
        [JsonProperty("mode", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public CipherMode Mode { get; set; }

        [JsonProperty("iv", NullValueHandling = NullValueHandling.Ignore)]
        public string Iv { get; set; }
    }

    public class ChaCha20ParamsEB
    {
        /// <summary>
        /// Must be encoded with `hex`.
        /// </summary>
        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public string Key { get; set; }

        /// <summary>
        /// Must be encoded with `hex`.
        /// </summary>
        [JsonProperty("nonce", NullValueHandling = NullValueHandling.Ignore)]
        public string Nonce { get; set; }
    }

    public class NaclBoxParamsEB
    {
        /// <summary>
        /// Must be encoded with `hex`.
        /// </summary>
        [JsonProperty("their_public", NullValueHandling = NullValueHandling.Ignore)]
        public string TheirPublic { get; set; }

        /// <summary>
        /// Must be encoded with `hex`.
        /// </summary>
        [JsonProperty("secret", NullValueHandling = NullValueHandling.Ignore)]
        public string Secret { get; set; }

        /// <summary>
        /// Must be encoded with `hex`.
        /// </summary>
        [JsonProperty("nonce", NullValueHandling = NullValueHandling.Ignore)]
        public string Nonce { get; set; }
    }

    public class NaclSecretBoxParamsEB
    {
        /// <summary>
        /// Secret key - unprefixed 0-padded to 64 symbols hex string
        /// </summary>
        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public string Key { get; set; }

        /// <summary>
        /// Nonce in `hex`
        /// </summary>
        [JsonProperty("nonce", NullValueHandling = NullValueHandling.Ignore)]
        public string Nonce { get; set; }
    }

    /// <summary>
    /// Crypto Box Secret.
    /// </summary>
    public abstract class CryptoBoxSecret
    {
        /// <summary>
        /// This type should be used upon the first wallet initialization, all further initializations
        /// should use `EncryptedSecret` type instead.
        /// 
        /// Get `encrypted_secret` with `get_crypto_box_info` function and store it on your side.
        /// </summary>
        public class RandomSeedPhrase : CryptoBoxSecret
        {
            [JsonProperty("dictionary", NullValueHandling = NullValueHandling.Ignore)]
            public byte Dictionary { get; set; }

            [JsonProperty("wordcount", NullValueHandling = NullValueHandling.Ignore)]
            public byte Wordcount { get; set; }
        }

        /// <summary>
        /// This type should be used only upon the first wallet initialization, all further
        /// initializations should use `EncryptedSecret` type instead.
        /// 
        /// Get `encrypted_secret` with `get_crypto_box_info` function and store it on your side.
        /// </summary>
        public class PredefinedSeedPhrase : CryptoBoxSecret
        {
            [JsonProperty("phrase", NullValueHandling = NullValueHandling.Ignore)]
            public string Phrase { get; set; }

            [JsonProperty("dictionary", NullValueHandling = NullValueHandling.Ignore)]
            public byte Dictionary { get; set; }

            [JsonProperty("wordcount", NullValueHandling = NullValueHandling.Ignore)]
            public byte Wordcount { get; set; }
        }

        /// <summary>
        /// It is an object, containing seed phrase or private key, encrypted with
        /// `secret_encryption_salt` and password from `password_provider`.
        /// 
        /// Note that if you want to change salt or password provider, then you need to reinitialize
        /// the wallet with `PredefinedSeedPhrase`, then get `EncryptedSecret` via `get_crypto_box_info`,
        /// store it somewhere, and only after that initialize the wallet with `EncryptedSecret` type.
        /// </summary>
        public class EncryptedSecret : CryptoBoxSecret
        {
            /// <summary>
            /// It is an object, containing encrypted seed phrase or private key (now we support only seed phrase).
            /// </summary>
            [JsonProperty("encrypted_secret", NullValueHandling = NullValueHandling.Ignore)]
            public string EncryptedSecretProperty { get; set; }
        }
    }

    public abstract class BoxEncryptionAlgorithm
    {
        public class ChaCha20 : BoxEncryptionAlgorithm
        {
            /// <summary>
            /// Must be encoded with `hex`.
            /// </summary>
            [JsonProperty("nonce", NullValueHandling = NullValueHandling.Ignore)]
            public string Nonce { get; set; }
        }

        public class NaclBox : BoxEncryptionAlgorithm
        {
            /// <summary>
            /// Must be encoded with `hex`.
            /// </summary>
            [JsonProperty("their_public", NullValueHandling = NullValueHandling.Ignore)]
            public string TheirPublic { get; set; }

            /// <summary>
            /// Must be encoded with `hex`.
            /// </summary>
            [JsonProperty("nonce", NullValueHandling = NullValueHandling.Ignore)]
            public string Nonce { get; set; }
        }

        public class NaclSecretBox : BoxEncryptionAlgorithm
        {
            /// <summary>
            /// Nonce in `hex`
            /// </summary>
            [JsonProperty("nonce", NullValueHandling = NullValueHandling.Ignore)]
            public string Nonce { get; set; }
        }
    }

    public class ChaCha20ParamsCB
    {
        /// <summary>
        /// Must be encoded with `hex`.
        /// </summary>
        [JsonProperty("nonce", NullValueHandling = NullValueHandling.Ignore)]
        public string Nonce { get; set; }
    }

    public class NaclBoxParamsCB
    {
        /// <summary>
        /// Must be encoded with `hex`.
        /// </summary>
        [JsonProperty("their_public", NullValueHandling = NullValueHandling.Ignore)]
        public string TheirPublic { get; set; }

        /// <summary>
        /// Must be encoded with `hex`.
        /// </summary>
        [JsonProperty("nonce", NullValueHandling = NullValueHandling.Ignore)]
        public string Nonce { get; set; }
    }

    public class NaclSecretBoxParamsCB
    {
        /// <summary>
        /// Nonce in `hex`
        /// </summary>
        [JsonProperty("nonce", NullValueHandling = NullValueHandling.Ignore)]
        public string Nonce { get; set; }
    }

    public class ParamsOfFactorize
    {
        /// <summary>
        /// Hexadecimal representation of u64 composite number.
        /// </summary>
        [JsonProperty("composite", NullValueHandling = NullValueHandling.Ignore)]
        public string Composite { get; set; }
    }

    public class ResultOfFactorize
    {
        /// <summary>
        /// Two factors of composite or empty if composite can't be factorized.
        /// </summary>
        [JsonProperty("factors", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Factors { get; set; }
    }

    public class ParamsOfModularPower
    {
        /// <summary>
        /// `base` argument of calculation.
        /// </summary>
        [JsonProperty("base", NullValueHandling = NullValueHandling.Ignore)]
        public string Base { get; set; }

        /// <summary>
        /// `exponent` argument of calculation.
        /// </summary>
        [JsonProperty("exponent", NullValueHandling = NullValueHandling.Ignore)]
        public string Exponent { get; set; }

        /// <summary>
        /// `modulus` argument of calculation.
        /// </summary>
        [JsonProperty("modulus", NullValueHandling = NullValueHandling.Ignore)]
        public string Modulus { get; set; }
    }

    public class ResultOfModularPower
    {
        /// <summary>
        /// Result of modular exponentiation
        /// </summary>
        [JsonProperty("modular_power", NullValueHandling = NullValueHandling.Ignore)]
        public string ModularPower { get; set; }
    }

    public class ParamsOfTonCrc16
    {
        /// <summary>
        /// Encoded with `base64`.
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public string Data { get; set; }
    }

    public class ResultOfTonCrc16
    {
        /// <summary>
        /// Calculated CRC for input data.
        /// </summary>
        [JsonProperty("crc", NullValueHandling = NullValueHandling.Ignore)]
        public ushort Crc { get; set; }
    }

    public class ParamsOfGenerateRandomBytes
    {
        /// <summary>
        /// Size of random byte array.
        /// </summary>
        [JsonProperty("length", NullValueHandling = NullValueHandling.Ignore)]
        public uint Length { get; set; }
    }

    public class ResultOfGenerateRandomBytes
    {
        /// <summary>
        /// Generated bytes encoded in `base64`.
        /// </summary>
        [JsonProperty("bytes", NullValueHandling = NullValueHandling.Ignore)]
        public string Bytes { get; set; }
    }

    public class ParamsOfConvertPublicKeyToTonSafeFormat
    {
        /// <summary>
        /// Public key - 64 symbols hex string
        /// </summary>
        [JsonProperty("public_key", NullValueHandling = NullValueHandling.Ignore)]
        public string PublicKey { get; set; }
    }

    public class ResultOfConvertPublicKeyToTonSafeFormat
    {
        /// <summary>
        /// Public key represented in TON safe format.
        /// </summary>
        [JsonProperty("ton_public_key", NullValueHandling = NullValueHandling.Ignore)]
        public string TonPublicKey { get; set; }
    }

    public class KeyPair
    {
        /// <summary>
        /// Public key - 64 symbols hex string
        /// </summary>
        [JsonProperty("public", NullValueHandling = NullValueHandling.Ignore)]
        public string Public { get; set; }

        /// <summary>
        /// Private key - u64 symbols hex string
        /// </summary>
        [JsonProperty("secret", NullValueHandling = NullValueHandling.Ignore)]
        public string Secret { get; set; }
    }

    public class ParamsOfSign
    {
        /// <summary>
        /// Data that must be signed encoded in `base64`.
        /// </summary>
        [JsonProperty("unsigned", NullValueHandling = NullValueHandling.Ignore)]
        public string Unsigned { get; set; }

        /// <summary>
        /// Sign keys.
        /// </summary>
        [JsonProperty("keys", NullValueHandling = NullValueHandling.Ignore)]
        public KeyPair Keys { get; set; }
    }

    public class ResultOfSign
    {
        /// <summary>
        /// Signed data combined with signature encoded in `base64`.
        /// </summary>
        [JsonProperty("signed", NullValueHandling = NullValueHandling.Ignore)]
        public string Signed { get; set; }

        /// <summary>
        /// Signature encoded in `hex`.
        /// </summary>
        [JsonProperty("signature", NullValueHandling = NullValueHandling.Ignore)]
        public string Signature { get; set; }
    }

    public class ParamsOfVerifySignature
    {
        /// <summary>
        /// Signed data that must be verified encoded in `base64`.
        /// </summary>
        [JsonProperty("signed", NullValueHandling = NullValueHandling.Ignore)]
        public string Signed { get; set; }

        /// <summary>
        /// Signer's public key - 64 symbols hex string
        /// </summary>
        [JsonProperty("public", NullValueHandling = NullValueHandling.Ignore)]
        public string Public { get; set; }
    }

    public class ResultOfVerifySignature
    {
        /// <summary>
        /// Unsigned data encoded in `base64`.
        /// </summary>
        [JsonProperty("unsigned", NullValueHandling = NullValueHandling.Ignore)]
        public string Unsigned { get; set; }
    }

    public class ParamsOfHash
    {
        /// <summary>
        /// Encoded with `base64`.
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public string Data { get; set; }
    }

    public class ResultOfHash
    {
        /// <summary>
        /// Encoded with 'hex'.
        /// </summary>
        [JsonProperty("hash", NullValueHandling = NullValueHandling.Ignore)]
        public string Hash { get; set; }
    }

    public class ParamsOfScrypt
    {
        /// <summary>
        /// The password bytes to be hashed. Must be encoded with `base64`.
        /// </summary>
        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }

        /// <summary>
        /// Salt bytes that modify the hash to protect against Rainbow table attacks. Must be encoded with
        /// `base64`.
        /// </summary>
        [JsonProperty("salt", NullValueHandling = NullValueHandling.Ignore)]
        public string Salt { get; set; }

        /// <summary>
        /// CPU/memory cost parameter
        /// </summary>
        [JsonProperty("log_n", NullValueHandling = NullValueHandling.Ignore)]
        public byte LogN { get; set; }

        /// <summary>
        /// The block size parameter, which fine-tunes sequential memory read size and performance.
        /// </summary>
        [JsonProperty("r", NullValueHandling = NullValueHandling.Ignore)]
        public uint R { get; set; }

        /// <summary>
        /// Parallelization parameter.
        /// </summary>
        [JsonProperty("p", NullValueHandling = NullValueHandling.Ignore)]
        public uint P { get; set; }

        /// <summary>
        /// Intended output length in octets of the derived key.
        /// </summary>
        [JsonProperty("dk_len", NullValueHandling = NullValueHandling.Ignore)]
        public uint DkLen { get; set; }
    }

    public class ResultOfScrypt
    {
        /// <summary>
        /// Encoded with `hex`.
        /// </summary>
        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public string Key { get; set; }
    }

    public class ParamsOfNaclSignKeyPairFromSecret
    {
        /// <summary>
        /// Secret key - unprefixed 0-padded to 64 symbols hex string
        /// </summary>
        [JsonProperty("secret", NullValueHandling = NullValueHandling.Ignore)]
        public string Secret { get; set; }
    }

    public class ParamsOfNaclSign
    {
        /// <summary>
        /// Data that must be signed encoded in `base64`.
        /// </summary>
        [JsonProperty("unsigned", NullValueHandling = NullValueHandling.Ignore)]
        public string Unsigned { get; set; }

        /// <summary>
        /// Signer's secret key - unprefixed 0-padded to 128 symbols hex string (concatenation of 64 symbols
        /// secret and 64 symbols public keys). See `nacl_sign_keypair_from_secret_key`.
        /// </summary>
        [JsonProperty("secret", NullValueHandling = NullValueHandling.Ignore)]
        public string Secret { get; set; }
    }

    public class ResultOfNaclSign
    {
        /// <summary>
        /// Signed data, encoded in `base64`.
        /// </summary>
        [JsonProperty("signed", NullValueHandling = NullValueHandling.Ignore)]
        public string Signed { get; set; }
    }

    public class ParamsOfNaclSignOpen
    {
        /// <summary>
        /// Encoded with `base64`.
        /// </summary>
        [JsonProperty("signed", NullValueHandling = NullValueHandling.Ignore)]
        public string Signed { get; set; }

        /// <summary>
        /// Signer's public key - unprefixed 0-padded to 64 symbols hex string
        /// </summary>
        [JsonProperty("public", NullValueHandling = NullValueHandling.Ignore)]
        public string Public { get; set; }
    }

    public class ResultOfNaclSignOpen
    {
        /// <summary>
        /// Unsigned data, encoded in `base64`.
        /// </summary>
        [JsonProperty("unsigned", NullValueHandling = NullValueHandling.Ignore)]
        public string Unsigned { get; set; }
    }

    public class ResultOfNaclSignDetached
    {
        /// <summary>
        /// Signature encoded in `hex`.
        /// </summary>
        [JsonProperty("signature", NullValueHandling = NullValueHandling.Ignore)]
        public string Signature { get; set; }
    }

    public class ParamsOfNaclSignDetachedVerify
    {
        /// <summary>
        /// Encoded with `base64`.
        /// </summary>
        [JsonProperty("unsigned", NullValueHandling = NullValueHandling.Ignore)]
        public string Unsigned { get; set; }

        /// <summary>
        /// Encoded with `hex`.
        /// </summary>
        [JsonProperty("signature", NullValueHandling = NullValueHandling.Ignore)]
        public string Signature { get; set; }

        /// <summary>
        /// Signer's public key - unprefixed 0-padded to 64 symbols hex string.
        /// </summary>
        [JsonProperty("public", NullValueHandling = NullValueHandling.Ignore)]
        public string Public { get; set; }
    }

    public class ResultOfNaclSignDetachedVerify
    {
        /// <summary>
        /// `true` if verification succeeded or `false` if it failed
        /// </summary>
        [JsonProperty("succeeded", NullValueHandling = NullValueHandling.Ignore)]
        public bool Succeeded { get; set; }
    }

    public class ParamsOfNaclBoxKeyPairFromSecret
    {
        /// <summary>
        /// Secret key - unprefixed 0-padded to 64 symbols hex string
        /// </summary>
        [JsonProperty("secret", NullValueHandling = NullValueHandling.Ignore)]
        public string Secret { get; set; }
    }

    public class ParamsOfNaclBox
    {
        /// <summary>
        /// Data that must be encrypted encoded in `base64`.
        /// </summary>
        [JsonProperty("decrypted", NullValueHandling = NullValueHandling.Ignore)]
        public string Decrypted { get; set; }

        /// <summary>
        /// Nonce, encoded in `hex`
        /// </summary>
        [JsonProperty("nonce", NullValueHandling = NullValueHandling.Ignore)]
        public string Nonce { get; set; }

        /// <summary>
        /// Receiver's public key - unprefixed 0-padded to 64 symbols hex string
        /// </summary>
        [JsonProperty("their_public", NullValueHandling = NullValueHandling.Ignore)]
        public string TheirPublic { get; set; }

        /// <summary>
        /// Sender's private key - unprefixed 0-padded to 64 symbols hex string
        /// </summary>
        [JsonProperty("secret", NullValueHandling = NullValueHandling.Ignore)]
        public string Secret { get; set; }
    }

    public class ResultOfNaclBox
    {
        /// <summary>
        /// Encrypted data encoded in `base64`.
        /// </summary>
        [JsonProperty("encrypted", NullValueHandling = NullValueHandling.Ignore)]
        public string Encrypted { get; set; }
    }

    public class ParamsOfNaclBoxOpen
    {
        /// <summary>
        /// Encoded with `base64`.
        /// </summary>
        [JsonProperty("encrypted", NullValueHandling = NullValueHandling.Ignore)]
        public string Encrypted { get; set; }

        /// <summary>
        /// Nonce
        /// </summary>
        [JsonProperty("nonce", NullValueHandling = NullValueHandling.Ignore)]
        public string Nonce { get; set; }

        /// <summary>
        /// Sender's public key - unprefixed 0-padded to 64 symbols hex string
        /// </summary>
        [JsonProperty("their_public", NullValueHandling = NullValueHandling.Ignore)]
        public string TheirPublic { get; set; }

        /// <summary>
        /// Receiver's private key - unprefixed 0-padded to 64 symbols hex string
        /// </summary>
        [JsonProperty("secret", NullValueHandling = NullValueHandling.Ignore)]
        public string Secret { get; set; }
    }

    public class ResultOfNaclBoxOpen
    {
        /// <summary>
        /// Decrypted data encoded in `base64`.
        /// </summary>
        [JsonProperty("decrypted", NullValueHandling = NullValueHandling.Ignore)]
        public string Decrypted { get; set; }
    }

    public class ParamsOfNaclSecretBox
    {
        /// <summary>
        /// Encoded with `base64`.
        /// </summary>
        [JsonProperty("decrypted", NullValueHandling = NullValueHandling.Ignore)]
        public string Decrypted { get; set; }

        /// <summary>
        /// Nonce in `hex`
        /// </summary>
        [JsonProperty("nonce", NullValueHandling = NullValueHandling.Ignore)]
        public string Nonce { get; set; }

        /// <summary>
        /// Secret key - unprefixed 0-padded to 64 symbols hex string
        /// </summary>
        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public string Key { get; set; }
    }

    public class ParamsOfNaclSecretBoxOpen
    {
        /// <summary>
        /// Encoded with `base64`.
        /// </summary>
        [JsonProperty("encrypted", NullValueHandling = NullValueHandling.Ignore)]
        public string Encrypted { get; set; }

        /// <summary>
        /// Nonce in `hex`
        /// </summary>
        [JsonProperty("nonce", NullValueHandling = NullValueHandling.Ignore)]
        public string Nonce { get; set; }

        /// <summary>
        /// Secret key - unprefixed 0-padded to 64 symbols hex string
        /// </summary>
        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public string Key { get; set; }
    }

    public class ParamsOfMnemonicWords
    {
        /// <summary>
        /// Dictionary identifier
        /// </summary>
        [JsonProperty("dictionary", NullValueHandling = NullValueHandling.Ignore)]
        public byte? Dictionary { get; set; }
    }

    public class ResultOfMnemonicWords
    {
        /// <summary>
        /// The list of mnemonic words
        /// </summary>
        [JsonProperty("words", NullValueHandling = NullValueHandling.Ignore)]
        public string Words { get; set; }
    }

    public class ParamsOfMnemonicFromRandom
    {
        /// <summary>
        /// Dictionary identifier
        /// </summary>
        [JsonProperty("dictionary", NullValueHandling = NullValueHandling.Ignore)]
        public byte? Dictionary { get; set; }

        /// <summary>
        /// Mnemonic word count
        /// </summary>
        [JsonProperty("word_count", NullValueHandling = NullValueHandling.Ignore)]
        public byte? WordCount { get; set; }
    }

    public class ResultOfMnemonicFromRandom
    {
        /// <summary>
        /// String of mnemonic words
        /// </summary>
        [JsonProperty("phrase", NullValueHandling = NullValueHandling.Ignore)]
        public string Phrase { get; set; }
    }

    public class ParamsOfMnemonicFromEntropy
    {
        /// <summary>
        /// Hex encoded.
        /// </summary>
        [JsonProperty("entropy", NullValueHandling = NullValueHandling.Ignore)]
        public string Entropy { get; set; }

        /// <summary>
        /// Dictionary identifier
        /// </summary>
        [JsonProperty("dictionary", NullValueHandling = NullValueHandling.Ignore)]
        public byte? Dictionary { get; set; }

        /// <summary>
        /// Mnemonic word count
        /// </summary>
        [JsonProperty("word_count", NullValueHandling = NullValueHandling.Ignore)]
        public byte? WordCount { get; set; }
    }

    public class ResultOfMnemonicFromEntropy
    {
        /// <summary>
        /// Phrase
        /// </summary>
        [JsonProperty("phrase", NullValueHandling = NullValueHandling.Ignore)]
        public string Phrase { get; set; }
    }

    public class ParamsOfMnemonicVerify
    {
        /// <summary>
        /// Phrase
        /// </summary>
        [JsonProperty("phrase", NullValueHandling = NullValueHandling.Ignore)]
        public string Phrase { get; set; }

        /// <summary>
        /// Dictionary identifier
        /// </summary>
        [JsonProperty("dictionary", NullValueHandling = NullValueHandling.Ignore)]
        public byte? Dictionary { get; set; }

        /// <summary>
        /// Word count
        /// </summary>
        [JsonProperty("word_count", NullValueHandling = NullValueHandling.Ignore)]
        public byte? WordCount { get; set; }
    }

    public class ResultOfMnemonicVerify
    {
        /// <summary>
        /// Flag indicating if the mnemonic is valid or not
        /// </summary>
        [JsonProperty("valid", NullValueHandling = NullValueHandling.Ignore)]
        public bool Valid { get; set; }
    }

    public class ParamsOfMnemonicDeriveSignKeys
    {
        /// <summary>
        /// Phrase
        /// </summary>
        [JsonProperty("phrase", NullValueHandling = NullValueHandling.Ignore)]
        public string Phrase { get; set; }

        /// <summary>
        /// Derivation path, for instance "m/44'/396'/0'/0/0"
        /// </summary>
        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string Path { get; set; }

        /// <summary>
        /// Dictionary identifier
        /// </summary>
        [JsonProperty("dictionary", NullValueHandling = NullValueHandling.Ignore)]
        public byte? Dictionary { get; set; }

        /// <summary>
        /// Word count
        /// </summary>
        [JsonProperty("word_count", NullValueHandling = NullValueHandling.Ignore)]
        public byte? WordCount { get; set; }
    }

    public class ParamsOfHDKeyXPrvFromMnemonic
    {
        /// <summary>
        /// String with seed phrase
        /// </summary>
        [JsonProperty("phrase", NullValueHandling = NullValueHandling.Ignore)]
        public string Phrase { get; set; }

        /// <summary>
        /// Dictionary identifier
        /// </summary>
        [JsonProperty("dictionary", NullValueHandling = NullValueHandling.Ignore)]
        public byte? Dictionary { get; set; }

        /// <summary>
        /// Mnemonic word count
        /// </summary>
        [JsonProperty("word_count", NullValueHandling = NullValueHandling.Ignore)]
        public byte? WordCount { get; set; }
    }

    public class ResultOfHDKeyXPrvFromMnemonic
    {
        /// <summary>
        /// Serialized extended master private key
        /// </summary>
        [JsonProperty("xprv", NullValueHandling = NullValueHandling.Ignore)]
        public string Xprv { get; set; }
    }

    public class ParamsOfHDKeyDeriveFromXPrv
    {
        /// <summary>
        /// Serialized extended private key
        /// </summary>
        [JsonProperty("xprv", NullValueHandling = NullValueHandling.Ignore)]
        public string Xprv { get; set; }

        /// <summary>
        /// Child index (see BIP-0032)
        /// </summary>
        [JsonProperty("child_index", NullValueHandling = NullValueHandling.Ignore)]
        public uint ChildIndex { get; set; }

        /// <summary>
        /// Indicates the derivation of hardened/not-hardened key (see BIP-0032)
        /// </summary>
        [JsonProperty("hardened", NullValueHandling = NullValueHandling.Ignore)]
        public bool Hardened { get; set; }
    }

    public class ResultOfHDKeyDeriveFromXPrv
    {
        /// <summary>
        /// Serialized extended private key
        /// </summary>
        [JsonProperty("xprv", NullValueHandling = NullValueHandling.Ignore)]
        public string Xprv { get; set; }
    }

    public class ParamsOfHDKeyDeriveFromXPrvPath
    {
        /// <summary>
        /// Serialized extended private key
        /// </summary>
        [JsonProperty("xprv", NullValueHandling = NullValueHandling.Ignore)]
        public string Xprv { get; set; }

        /// <summary>
        /// Derivation path, for instance "m/44'/396'/0'/0/0"
        /// </summary>
        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string Path { get; set; }
    }

    public class ResultOfHDKeyDeriveFromXPrvPath
    {
        /// <summary>
        /// Derived serialized extended private key
        /// </summary>
        [JsonProperty("xprv", NullValueHandling = NullValueHandling.Ignore)]
        public string Xprv { get; set; }
    }

    public class ParamsOfHDKeySecretFromXPrv
    {
        /// <summary>
        /// Serialized extended private key
        /// </summary>
        [JsonProperty("xprv", NullValueHandling = NullValueHandling.Ignore)]
        public string Xprv { get; set; }
    }

    public class ResultOfHDKeySecretFromXPrv
    {
        /// <summary>
        /// Private key - 64 symbols hex string
        /// </summary>
        [JsonProperty("secret", NullValueHandling = NullValueHandling.Ignore)]
        public string Secret { get; set; }
    }

    public class ParamsOfHDKeyPublicFromXPrv
    {
        /// <summary>
        /// Serialized extended private key
        /// </summary>
        [JsonProperty("xprv", NullValueHandling = NullValueHandling.Ignore)]
        public string Xprv { get; set; }
    }

    public class ResultOfHDKeyPublicFromXPrv
    {
        /// <summary>
        /// Public key - 64 symbols hex string
        /// </summary>
        [JsonProperty("public", NullValueHandling = NullValueHandling.Ignore)]
        public string Public { get; set; }
    }

    public class ParamsOfChaCha20
    {
        /// <summary>
        /// Must be encoded with `base64`.
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public string Data { get; set; }

        /// <summary>
        /// Must be encoded with `hex`.
        /// </summary>
        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public string Key { get; set; }

        /// <summary>
        /// Must be encoded with `hex`.
        /// </summary>
        [JsonProperty("nonce", NullValueHandling = NullValueHandling.Ignore)]
        public string Nonce { get; set; }
    }

    public class ResultOfChaCha20
    {
        /// <summary>
        /// Encoded with `base64`.
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public string Data { get; set; }
    }

    public class ParamsOfCreateCryptoBox
    {
        /// <summary>
        /// Salt used for secret encryption. For example, a mobile device can use device ID as salt.
        /// </summary>
        [JsonProperty("secret_encryption_salt", NullValueHandling = NullValueHandling.Ignore)]
        public string SecretEncryptionSalt { get; set; }

        /// <summary>
        /// Cryptobox secret
        /// </summary>
        [JsonProperty("secret", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public CryptoBoxSecret Secret { get; set; }
    }

    public class RegisteredCryptoBox
    {
        [JsonProperty("handle", NullValueHandling = NullValueHandling.Ignore)]
        public uint Handle { get; set; }
    }

    /// <summary>
    /// To secure the password while passing it from application to the library,
    /// the library generates a temporary key pair, passes the pubkey
    /// to the passwordProvider, decrypts the received password with private key,
    /// and deletes the key pair right away.
    /// 
    /// Application should generate a temporary nacl_box_keypair
    /// and encrypt the password with naclbox function using nacl_box_keypair.secret
    /// and encryption_public_key keys + nonce = 24-byte prefix of encryption_public_key.
    /// </summary>
    public abstract class ParamsOfAppPasswordProvider
    {
        public class GetPassword : ParamsOfAppPasswordProvider
        {
            /// <summary>
            /// Temporary library pubkey, that is used on application side for password encryption, along with
            /// application temporary private key and nonce. Used for password decryption on library side.
            /// </summary>
            [JsonProperty("encryption_public_key", NullValueHandling = NullValueHandling.Ignore)]
            public string EncryptionPublicKey { get; set; }
        }
    }

    public abstract class ResultOfAppPasswordProvider
    {
        public class GetPassword : ResultOfAppPasswordProvider
        {
            /// <summary>
            /// Password, encrypted and encoded to base64. Crypto box uses this password to decrypt its secret (seed
            /// phrase).
            /// </summary>
            [JsonProperty("encrypted_password", NullValueHandling = NullValueHandling.Ignore)]
            public string EncryptedPassword { get; set; }

            /// <summary>
            /// Used together with `encryption_public_key` to decode `encrypted_password`.
            /// </summary>
            [JsonProperty("app_encryption_pubkey", NullValueHandling = NullValueHandling.Ignore)]
            public string AppEncryptionPubkey { get; set; }
        }
    }

    public class ResultOfGetCryptoBoxInfo
    {
        /// <summary>
        /// Secret (seed phrase) encrypted with salt and password.
        /// </summary>
        [JsonProperty("encrypted_secret", NullValueHandling = NullValueHandling.Ignore)]
        public string EncryptedSecret { get; set; }
    }

    public class ResultOfGetCryptoBoxSeedPhrase
    {
        [JsonProperty("phrase", NullValueHandling = NullValueHandling.Ignore)]
        public string Phrase { get; set; }

        [JsonProperty("dictionary", NullValueHandling = NullValueHandling.Ignore)]
        public byte Dictionary { get; set; }

        [JsonProperty("wordcount", NullValueHandling = NullValueHandling.Ignore)]
        public byte Wordcount { get; set; }
    }

    public class ParamsOfGetSigningBoxFromCryptoBox
    {
        /// <summary>
        /// Crypto Box Handle.
        /// </summary>
        [JsonProperty("handle", NullValueHandling = NullValueHandling.Ignore)]
        public uint Handle { get; set; }

        /// <summary>
        /// By default, Everscale HD path is used.
        /// </summary>
        [JsonProperty("hdpath", NullValueHandling = NullValueHandling.Ignore)]
        public string Hdpath { get; set; }

        /// <summary>
        /// Store derived secret for this lifetime (in ms). The timer starts after each signing box operation.
        /// Secrets will be deleted immediately after each signing box operation, if this value is not set.
        /// </summary>
        [JsonProperty("secret_lifetime", NullValueHandling = NullValueHandling.Ignore)]
        public uint? SecretLifetime { get; set; }
    }

    public class RegisteredSigningBox
    {
        /// <summary>
        /// Handle of the signing box.
        /// </summary>
        [JsonProperty("handle", NullValueHandling = NullValueHandling.Ignore)]
        public uint Handle { get; set; }
    }

    public class ParamsOfGetEncryptionBoxFromCryptoBox
    {
        /// <summary>
        /// Crypto Box Handle.
        /// </summary>
        [JsonProperty("handle", NullValueHandling = NullValueHandling.Ignore)]
        public uint Handle { get; set; }

        /// <summary>
        /// By default, Everscale HD path is used.
        /// </summary>
        [JsonProperty("hdpath", NullValueHandling = NullValueHandling.Ignore)]
        public string Hdpath { get; set; }

        /// <summary>
        /// Encryption algorithm.
        /// </summary>
        [JsonProperty("algorithm", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public BoxEncryptionAlgorithm Algorithm { get; set; }

        /// <summary>
        /// Store derived secret for encryption algorithm for this lifetime (in ms). The timer starts after each
        /// encryption box operation. Secrets will be deleted (overwritten with zeroes) after each encryption
        /// operation, if this value is not set.
        /// </summary>
        [JsonProperty("secret_lifetime", NullValueHandling = NullValueHandling.Ignore)]
        public uint? SecretLifetime { get; set; }
    }

    public class RegisteredEncryptionBox
    {
        /// <summary>
        /// Handle of the encryption box.
        /// </summary>
        [JsonProperty("handle", NullValueHandling = NullValueHandling.Ignore)]
        public uint Handle { get; set; }
    }

    /// <summary>
    /// Signing box callbacks.
    /// </summary>
    public abstract class ParamsOfAppSigningBox
    {
        /// <summary>
        /// Get signing box public key
        /// </summary>
        public class GetPublicKey : ParamsOfAppSigningBox
        {
        }

        /// <summary>
        /// Sign data
        /// </summary>
        public class Sign : ParamsOfAppSigningBox
        {
            /// <summary>
            /// Data to sign encoded as base64
            /// </summary>
            [JsonProperty("unsigned", NullValueHandling = NullValueHandling.Ignore)]
            public string Unsigned { get; set; }
        }
    }

    /// <summary>
    /// Returning values from signing box callbacks.
    /// </summary>
    public abstract class ResultOfAppSigningBox
    {
        /// <summary>
        /// Result of getting public key
        /// </summary>
        public class GetPublicKey : ResultOfAppSigningBox
        {
            /// <summary>
            /// Signing box public key
            /// </summary>
            [JsonProperty("public_key", NullValueHandling = NullValueHandling.Ignore)]
            public string PublicKey { get; set; }
        }

        /// <summary>
        /// Result of signing data
        /// </summary>
        public class Sign : ResultOfAppSigningBox
        {
            /// <summary>
            /// Data signature encoded as hex
            /// </summary>
            [JsonProperty("signature", NullValueHandling = NullValueHandling.Ignore)]
            public string Signature { get; set; }
        }
    }

    public class ResultOfSigningBoxGetPublicKey
    {
        /// <summary>
        /// Encoded with hex
        /// </summary>
        [JsonProperty("pubkey", NullValueHandling = NullValueHandling.Ignore)]
        public string Pubkey { get; set; }
    }

    public class ParamsOfSigningBoxSign
    {
        /// <summary>
        /// Signing Box handle.
        /// </summary>
        [JsonProperty("signing_box", NullValueHandling = NullValueHandling.Ignore)]
        public uint SigningBox { get; set; }

        /// <summary>
        /// Must be encoded with `base64`.
        /// </summary>
        [JsonProperty("unsigned", NullValueHandling = NullValueHandling.Ignore)]
        public string Unsigned { get; set; }
    }

    public class ResultOfSigningBoxSign
    {
        /// <summary>
        /// Encoded with `hex`.
        /// </summary>
        [JsonProperty("signature", NullValueHandling = NullValueHandling.Ignore)]
        public string Signature { get; set; }
    }

    /// <summary>
    /// Interface for data encryption/decryption
    /// </summary>
    public abstract class ParamsOfAppEncryptionBox
    {
        /// <summary>
        /// Get encryption box info
        /// </summary>
        public class GetInfo : ParamsOfAppEncryptionBox
        {
        }

        /// <summary>
        /// Encrypt data
        /// </summary>
        public class Encrypt : ParamsOfAppEncryptionBox
        {
            /// <summary>
            /// Data, encoded in Base64
            /// </summary>
            [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
            public string Data { get; set; }
        }

        /// <summary>
        /// Decrypt data
        /// </summary>
        public class Decrypt : ParamsOfAppEncryptionBox
        {
            /// <summary>
            /// Data, encoded in Base64
            /// </summary>
            [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
            public string Data { get; set; }
        }
    }

    /// <summary>
    /// Returning values from signing box callbacks.
    /// </summary>
    public abstract class ResultOfAppEncryptionBox
    {
        /// <summary>
        /// Result of getting encryption box info
        /// </summary>
        public class GetInfo : ResultOfAppEncryptionBox
        {
            [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore)]
            public EncryptionBoxInfo Info { get; set; }
        }

        /// <summary>
        /// Result of encrypting data
        /// </summary>
        public class Encrypt : ResultOfAppEncryptionBox
        {
            /// <summary>
            /// Encrypted data, encoded in Base64
            /// </summary>
            [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
            public string Data { get; set; }
        }

        /// <summary>
        /// Result of decrypting data
        /// </summary>
        public class Decrypt : ResultOfAppEncryptionBox
        {
            /// <summary>
            /// Decrypted data, encoded in Base64
            /// </summary>
            [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
            public string Data { get; set; }
        }
    }

    public class ParamsOfEncryptionBoxGetInfo
    {
        /// <summary>
        /// Encryption box handle
        /// </summary>
        [JsonProperty("encryption_box", NullValueHandling = NullValueHandling.Ignore)]
        public uint EncryptionBox { get; set; }
    }

    public class ResultOfEncryptionBoxGetInfo
    {
        /// <summary>
        /// Encryption box information
        /// </summary>
        [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore)]
        public EncryptionBoxInfo Info { get; set; }
    }

    public class ParamsOfEncryptionBoxEncrypt
    {
        /// <summary>
        /// Encryption box handle
        /// </summary>
        [JsonProperty("encryption_box", NullValueHandling = NullValueHandling.Ignore)]
        public uint EncryptionBox { get; set; }

        /// <summary>
        /// Data to be encrypted, encoded in Base64
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public string Data { get; set; }
    }

    public class ResultOfEncryptionBoxEncrypt
    {
        /// <summary>
        /// Padded to cipher block size
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public string Data { get; set; }
    }

    public class ParamsOfEncryptionBoxDecrypt
    {
        /// <summary>
        /// Encryption box handle
        /// </summary>
        [JsonProperty("encryption_box", NullValueHandling = NullValueHandling.Ignore)]
        public uint EncryptionBox { get; set; }

        /// <summary>
        /// Data to be decrypted, encoded in Base64
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public string Data { get; set; }
    }

    public class ResultOfEncryptionBoxDecrypt
    {
        /// <summary>
        /// Decrypted data, encoded in Base64.
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public string Data { get; set; }
    }

    public class ParamsOfCreateEncryptionBox
    {
        /// <summary>
        /// Encryption algorithm specifier including cipher parameters (key, IV, etc)
        /// </summary>
        [JsonProperty("algorithm", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PolymorphicTypeConverter))]
        public EncryptionAlgorithm Algorithm { get; set; }
    }

    /// <summary>
    /// Crypto functions.
    /// </summary>
    public interface ICryptoModule
    {
        /// <summary>
        /// Performs prime factorization – decomposition of a composite number
        /// into a product of smaller prime integers (factors).
        /// See [https://en.wikipedia.org/wiki/Integer_factorization]
        /// </summary>
        Task<ResultOfFactorize> FactorizeAsync(ParamsOfFactorize @params);

        /// <summary>
        /// Performs modular exponentiation for big integers (`base`^`exponent` mod `modulus`).
        /// See [https://en.wikipedia.org/wiki/Modular_exponentiation]
        /// </summary>
        Task<ResultOfModularPower> ModularPowerAsync(ParamsOfModularPower @params);

        /// <summary>
        /// Calculates CRC16 using TON algorithm.
        /// </summary>
        Task<ResultOfTonCrc16> TonCrc16Async(ParamsOfTonCrc16 @params);

        /// <summary>
        /// Generates random byte array of the specified length and returns it in `base64` format
        /// </summary>
        Task<ResultOfGenerateRandomBytes> GenerateRandomBytesAsync(ParamsOfGenerateRandomBytes @params);

        /// <summary>
        /// Converts public key to ton safe_format
        /// </summary>
        Task<ResultOfConvertPublicKeyToTonSafeFormat> ConvertPublicKeyToTonSafeFormatAsync(ParamsOfConvertPublicKeyToTonSafeFormat @params);

        /// <summary>
        /// Generates random ed25519 key pair.
        /// </summary>
        Task<KeyPair> GenerateRandomSignKeysAsync();

        /// <summary>
        /// Signs a data using the provided keys.
        /// </summary>
        Task<ResultOfSign> SignAsync(ParamsOfSign @params);

        /// <summary>
        /// Verifies signed data using the provided public key. Raises error if verification is failed.
        /// </summary>
        Task<ResultOfVerifySignature> VerifySignatureAsync(ParamsOfVerifySignature @params);

        /// <summary>
        /// Calculates SHA256 hash of the specified data.
        /// </summary>
        Task<ResultOfHash> Sha256Async(ParamsOfHash @params);

        /// <summary>
        /// Calculates SHA512 hash of the specified data.
        /// </summary>
        Task<ResultOfHash> Sha512Async(ParamsOfHash @params);

        /// <summary>
        /// Derives key from `password` and `key` using `scrypt` algorithm.
        /// See [https://en.wikipedia.org/wiki/Scrypt].
        /// 
        /// # Arguments
        /// - `log_n` - The log2 of the Scrypt parameter `N`
        /// - `r` - The Scrypt parameter `r`
        /// - `p` - The Scrypt parameter `p`
        /// # Conditions
        /// - `log_n` must be less than `64`
        /// - `r` must be greater than `0` and less than or equal to `4294967295`
        /// - `p` must be greater than `0` and less than `4294967295`
        /// # Recommended values sufficient for most use-cases
        /// - `log_n = 15` (`n = 32768`)
        /// - `r = 8`
        /// - `p = 1`
        /// </summary>
        Task<ResultOfScrypt> ScryptAsync(ParamsOfScrypt @params);

        /// <summary>
        /// **NOTE:** In the result the secret key is actually the concatenation
        /// of secret and public keys (128 symbols hex string) by design of
        /// [NaCL](http://nacl.cr.yp.to/sign.html).
        /// See also [the stackexchange question](https://crypto.stackexchange.com/questions/54353/).
        /// </summary>
        Task<KeyPair> NaclSignKeypairFromSecretKeyAsync(ParamsOfNaclSignKeyPairFromSecret @params);

        /// <summary>
        /// Signs data using the signer's secret key.
        /// </summary>
        Task<ResultOfNaclSign> NaclSignAsync(ParamsOfNaclSign @params);

        /// <summary>
        /// Verifies the signature in `signed` using the signer's public key `public`
        /// and returns the message `unsigned`.
        /// 
        /// If the signature fails verification, crypto_sign_open raises an exception.
        /// </summary>
        Task<ResultOfNaclSignOpen> NaclSignOpenAsync(ParamsOfNaclSignOpen @params);

        /// <summary>
        /// Signs the message `unsigned` using the secret key `secret`
        /// and returns a signature `signature`.
        /// </summary>
        Task<ResultOfNaclSignDetached> NaclSignDetachedAsync(ParamsOfNaclSign @params);

        /// <summary>
        /// Verifies the signature with public key and `unsigned` data.
        /// </summary>
        Task<ResultOfNaclSignDetachedVerify> NaclSignDetachedVerifyAsync(ParamsOfNaclSignDetachedVerify @params);

        /// <summary>
        /// Generates a random NaCl key pair
        /// </summary>
        Task<KeyPair> NaclBoxKeypairAsync();

        /// <summary>
        /// Generates key pair from a secret key
        /// </summary>
        Task<KeyPair> NaclBoxKeypairFromSecretKeyAsync(ParamsOfNaclBoxKeyPairFromSecret @params);

        /// <summary>
        /// Encrypt and authenticate a message using the senders secret key, the receivers public
        /// key, and a nonce.
        /// </summary>
        Task<ResultOfNaclBox> NaclBoxAsync(ParamsOfNaclBox @params);

        /// <summary>
        /// Decrypt and verify the cipher text using the receivers secret key, the senders public key, and the
        /// nonce.
        /// </summary>
        Task<ResultOfNaclBoxOpen> NaclBoxOpenAsync(ParamsOfNaclBoxOpen @params);

        /// <summary>
        /// Encrypt and authenticate message using nonce and secret key.
        /// </summary>
        Task<ResultOfNaclBox> NaclSecretBoxAsync(ParamsOfNaclSecretBox @params);

        /// <summary>
        /// Decrypts and verifies cipher text using `nonce` and secret `key`.
        /// </summary>
        Task<ResultOfNaclBoxOpen> NaclSecretBoxOpenAsync(ParamsOfNaclSecretBoxOpen @params);

        /// <summary>
        /// Prints the list of words from the specified dictionary
        /// </summary>
        Task<ResultOfMnemonicWords> MnemonicWordsAsync(ParamsOfMnemonicWords @params);

        /// <summary>
        /// Generates a random mnemonic from the specified dictionary and word count
        /// </summary>
        Task<ResultOfMnemonicFromRandom> MnemonicFromRandomAsync(ParamsOfMnemonicFromRandom @params);

        /// <summary>
        /// Generates mnemonic from pre-generated entropy
        /// </summary>
        Task<ResultOfMnemonicFromEntropy> MnemonicFromEntropyAsync(ParamsOfMnemonicFromEntropy @params);

        /// <summary>
        /// The phrase supplied will be checked for word length and validated according to the checksum
        /// specified in BIP0039.
        /// </summary>
        Task<ResultOfMnemonicVerify> MnemonicVerifyAsync(ParamsOfMnemonicVerify @params);

        /// <summary>
        /// Validates the seed phrase, generates master key and then derives
        /// the key pair from the master key and the specified path
        /// </summary>
        Task<KeyPair> MnemonicDeriveSignKeysAsync(ParamsOfMnemonicDeriveSignKeys @params);

        /// <summary>
        /// Generates an extended master private key that will be the root for all the derived keys
        /// </summary>
        Task<ResultOfHDKeyXPrvFromMnemonic> HdkeyXprvFromMnemonicAsync(ParamsOfHDKeyXPrvFromMnemonic @params);

        /// <summary>
        /// Returns extended private key derived from the specified extended private key and child index
        /// </summary>
        Task<ResultOfHDKeyDeriveFromXPrv> HdkeyDeriveFromXprvAsync(ParamsOfHDKeyDeriveFromXPrv @params);

        /// <summary>
        /// Derives the extended private key from the specified key and path
        /// </summary>
        Task<ResultOfHDKeyDeriveFromXPrvPath> HdkeyDeriveFromXprvPathAsync(ParamsOfHDKeyDeriveFromXPrvPath @params);

        /// <summary>
        /// Extracts the private key from the serialized extended private key
        /// </summary>
        Task<ResultOfHDKeySecretFromXPrv> HdkeySecretFromXprvAsync(ParamsOfHDKeySecretFromXPrv @params);

        /// <summary>
        /// Extracts the public key from the serialized extended private key
        /// </summary>
        Task<ResultOfHDKeyPublicFromXPrv> HdkeyPublicFromXprvAsync(ParamsOfHDKeyPublicFromXPrv @params);

        /// <summary>
        /// Performs symmetric `chacha20` encryption.
        /// </summary>
        Task<ResultOfChaCha20> Chacha20Async(ParamsOfChaCha20 @params);

        /// <summary>
        /// Crypto Box is a root crypto object, that encapsulates some secret (seed phrase usually)
        /// in encrypted form and acts as a factory for all crypto primitives used in SDK:
        /// keys for signing and encryption, derived from this secret.
        /// 
        /// Crypto Box encrypts original Seed Phrase with salt and password that is retrieved
        /// from `password_provider` callback, implemented on Application side.
        /// 
        /// When used, decrypted secret shows up in core library's memory for a very short period
        /// of time and then is immediately overwritten with zeroes.
        /// </summary>
        Task<RegisteredCryptoBox> CreateCryptoBoxAsync(ParamsOfCreateCryptoBox @params, Func<ParamsOfAppPasswordProvider, Task<ResultOfAppPasswordProvider>> password_provider);

        /// <summary>
        /// Removes Crypto Box. Clears all secret data.
        /// </summary>
        Task RemoveCryptoBoxAsync(RegisteredCryptoBox @params);

        /// <summary>
        /// Get Crypto Box Info. Used to get `encrypted_secret` that should be used for all the cryptobox
        /// initializations except the first one.
        /// </summary>
        Task<ResultOfGetCryptoBoxInfo> GetCryptoBoxInfoAsync(RegisteredCryptoBox @params);

        /// <summary>
        /// Attention! Store this data in your application for a very short period of time and overwrite it with
        /// zeroes ASAP.
        /// </summary>
        Task<ResultOfGetCryptoBoxSeedPhrase> GetCryptoBoxSeedPhraseAsync(RegisteredCryptoBox @params);

        /// <summary>
        /// Get handle of Signing Box derived from Crypto Box.
        /// </summary>
        Task<RegisteredSigningBox> GetSigningBoxFromCryptoBoxAsync(ParamsOfGetSigningBoxFromCryptoBox @params);

        /// <summary>
        /// Derives encryption keypair from cryptobox secret and hdpath and
        /// stores it in cache for `secret_lifetime`
        /// or until explicitly cleared by `clear_crypto_box_secret_cache` method.
        /// If `secret_lifetime` is not specified - overwrites encryption secret with zeroes immediately after
        /// encryption operation.
        /// </summary>
        Task<RegisteredEncryptionBox> GetEncryptionBoxFromCryptoBoxAsync(ParamsOfGetEncryptionBoxFromCryptoBox @params);

        /// <summary>
        /// Removes cached secrets (overwrites with zeroes) from all signing and encryption boxes, derived from
        /// crypto box.
        /// </summary>
        Task ClearCryptoBoxSecretCacheAsync(RegisteredCryptoBox @params);

        /// <summary>
        /// Register an application implemented signing box.
        /// </summary>
        Task<RegisteredSigningBox> RegisterSigningBoxAsync(Func<ParamsOfAppSigningBox, Task<ResultOfAppSigningBox>> app_object);

        /// <summary>
        /// Creates a default signing box implementation.
        /// </summary>
        Task<RegisteredSigningBox> GetSigningBoxAsync(KeyPair @params);

        /// <summary>
        /// Returns public key of signing key pair.
        /// </summary>
        Task<ResultOfSigningBoxGetPublicKey> SigningBoxGetPublicKeyAsync(RegisteredSigningBox @params);

        /// <summary>
        /// Returns signed user data.
        /// </summary>
        Task<ResultOfSigningBoxSign> SigningBoxSignAsync(ParamsOfSigningBoxSign @params);

        /// <summary>
        /// Removes signing box from SDK.
        /// </summary>
        Task RemoveSigningBoxAsync(RegisteredSigningBox @params);

        /// <summary>
        /// Register an application implemented encryption box.
        /// </summary>
        Task<RegisteredEncryptionBox> RegisterEncryptionBoxAsync(Func<ParamsOfAppEncryptionBox, Task<ResultOfAppEncryptionBox>> app_object);

        /// <summary>
        /// Removes encryption box from SDK
        /// </summary>
        Task RemoveEncryptionBoxAsync(RegisteredEncryptionBox @params);

        /// <summary>
        /// Queries info from the given encryption box
        /// </summary>
        Task<ResultOfEncryptionBoxGetInfo> EncryptionBoxGetInfoAsync(ParamsOfEncryptionBoxGetInfo @params);

        /// <summary>
        /// Block cipher algorithms pad data to cipher block size so encrypted data can be longer then original
        /// data. Client should store the original data size after encryption and use it after
        /// decryption to retrieve the original data from decrypted data.
        /// </summary>
        Task<ResultOfEncryptionBoxEncrypt> EncryptionBoxEncryptAsync(ParamsOfEncryptionBoxEncrypt @params);

        /// <summary>
        /// Block cipher algorithms pad data to cipher block size so encrypted data can be longer then original
        /// data. Client should store the original data size after encryption and use it after
        /// decryption to retrieve the original data from decrypted data.
        /// </summary>
        Task<ResultOfEncryptionBoxDecrypt> EncryptionBoxDecryptAsync(ParamsOfEncryptionBoxDecrypt @params);

        /// <summary>
        /// Creates encryption box with specified algorithm
        /// </summary>
        Task<RegisteredEncryptionBox> CreateEncryptionBoxAsync(ParamsOfCreateEncryptionBox @params);
    }

    internal class CryptoModule : ICryptoModule
    {
        private readonly TonClient _client;

        internal CryptoModule(TonClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<ResultOfFactorize> FactorizeAsync(ParamsOfFactorize @params)
        {
            return await _client.CallFunctionAsync<ResultOfFactorize>("crypto.factorize", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfModularPower> ModularPowerAsync(ParamsOfModularPower @params)
        {
            return await _client.CallFunctionAsync<ResultOfModularPower>("crypto.modular_power", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfTonCrc16> TonCrc16Async(ParamsOfTonCrc16 @params)
        {
            return await _client.CallFunctionAsync<ResultOfTonCrc16>("crypto.ton_crc16", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfGenerateRandomBytes> GenerateRandomBytesAsync(ParamsOfGenerateRandomBytes @params)
        {
            return await _client.CallFunctionAsync<ResultOfGenerateRandomBytes>("crypto.generate_random_bytes", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfConvertPublicKeyToTonSafeFormat> ConvertPublicKeyToTonSafeFormatAsync(ParamsOfConvertPublicKeyToTonSafeFormat @params)
        {
            return await _client.CallFunctionAsync<ResultOfConvertPublicKeyToTonSafeFormat>("crypto.convert_public_key_to_ton_safe_format", @params).ConfigureAwait(false);
        }

        public async Task<KeyPair> GenerateRandomSignKeysAsync()
        {
            return await _client.CallFunctionAsync<KeyPair>("crypto.generate_random_sign_keys").ConfigureAwait(false);
        }

        public async Task<ResultOfSign> SignAsync(ParamsOfSign @params)
        {
            return await _client.CallFunctionAsync<ResultOfSign>("crypto.sign", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfVerifySignature> VerifySignatureAsync(ParamsOfVerifySignature @params)
        {
            return await _client.CallFunctionAsync<ResultOfVerifySignature>("crypto.verify_signature", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfHash> Sha256Async(ParamsOfHash @params)
        {
            return await _client.CallFunctionAsync<ResultOfHash>("crypto.sha256", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfHash> Sha512Async(ParamsOfHash @params)
        {
            return await _client.CallFunctionAsync<ResultOfHash>("crypto.sha512", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfScrypt> ScryptAsync(ParamsOfScrypt @params)
        {
            return await _client.CallFunctionAsync<ResultOfScrypt>("crypto.scrypt", @params).ConfigureAwait(false);
        }

        public async Task<KeyPair> NaclSignKeypairFromSecretKeyAsync(ParamsOfNaclSignKeyPairFromSecret @params)
        {
            return await _client.CallFunctionAsync<KeyPair>("crypto.nacl_sign_keypair_from_secret_key", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfNaclSign> NaclSignAsync(ParamsOfNaclSign @params)
        {
            return await _client.CallFunctionAsync<ResultOfNaclSign>("crypto.nacl_sign", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfNaclSignOpen> NaclSignOpenAsync(ParamsOfNaclSignOpen @params)
        {
            return await _client.CallFunctionAsync<ResultOfNaclSignOpen>("crypto.nacl_sign_open", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfNaclSignDetached> NaclSignDetachedAsync(ParamsOfNaclSign @params)
        {
            return await _client.CallFunctionAsync<ResultOfNaclSignDetached>("crypto.nacl_sign_detached", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfNaclSignDetachedVerify> NaclSignDetachedVerifyAsync(ParamsOfNaclSignDetachedVerify @params)
        {
            return await _client.CallFunctionAsync<ResultOfNaclSignDetachedVerify>("crypto.nacl_sign_detached_verify", @params).ConfigureAwait(false);
        }

        public async Task<KeyPair> NaclBoxKeypairAsync()
        {
            return await _client.CallFunctionAsync<KeyPair>("crypto.nacl_box_keypair").ConfigureAwait(false);
        }

        public async Task<KeyPair> NaclBoxKeypairFromSecretKeyAsync(ParamsOfNaclBoxKeyPairFromSecret @params)
        {
            return await _client.CallFunctionAsync<KeyPair>("crypto.nacl_box_keypair_from_secret_key", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfNaclBox> NaclBoxAsync(ParamsOfNaclBox @params)
        {
            return await _client.CallFunctionAsync<ResultOfNaclBox>("crypto.nacl_box", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfNaclBoxOpen> NaclBoxOpenAsync(ParamsOfNaclBoxOpen @params)
        {
            return await _client.CallFunctionAsync<ResultOfNaclBoxOpen>("crypto.nacl_box_open", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfNaclBox> NaclSecretBoxAsync(ParamsOfNaclSecretBox @params)
        {
            return await _client.CallFunctionAsync<ResultOfNaclBox>("crypto.nacl_secret_box", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfNaclBoxOpen> NaclSecretBoxOpenAsync(ParamsOfNaclSecretBoxOpen @params)
        {
            return await _client.CallFunctionAsync<ResultOfNaclBoxOpen>("crypto.nacl_secret_box_open", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfMnemonicWords> MnemonicWordsAsync(ParamsOfMnemonicWords @params)
        {
            return await _client.CallFunctionAsync<ResultOfMnemonicWords>("crypto.mnemonic_words", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfMnemonicFromRandom> MnemonicFromRandomAsync(ParamsOfMnemonicFromRandom @params)
        {
            return await _client.CallFunctionAsync<ResultOfMnemonicFromRandom>("crypto.mnemonic_from_random", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfMnemonicFromEntropy> MnemonicFromEntropyAsync(ParamsOfMnemonicFromEntropy @params)
        {
            return await _client.CallFunctionAsync<ResultOfMnemonicFromEntropy>("crypto.mnemonic_from_entropy", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfMnemonicVerify> MnemonicVerifyAsync(ParamsOfMnemonicVerify @params)
        {
            return await _client.CallFunctionAsync<ResultOfMnemonicVerify>("crypto.mnemonic_verify", @params).ConfigureAwait(false);
        }

        public async Task<KeyPair> MnemonicDeriveSignKeysAsync(ParamsOfMnemonicDeriveSignKeys @params)
        {
            return await _client.CallFunctionAsync<KeyPair>("crypto.mnemonic_derive_sign_keys", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfHDKeyXPrvFromMnemonic> HdkeyXprvFromMnemonicAsync(ParamsOfHDKeyXPrvFromMnemonic @params)
        {
            return await _client.CallFunctionAsync<ResultOfHDKeyXPrvFromMnemonic>("crypto.hdkey_xprv_from_mnemonic", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfHDKeyDeriveFromXPrv> HdkeyDeriveFromXprvAsync(ParamsOfHDKeyDeriveFromXPrv @params)
        {
            return await _client.CallFunctionAsync<ResultOfHDKeyDeriveFromXPrv>("crypto.hdkey_derive_from_xprv", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfHDKeyDeriveFromXPrvPath> HdkeyDeriveFromXprvPathAsync(ParamsOfHDKeyDeriveFromXPrvPath @params)
        {
            return await _client.CallFunctionAsync<ResultOfHDKeyDeriveFromXPrvPath>("crypto.hdkey_derive_from_xprv_path", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfHDKeySecretFromXPrv> HdkeySecretFromXprvAsync(ParamsOfHDKeySecretFromXPrv @params)
        {
            return await _client.CallFunctionAsync<ResultOfHDKeySecretFromXPrv>("crypto.hdkey_secret_from_xprv", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfHDKeyPublicFromXPrv> HdkeyPublicFromXprvAsync(ParamsOfHDKeyPublicFromXPrv @params)
        {
            return await _client.CallFunctionAsync<ResultOfHDKeyPublicFromXPrv>("crypto.hdkey_public_from_xprv", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfChaCha20> Chacha20Async(ParamsOfChaCha20 @params)
        {
            return await _client.CallFunctionAsync<ResultOfChaCha20>("crypto.chacha20", @params).ConfigureAwait(false);
        }

        public async Task<RegisteredCryptoBox> CreateCryptoBoxAsync(ParamsOfCreateCryptoBox @params, Func<ParamsOfAppPasswordProvider, Task<ResultOfAppPasswordProvider>> password_provider)
        {
            return await _client.CallFunctionAsync<RegisteredCryptoBox, ParamsOfAppPasswordProvider, ResultOfAppPasswordProvider>("crypto.create_crypto_box", @params, password_provider).ConfigureAwait(false);
        }

        public async Task RemoveCryptoBoxAsync(RegisteredCryptoBox @params)
        {
            await _client.CallFunctionAsync("crypto.remove_crypto_box", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfGetCryptoBoxInfo> GetCryptoBoxInfoAsync(RegisteredCryptoBox @params)
        {
            return await _client.CallFunctionAsync<ResultOfGetCryptoBoxInfo>("crypto.get_crypto_box_info", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfGetCryptoBoxSeedPhrase> GetCryptoBoxSeedPhraseAsync(RegisteredCryptoBox @params)
        {
            return await _client.CallFunctionAsync<ResultOfGetCryptoBoxSeedPhrase>("crypto.get_crypto_box_seed_phrase", @params).ConfigureAwait(false);
        }

        public async Task<RegisteredSigningBox> GetSigningBoxFromCryptoBoxAsync(ParamsOfGetSigningBoxFromCryptoBox @params)
        {
            return await _client.CallFunctionAsync<RegisteredSigningBox>("crypto.get_signing_box_from_crypto_box", @params).ConfigureAwait(false);
        }

        public async Task<RegisteredEncryptionBox> GetEncryptionBoxFromCryptoBoxAsync(ParamsOfGetEncryptionBoxFromCryptoBox @params)
        {
            return await _client.CallFunctionAsync<RegisteredEncryptionBox>("crypto.get_encryption_box_from_crypto_box", @params).ConfigureAwait(false);
        }

        public async Task ClearCryptoBoxSecretCacheAsync(RegisteredCryptoBox @params)
        {
            await _client.CallFunctionAsync("crypto.clear_crypto_box_secret_cache", @params).ConfigureAwait(false);
        }

        public async Task<RegisteredSigningBox> RegisterSigningBoxAsync(Func<ParamsOfAppSigningBox, Task<ResultOfAppSigningBox>> app_object)
        {
            return await _client.CallFunctionAsync<RegisteredSigningBox, ParamsOfAppSigningBox, ResultOfAppSigningBox>("crypto.register_signing_box", app_object).ConfigureAwait(false);
        }

        public async Task<RegisteredSigningBox> GetSigningBoxAsync(KeyPair @params)
        {
            return await _client.CallFunctionAsync<RegisteredSigningBox>("crypto.get_signing_box", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfSigningBoxGetPublicKey> SigningBoxGetPublicKeyAsync(RegisteredSigningBox @params)
        {
            return await _client.CallFunctionAsync<ResultOfSigningBoxGetPublicKey>("crypto.signing_box_get_public_key", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfSigningBoxSign> SigningBoxSignAsync(ParamsOfSigningBoxSign @params)
        {
            return await _client.CallFunctionAsync<ResultOfSigningBoxSign>("crypto.signing_box_sign", @params).ConfigureAwait(false);
        }

        public async Task RemoveSigningBoxAsync(RegisteredSigningBox @params)
        {
            await _client.CallFunctionAsync("crypto.remove_signing_box", @params).ConfigureAwait(false);
        }

        public async Task<RegisteredEncryptionBox> RegisterEncryptionBoxAsync(Func<ParamsOfAppEncryptionBox, Task<ResultOfAppEncryptionBox>> app_object)
        {
            return await _client.CallFunctionAsync<RegisteredEncryptionBox, ParamsOfAppEncryptionBox, ResultOfAppEncryptionBox>("crypto.register_encryption_box", app_object).ConfigureAwait(false);
        }

        public async Task RemoveEncryptionBoxAsync(RegisteredEncryptionBox @params)
        {
            await _client.CallFunctionAsync("crypto.remove_encryption_box", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfEncryptionBoxGetInfo> EncryptionBoxGetInfoAsync(ParamsOfEncryptionBoxGetInfo @params)
        {
            return await _client.CallFunctionAsync<ResultOfEncryptionBoxGetInfo>("crypto.encryption_box_get_info", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfEncryptionBoxEncrypt> EncryptionBoxEncryptAsync(ParamsOfEncryptionBoxEncrypt @params)
        {
            return await _client.CallFunctionAsync<ResultOfEncryptionBoxEncrypt>("crypto.encryption_box_encrypt", @params).ConfigureAwait(false);
        }

        public async Task<ResultOfEncryptionBoxDecrypt> EncryptionBoxDecryptAsync(ParamsOfEncryptionBoxDecrypt @params)
        {
            return await _client.CallFunctionAsync<ResultOfEncryptionBoxDecrypt>("crypto.encryption_box_decrypt", @params).ConfigureAwait(false);
        }

        public async Task<RegisteredEncryptionBox> CreateEncryptionBoxAsync(ParamsOfCreateEncryptionBox @params)
        {
            return await _client.CallFunctionAsync<RegisteredEncryptionBox>("crypto.create_encryption_box", @params).ConfigureAwait(false);
        }
    }
}

namespace TonSdk
{
    public partial interface ITonClient
    {
        ICryptoModule Crypto { get; }
    }

    public partial class TonClient
    {
        private CryptoModule _cryptoModule;

        public ICryptoModule Crypto
        {
            get
            {
                return _cryptoModule ?? (_cryptoModule = new CryptoModule(this));
            }
        }
    }
}

