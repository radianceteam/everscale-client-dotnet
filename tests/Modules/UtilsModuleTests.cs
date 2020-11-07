using System;
using System.Threading.Tasks;
using TonSdk.Modules;
using Xunit;
using Xunit.Abstractions;

namespace TonSdk.Tests.Modules
{
    public class UtilsModuleTests : IDisposable
    {
        private readonly ITonClient _client;

        public UtilsModuleTests(ITestOutputHelper outputHelper)
        {
            _client = TestClient.Create(new XUnitTestLogger(outputHelper));
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        [Fact]
        public async Task Should_Convert_Address_Hex()
        {
            var converted = await _client.Utils.ConvertAddressAsync(new ParamsOfConvertAddress
            {
                Address = "fcb91a3a3816d0f7b8c2c76108b8a9bc5a6b7a55bd79f8ab101c52db29232260",
                OutputFormat = new AddressStringFormat.Hex()
            });

            Assert.NotNull(converted);
            Assert.Equal("0:fcb91a3a3816d0f7b8c2c76108b8a9bc5a6b7a55bd79f8ab101c52db29232260", converted.Address);
        }

        [Fact]
        public async Task Should_Convert_Address_AccountId()
        {
            var converted = await _client.Utils.ConvertAddressAsync(new ParamsOfConvertAddress
            {
                Address = "fcb91a3a3816d0f7b8c2c76108b8a9bc5a6b7a55bd79f8ab101c52db29232260",
                OutputFormat = new AddressStringFormat.AccountId()
            });

            Assert.NotNull(converted);
            Assert.Equal("fcb91a3a3816d0f7b8c2c76108b8a9bc5a6b7a55bd79f8ab101c52db29232260", converted.Address);
        }

        [Theory]
        [InlineData(false, false, false, "-1:fcb91a3a3816d0f7b8c2c76108b8a9bc5a6b7a55bd79f8ab101c52db29232260", "Uf/8uRo6OBbQ97jCx2EIuKm8Wmt6Vb15+KsQHFLbKSMiYG+9")]
        [InlineData(true, true, true, "Uf/8uRo6OBbQ97jCx2EIuKm8Wmt6Vb15+KsQHFLbKSMiYG+9", "kf_8uRo6OBbQ97jCx2EIuKm8Wmt6Vb15-KsQHFLbKSMiYIny")]
        public async Task Should_Convert_Address_Base64(bool bounce, bool test, bool url, string address, string convertedAddress)
        {
            var converted = await _client.Utils.ConvertAddressAsync(new ParamsOfConvertAddress
            {
                Address = address,
                OutputFormat = new AddressStringFormat.Base64
                {
                    Bounce = bounce,
                    Test = test,
                    Url = url
                }
            });

            Assert.NotNull(converted);
            Assert.Equal(convertedAddress, converted.Address);
        }

        [Fact]
        public async Task Should_Convert_Address_HexUrl_To_Hex()
        {
            var converted = await _client.Utils.ConvertAddressAsync(new ParamsOfConvertAddress
            {
                Address = "kf_8uRo6OBbQ97jCx2EIuKm8Wmt6Vb15-KsQHFLbKSMiYIny",
                OutputFormat = new AddressStringFormat.Hex()
            });

            Assert.NotNull(converted);
            Assert.Equal("-1:fcb91a3a3816d0f7b8c2c76108b8a9bc5a6b7a55bd79f8ab101c52db29232260", converted.Address);
        }

    }
}
