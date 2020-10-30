using System;
using System.Text;
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
            _client = TonClient.Create(new TonClientConfig
            {
                Logger = new XUnitTestLogger(outputHelper)
            });
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        [Fact]
        public async Task ShouldFactorize()
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

            Assert.Equal(106, e.Code);
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
                Unsigned = Convert.ToBase64String(Encoding.UTF8.GetBytes("test")),
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
            Assert.Equal("test", Encoding.UTF8.GetString(Convert.FromBase64String(verified.Unsigned)));
        }

        [Fact]
        public async Task Should_Not_Verify_String_Signed_With_Other_Key_Pair()
        {
            var keys = await _client.Crypto.GenerateRandomSignKeysAsync();
            var otherKeys = await _client.Crypto.GenerateRandomSignKeysAsync();

            var result = await _client.Crypto.SignAsync(new ParamsOfSign
            {
                Unsigned = Convert.ToBase64String(Encoding.UTF8.GetBytes("test")),
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

            Assert.Equal(112, exception.Code);
            Assert.Contains("verify signature failed", exception.Message);
        }

        [Fact]
        public async Task Should_Calculate_Sha256()
        {
            var result = await _client.Crypto.Sha256Async(new ParamsOfHash
            {
                Data = Convert.ToBase64String(Encoding.UTF8.GetBytes("12345"))
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
                Data = Convert.ToBase64String(Encoding.UTF8.GetBytes("12345"))
            });

            Assert.NotNull(result);
            Assert.NotEmpty(result.Hash);
            Assert.Equal("3627909A29C31381A071EC27F7C9CA97726182AED29A7DDD2E54353322CFB30ABB9E3A6DF2AC2C20FE23436311D678564D0C8D305930575F60E2D3D048184D79", result.Hash.ToUpper());
        }
    }
}
