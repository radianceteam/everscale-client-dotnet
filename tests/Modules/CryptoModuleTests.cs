using System;
using System.Threading.Tasks;
using TonSdk.Modules;
using Xunit;
using Xunit.Abstractions;

namespace TonSdk.Tests.Modules
{
    public class CryptoModuleTests : IDisposable
    {
        private readonly ITonClient _client;

        public CryptoModuleTests(ITestOutputHelper outputHelper)
        {
            _client = TonClient.Create(new XUnitTestLogger(outputHelper));
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        [Fact]
        public async Task Should_Factorize()
        {
            var result = await _client.Crypto.FactorizeAsync(new ParamsOfFactorize
            {
                Composite = "c"
            });

            Assert.NotNull(result);
            Assert.Equal(new[] { "3", "4" }, result.Factors);
        }

        [Fact]
        public async Task Should_Not_Factorize_Prime_Number()
        {
            var e = await Assert.ThrowsAsync<TonClientException>(() =>
                _client.Crypto.FactorizeAsync(new ParamsOfFactorize
                {
                    Composite = "b"
                }));

            Assert.Equal(106, e.Code); // InvalidFactorizeChallenge 
            Assert.Contains("Invalid factorize challenge: Composite number can't be factorized", e.Message);
            Assert.True(e.Data.Contains("core_version"));

            var versionString = e.Data["core_version"]?.ToString();
            Assert.NotNull(versionString);
            Assert.Matches(@"\d+\.\d+\.\d+", versionString);
        }

        [Fact]
        public async Task Should_Calculate_Modular_Power()
        {
            var result = await _client.Crypto.ModularPowerAsync(new ParamsOfModularPower
            {
                Base = "4",
                Exponent = "2",
                Modulus = "6"
            });

            Assert.NotNull(result);
            Assert.Equal("4", result.ModularPower);
        }

        [Fact]
        public async Task Should_Calculate_TonCrc16()
        {
            var result = await _client.Crypto.TonCrc16Async(new ParamsOfTonCrc16
            {
                Data = "anything"
            });

            Assert.NotNull(result);
            Assert.Equal(21741, result.Crc);
        }

        [Fact]
        public async Task Should_Generate_Random_Bytes()
        {
            var result = await _client.Crypto.GenerateRandomBytesAsync(new ParamsOfGenerateRandomBytes
            {
                Length = 10
            });

            Assert.NotNull(result);
            Assert.Equal(10, Convert.FromBase64String(result.Bytes).Length);
        }

        [Fact]
        public async Task Should_ConvertPublicKeyToTonSafeFormat()
        {
            var key = "1114676765926380121143944239225577351517094772360684970410316804";
            var result = await _client.Crypto.ConvertPublicKeyToTonSafeFormatAsync(new ParamsOfConvertPublicKeyToTonSafeFormat
            {
                PublicKey = key
            });

            Assert.NotNull(result);
            Assert.NotEmpty(result.TonPublicKey);
            Assert.NotEqual(result.TonPublicKey, key);
        }

        [Fact]
        public async Task Should_GenerateRandomSignKeys()
        {
            var result = await _client.Crypto.GenerateRandomSignKeysAsync();
            Assert.NotNull(result);
            Assert.NotEmpty(result.Public);
            Assert.NotEmpty(result.Secret);
        }

        [Fact]
        public async Task Should_Verify_Signed_String()
        {
            var keys = await _client.Crypto.GenerateRandomSignKeysAsync();
            var result = await _client.Crypto.SignAsync(new ParamsOfSign
            {
                Unsigned = "test".ToBase64String(),
                Keys = keys
            });

            Assert.NotNull(result);
            Assert.NotEmpty(result.Signed);
            Assert.NotEmpty(result.Signature);

            var verified = await _client.Crypto.VerifySignatureAsync(new ParamsOfVerifySignature
            {
                Public = keys.Public,
                Signed = result.Signed
            });

            Assert.NotNull(verified);
            Assert.NotEmpty(verified.Unsigned);
            Assert.Equal("test", verified.Unsigned.FromBase64String());
        }

        [Fact]
        public async Task Should_Not_Verify_String_Signed_With_Other_Key_Pair()
        {
            var keys = await _client.Crypto.GenerateRandomSignKeysAsync();
            var otherKeys = await _client.Crypto.GenerateRandomSignKeysAsync();

            var result = await _client.Crypto.SignAsync(new ParamsOfSign
            {
                Unsigned = "test".ToBase64String(),
                Keys = keys
            });

            Assert.NotNull(result);
            Assert.NotEmpty(result.Signed);
            Assert.NotEmpty(result.Signature);

            var exception = await Assert.ThrowsAsync<TonClientException>(() =>
                _client.Crypto.VerifySignatureAsync(new ParamsOfVerifySignature
                {
                    Public = otherKeys.Public,
                    Signed = result.Signed
                }));

            Assert.Equal(112, exception.Code); // NaclSignFailed 
            Assert.Contains("verify signature failed", exception.Message);
        }

        [Fact]
        public async Task Should_Calculate_Sha256()
        {
            var result = await _client.Crypto.Sha256Async(new ParamsOfHash
            {
                Data = "12345".ToBase64String()
            });

            Assert.NotNull(result);
            Assert.NotEmpty(result.Hash);
            Assert.Equal("5994471ABB01112AFCC18159F6CC74B4F511B99806DA59B3CAF5A9C173CACFC5", result.Hash.ToUpper());
        }

        [Fact]
        public async Task Should_Calculate_Sha512()
        {
            var result = await _client.Crypto.Sha512Async(new ParamsOfHash
            {
                Data = "12345".ToBase64String()
            });

            Assert.NotNull(result);
            Assert.NotEmpty(result.Hash);
            Assert.Equal("3627909A29C31381A071EC27F7C9CA97726182AED29A7DDD2E54353322CFB30ABB9E3A6DF2AC2C20FE23436311D678564D0C8D305930575F60E2D3D048184D79", result.Hash.ToUpper());
        }

        [Theory]
        [InlineData(15, 8, 1, 4, "c2VjcmV0", "dGVzdA==")]
        [InlineData(1, 8, 1, 4, "c2VjcmV0", "dGVzdA==")]
        [InlineData(15, 1, 1, 4, "c2VjcmV0", "dGVzdA==")]
        [InlineData(15, 8, 1, 4, "", "dGVzdA==")]
        [InlineData(15, 8, 1, 4, "c2VjcmV0", "")]
        public async Task Scrypt_Should_Produce_Correct_Result_For_Valid_Input(int logN, int r, int p, int dkLen, string password, string salt)
        {
            var result = await _client.Crypto.ScryptAsync(new ParamsOfScrypt
            {
                LogN = logN,
                R = r,
                P = p,
                DkLen = dkLen,
                Password = password,
                Salt = salt
            });

            Assert.NotNull(result);
            Assert.NotEmpty(result.Key);
        }

        [Theory]
        [InlineData(-1, 8, 1, 4, "c2VjcmV0", "dGVzdA==", 23)] // `log_n` must be be greater than `0`, InvalidParams
        [InlineData(65, 8, 1, 4, "c2VjcmV0", "dGVzdA==", 108)] // `log_n` must be less than `64`, ScryptFailed 
        [InlineData(15, 0, 1, 4, "c2VjcmV0", "dGVzdA==", 23)] // `r` must be greater than `0`, InvalidParams
        [InlineData(15, 8, 0, 4, "c2VjcmV0", "dGVzdA==", 23)] // `p` must be greater than `0`, InvalidParams
        [InlineData(15, 8, 1, 4, null, "dGVzdA==", 23)] // `password` must not be null, InvalidParams
        [InlineData(15, 8, 1, 4, "c2VjcmV0", null, 23)] // `salt` must not be null, InvalidParams
        public async Task Scrypt_Should_Throw_Exception_On_Invalid_Input(int logN, int r, int p, int dkLen, string password, string salt, int errorCode)
        {
            var exception = await Assert.ThrowsAsync<TonClientException>(() => _client.Crypto.ScryptAsync(new ParamsOfScrypt
            {
                LogN = logN,
                R = r,
                P = p,
                DkLen = dkLen,
                Password = password,
                Salt = salt
            }));

            Assert.NotNull(exception);
            Assert.Equal(errorCode, exception.Code);
            Assert.NotEmpty(exception.Message);
        }

        [Fact]
        public async Task Should_Call_NaclSignKeyPairFromSecretKey()
        {
            var keyPair = await _client.Crypto.GenerateRandomSignKeysAsync();

            var result = await _client.Crypto.NaclSignKeypairFromSecretKeyAsync(new ParamsOfNaclSignKeyPairFromSecret
            {
                Secret = keyPair.Secret
            });

            Assert.NotNull(result);
            Assert.NotEmpty(result.Secret);
            Assert.Equal(result.Public, keyPair.Public);
        }

        [Theory]
        [InlineData("test")] // not base64-encoded
        [InlineData("dGVzdA==")] // not a secret key
        public async Task NaclSignKeyPairFromSecretKey_Should_Throw_Exception_For_Invalid_Secret_Key(string secret)
        {
            var exception = await Assert.ThrowsAsync<TonClientException>(() => _client.Crypto
                .NaclSignKeypairFromSecretKeyAsync(new ParamsOfNaclSignKeyPairFromSecret
                {
                    Secret = secret
                }));

            Assert.NotNull(exception);
            Assert.NotEmpty(exception.Message);
            Assert.Equal(101, exception.Code); // InvalidSecretKey 
        }

        [Fact]
        public async Task Should_Call_Nacl_Sign_And_Sign_Open()
        {
            var signKeys = await _client.Crypto.GenerateRandomSignKeysAsync();
            var keyPair = await _client.Crypto.NaclSignKeypairFromSecretKeyAsync(new ParamsOfNaclSignKeyPairFromSecret
            {
                Secret = signKeys.Secret
            });

            var result = await _client.Crypto.NaclSignAsync(new ParamsOfNaclSign
            {
                Secret = keyPair.Secret,
                Unsigned = "test".ToBase64String()
            });

            Assert.NotNull(result);
            Assert.NotEmpty(result.Signed);

            var unsigned = await _client.Crypto.NaclSignOpenAsync(new ParamsOfNaclSignOpen
            {
                Public = keyPair.Public,
                Signed = result.Signed
            });

            Assert.NotNull(unsigned);
            Assert.NotEmpty(unsigned.Unsigned);
            Assert.Equal("test", unsigned.Unsigned.FromBase64String());
        }

        [Fact]
        public async Task Nacl_Sign_Should_Throw_Exception_For_Invalid_Data()
        {
            var exception = await Assert.ThrowsAsync<TonClientException>(async () =>
            {
                var signKeys = await _client.Crypto.GenerateRandomSignKeysAsync();
                var keyPair = await _client.Crypto.NaclSignKeypairFromSecretKeyAsync(
                    new ParamsOfNaclSignKeyPairFromSecret
                    {
                        Secret = signKeys.Secret
                    });

                await _client.Crypto.NaclSignAsync(new ParamsOfNaclSign
                {
                    Secret = keyPair.Secret,
                    Unsigned = null
                });
            });

            Assert.NotEmpty(exception.Message);
            Assert.Equal(23, exception.Code); // InvalidParams
        }

        [Fact]
        public async Task Nacl_Sign_Should_Throw_Exception_For_Invalid_Secret_Length()
        {
            var exception = await Assert.ThrowsAsync<TonClientException>(async () =>
            {
                var signKeys = await _client.Crypto.GenerateRandomSignKeysAsync();
                await _client.Crypto.NaclSignAsync(new ParamsOfNaclSign
                {
                    Secret = signKeys.Secret,
                    Unsigned = "test".ToBase64String()
                });
            });

            Assert.NotEmpty(exception.Message);
            Assert.Equal(109, exception.Code); // InvalidKeySize
        }

        [Theory]
        [InlineData(null, 23)] // InvalidParams
        [InlineData("", 109)] // InvalidKeySize 
        [InlineData("test", 2)] // InvalidHex
        [InlineData("dGVzdA==", 2)] // InvalidHex
        public async Task Nacl_Sign_Should_Throw_Exception_For_Invalid_Secret(string secret, int errorCode)
        {
            var exception = await Assert.ThrowsAsync<TonClientException>(() => _client.Crypto
                .NaclSignAsync(new ParamsOfNaclSign
                {
                    Secret = secret,
                    Unsigned = "test".ToBase64String()
                }));

            Assert.NotEmpty(exception.Message);
            Assert.Equal(errorCode, exception.Code);
        }

        [Fact]
        public async Task Should_Call_NaclSignDetached()
        {
            var result = await _client.Crypto.NaclSignDetachedAsync(new ParamsOfNaclSign
            {
                Unsigned = "Test Message".ToBase64String(),
                Secret =
                    "56b6a77093d6fdf14e593f36275d872d75de5b341942376b2a08759f3cbae78f1869b7ef29d58026217e9cf163cbfbd0de889bdf1bf4daebf5433a312f5b8d6e"
            });

            Assert.NotNull(result);
            Assert.Equal("fb0cfe40eea5d6c960652e6ceb904da8a72ee2fcf6e05089cf835203179ff65bb48c57ecf31dcfcd26510bea67e64f3e6898b7c58300dc14338254268cade103", result.Signature);
        }

        [Fact]
        public async Task Should_Create_NaclBoxKeyPair()
        {
            var result = await _client.Crypto.NaclBoxKeypairAsync();
            Assert.NotNull(result);
            Assert.Equal(64, result.Public.Length);
            Assert.Equal(64, result.Secret.Length);
            Assert.NotEqual(result.Public, result.Secret);
        }

        [Fact]
        public async Task Should_Call_NaclBox()
        {
            var result = await _client.Crypto.NaclBoxAsync(new ParamsOfNaclBox
            {
                Decrypted = "Test Message".ToBase64String(),
                Nonce = "cd7f99924bf422544046e83595dd5803f17536f5c9a11746",
                TheirPublic = "c4e2d9fe6a6baf8d1812b799856ef2a306291be7a7024837ad33a8530db79c6b",
                Secret = "d9b9dc5033fb416134e5d2107fdbacab5aadb297cb82dbdcd137d663bac59f7f"
            });

            Assert.NotNull(result);
            Assert.Equal("li4XED4kx/pjQ2qdP0eR2d/K30uN94voNADxwA==", result.Encrypted);
        }

        [Fact]
        public async Task Should_Call_NaclBoxOpen()
        {
            var result = await _client.Crypto.NaclBoxOpenAsync(new ParamsOfNaclBoxOpen
            {
                Encrypted = "962e17103e24c7fa63436a9d3f4791d9dfcadf4b8df78be83400f1c0".HexToBase64String(),
                Nonce = "cd7f99924bf422544046e83595dd5803f17536f5c9a11746",
                TheirPublic = "c4e2d9fe6a6baf8d1812b799856ef2a306291be7a7024837ad33a8530db79c6b",
                Secret = "d9b9dc5033fb416134e5d2107fdbacab5aadb297cb82dbdcd137d663bac59f7f"
            });

            Assert.NotNull(result);
            Assert.Equal("Test Message", result.Decrypted.FromBase64String());
        }

        [Fact]
        public async Task Should_Call_NaclSecretBox()
        {
            var result = await _client.Crypto.NaclSecretBoxAsync(new ParamsOfNaclSecretBox
            {
                Decrypted = "Test Message".ToBase64String(),
                Nonce = "2a33564717595ebe53d91a785b9e068aba625c8453a76e45",
                Key = "8f68445b4e78c000fe4d6b7fc826879c1e63e3118379219a754ae66327764bd8"
            });

            Assert.NotNull(result);
            Assert.Equal("JL7ejKWe2KXmrsns41yfXoQF0t/C1Q8RGyzQ2A==", result.Encrypted);
        }

        [Fact]
        public async Task Should_Call_NaclSecretBoxOpen()
        {
            var result = await _client.Crypto.NaclSecretBoxOpenAsync(new ParamsOfNaclSecretBoxOpen
            {
                Encrypted = "24bede8ca59ed8a5e6aec9ece35c9f5e8405d2dfc2d50f111b2cd0d8".HexToBase64String(),
                Nonce = "2a33564717595ebe53d91a785b9e068aba625c8453a76e45",
                Key = "8f68445b4e78c000fe4d6b7fc826879c1e63e3118379219a754ae66327764bd8"
            });

            Assert.NotNull(result);
            Assert.Equal("Test Message", result.Decrypted.FromBase64String());
        }

        [Fact]
        public async Task Should_Encrypt_And_Decrypt_Message_With_Special_Chars_Using_Nacl_Box_Functions()
        {
            const string nonce = "2a33564717595ebe53d91a785b9e068aba625c8453a76e45";
            const string key = "8f68445b4e78c000fe4d6b7fc826879c1e63e3118379219a754ae66327764bd8";
            const string text = "Text with \' and \" and : {}";

            var result = await _client.Crypto.NaclSecretBoxAsync(new ParamsOfNaclSecretBox
            {
                Decrypted = text.ToBase64String(),
                Nonce = nonce,
                Key = key
            });

            var open = await _client.Crypto.NaclSecretBoxOpenAsync(new ParamsOfNaclSecretBoxOpen
            {
                Encrypted = result.Encrypted,
                Nonce = nonce,
                Key = key
            });

            Assert.Equal(text, open.Decrypted.FromBase64String());
        }

        [Fact]
        public async Task Should_Call_MnemonicWords()
        {
            var result = await _client.Crypto.MnemonicWordsAsync(new ParamsOfMnemonicWords());
            Assert.NotNull(result);
            Assert.Equal(2048, result.Words.Split(" ").Length);
        }

        [Theory]
        [CombinatorialData]
        public async Task Should_Call_MnemonicFromRandom(
            [CombinatorialValues(1, 2, 3, 4, 5, 6, 7, 8)] int dictionary,
            [CombinatorialValues(12, 15, 18, 21, 24)] int wordCount)
        {
            var result = await _client.Crypto.MnemonicFromRandomAsync(new ParamsOfMnemonicFromRandom
            {
                Dictionary = dictionary,
                WordCount = wordCount
            });
            Assert.NotNull(result);
            Assert.Equal(wordCount, result.Phrase.Split(" ").Length);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(0, 12)]
        [InlineData(1, 12)]
        public async Task Should_Call_MnemonicFromRandom_More_Checks(int? dictionary, int? wordCount)
        {
            var result = await _client.Crypto.MnemonicFromRandomAsync(new ParamsOfMnemonicFromRandom
            {
                Dictionary = dictionary,
                WordCount = wordCount
            });
            Assert.NotNull(result);
            Assert.Equal(12, result.Phrase.Split(" ").Length);
        }

        [Fact]
        public async Task Should_Call_MnemonicFromEntropy()
        {
            var result = await _client.Crypto.MnemonicFromEntropyAsync(new ParamsOfMnemonicFromEntropy
            {
                Entropy = "00112233445566778899AABBCCDDEEFF",
                Dictionary = 1,
                WordCount = 12
            });
            Assert.NotNull(result);
            Assert.Equal("abandon math mimic master filter design carbon crystal rookie group knife young", result.Phrase);
        }

        [Theory]
        [CombinatorialData]
        public async Task Should_Call_MnemonicVerify(
            [CombinatorialValues(1, 2, 3, 4, 5, 6, 7, 8)] int dictionary,
            [CombinatorialValues(12, 15, 18, 21, 24)] int wordCount)
        {
            var result = await _client.Crypto.MnemonicFromRandomAsync(new ParamsOfMnemonicFromRandom
            {
                Dictionary = dictionary,
                WordCount = wordCount
            });

            Assert.NotNull(result);

            var verifyResult = await _client.Crypto.MnemonicVerifyAsync(new ParamsOfMnemonicVerify
            {
                Phrase = result.Phrase,
                Dictionary = dictionary,
                WordCount = wordCount
            });

            Assert.NotNull(verifyResult);
            Assert.True(verifyResult.Valid);
        }

        [Fact]
        public async Task MnemonicVerify_Should_Not_Verify_Invalid_Phrase()
        {
            var result = await _client.Crypto.MnemonicVerifyAsync(new ParamsOfMnemonicVerify
            {
                Phrase = "one two"
            });

            Assert.NotNull(result);
            Assert.False(result.Valid);
        }

        [Fact]
        public async Task Should_Call_MnemonicDeriveSignKeys()
        {
            var result = await _client.Crypto.MnemonicDeriveSignKeysAsync(new ParamsOfMnemonicDeriveSignKeys
            {
                Phrase =
                    "unit follow zone decline glare flower crisp vocal adapt magic much mesh cherry teach mechanic rain float vicious solution assume hedgehog rail sort chuckle",
                Dictionary = 0,
                WordCount = 24
            });

            Assert.NotNull(result);
            Assert.NotEmpty(result.Public);

            var convertedResult = await _client.Crypto.ConvertPublicKeyToTonSafeFormatAsync(
                new ParamsOfConvertPublicKeyToTonSafeFormat
                {
                    PublicKey = result.Public
                });
            Assert.NotNull(convertedResult);
            Assert.Equal("PuYTvCuf__YXhp-4jv3TXTHL0iK65ImwxG0RGrYc1sP3H4KS", convertedResult.TonPublicKey);
        }

        [Fact]
        public async Task Should_Call_MnemonicDeriveSignKeys_With_Path()
        {
            var result = await _client.Crypto.MnemonicDeriveSignKeysAsync(new ParamsOfMnemonicDeriveSignKeys
            {
                Phrase =
                    "unit follow zone decline glare flower crisp vocal adapt magic much mesh cherry teach mechanic rain float vicious solution assume hedgehog rail sort chuckle",
                Path = "m",
                Dictionary = 0,
                WordCount = 24
            });

            Assert.NotNull(result);
            Assert.NotEmpty(result.Public);

            var convertedResult = await _client.Crypto.ConvertPublicKeyToTonSafeFormatAsync(
                new ParamsOfConvertPublicKeyToTonSafeFormat
                {
                    PublicKey = result.Public
                });
            Assert.NotNull(convertedResult);
            Assert.Equal("PubDdJkMyss2qHywFuVP1vzww0TpsLxnRNnbifTCcu-XEgW0", convertedResult.TonPublicKey);
        }

        [Fact]
        public async Task Should_Call_MnemonicDeriveSignKeys_With_No_Dictionary_And_Word_Count_Params()
        {
            var result = await _client.Crypto.MnemonicDeriveSignKeysAsync(new ParamsOfMnemonicDeriveSignKeys
            {
                Phrase = "abandon math mimic master filter design carbon crystal rookie group knife young"
            });

            Assert.NotNull(result);
            Assert.NotEmpty(result.Public);

            var convertedResult = await _client.Crypto.ConvertPublicKeyToTonSafeFormatAsync(
                new ParamsOfConvertPublicKeyToTonSafeFormat
                {
                    PublicKey = result.Public
                });
            Assert.NotNull(convertedResult);
            Assert.Equal("PuZhw8W5ejPJwKA68RL7sn4_RNmeH4BIU_mEK7em5d4_-cIx", convertedResult.TonPublicKey);
        }

        [Fact]
        public async Task MnemonicDeriveSignKeys_Should_Work_In_Combination_With_MnemonicFromEntropy()
        {
            var result = await _client.Crypto.MnemonicFromEntropyAsync(new ParamsOfMnemonicFromEntropy
            {
                Entropy = "2199ebe996f14d9e4e2595113ad1e627"
            });
            Assert.NotNull(result);

            var deriveResult = await _client.Crypto.MnemonicDeriveSignKeysAsync(new ParamsOfMnemonicDeriveSignKeys
            {
                Phrase = result.Phrase
            });

            Assert.NotNull(deriveResult);
            Assert.NotEmpty(deriveResult.Public);

            var convertedResult = await _client.Crypto.ConvertPublicKeyToTonSafeFormatAsync(
                new ParamsOfConvertPublicKeyToTonSafeFormat
                {
                    PublicKey = deriveResult.Public
                });
            Assert.NotNull(convertedResult);
            Assert.Equal("PuZdw_KyXIzo8IksTrERN3_WoAoYTyK7OvM-yaLk711sUIB3", convertedResult.TonPublicKey);
        }

        [Fact]
        public async Task Should_Call_HdkeyXprvFromMnemonic()
        {
            var master = await _client.Crypto.HdkeyXprvFromMnemonicAsync(new ParamsOfHDKeyXPrvFromMnemonic
            {
                Phrase = "abuse boss fly battle rubber wasp afraid hamster guide essence vibrant tattoo"
            });

            Assert.NotNull(master);
            Assert.Equal("xprv9s21ZrQH143K25JhKqEwvJW7QAiVvkmi4WRenBZanA6kxHKtKAQQKwZG65kCyW5jWJ8NY9e3GkRoistUjjcpHNsGBUv94istDPXvqGNuWpC", master.Xprv);
        }
    }
}
