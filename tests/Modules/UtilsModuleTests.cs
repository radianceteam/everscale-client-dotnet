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

        [Fact]
        public async Task Should_Calc_Storage_Fee()
        {
            var result = await _client.Utils.CalcStorageFeeAsync(new ParamsOfCalcStorageFee
            {
                Account =
                    "te6ccgECHQEAA/wAAnfAArtKDoOR5+qId/SCUGSSS9Qc4RD86X6TnTMjmZ4e+7EyOobmQvsHNngAAAg6t/34DgJWKJuuOehjU0ADAQFBlcBqp0PR+QAN1kt1SY8QavS350RCNNfeZ+ommI9hgd/gAgBToB6t2E3E7a7aW2YkvXv2hTmSWVRTvSYmCVdH4HjgZ4Z94AAAAAvsHNwwAib/APSkICLAAZL0oOGK7VNYMPShBgQBCvSkIPShBQAAAgEgCgcBAv8IAf5/Ie1E0CDXScIBn9P/0wD0Bfhqf/hh+Gb4Yo4b9AVt+GpwAYBA9A7yvdcL//hicPhjcPhmf/hh4tMAAY4SgQIA1xgg+QFY+EIg+GX5EPKo3iP4RSBukjBw3vhCuvLgZSHTP9MfNCD4I7zyuSL5ACD4SoEBAPQOIJEx3vLQZvgACQA2IPhKI8jLP1mBAQD0Q/hqXwTTHwHwAfhHbvJ8AgEgEQsCAVgPDAEJuOiY/FANAdb4QW6OEu1E0NP/0wD0Bfhqf/hh+Gb4Yt7RcG1vAvhKgQEA9IaVAdcLP3+TcHBw4pEgjjJfM8gizwv/Ic8LPzExAW8iIaQDWYAg9ENvAjQi+EqBAQD0fJUB1ws/f5NwcHDiAjUzMehfAyHA/w4AmI4uI9DTAfpAMDHIz4cgzo0EAAAAAAAAAAAAAAAAD3RMfijPFiFvIgLLH/QAyXH7AN4wwP+OEvhCyMv/+EbPCwD4SgH0AMntVN5/+GcBCbkWq+fwEAC2+EFujjbtRNAg10nCAZ/T/9MA9AX4an/4Yfhm+GKOG/QFbfhqcAGAQPQO8r3XC//4YnD4Y3D4Zn/4YeLe+Ebyc3H4ZtH4APhCyMv/+EbPCwD4SgH0AMntVH/4ZwIBIBUSAQm7Fe+TWBMBtvhBbo4S7UTQ0//TAPQF+Gp/+GH4Zvhi3vpA1w1/ldTR0NN/39cMAJXU0dDSAN/RVHEgyM+FgMoAc89AzgH6AoBrz0DJc/sA+EqBAQD0hpUB1ws/f5NwcHDikSAUAISOKCH4I7ubIvhKgQEA9Fsw+GreIvhKgQEA9HyVAdcLP3+TcHBw4gI1MzHoXwb4QsjL//hGzwsA+EoB9ADJ7VR/+GcCASAYFgEJuORhh1AXAL74QW6OEu1E0NP/0wD0Bfhqf/hh+Gb4Yt7U0fhFIG6SMHDe+EK68uBl+AD4QsjL//hGzwsA+EoB9ADJ7VT4DyD7BCDQ7R7tU/ACMPhCyMv/+EbPCwD4SgH0AMntVH/4ZwIC2hsZAQFIGgAs+ELIy//4Rs8LAPhKAfQAye1U+A/yAAEBSBwAWHAi0NYCMdIAMNwhxwDcIdcNH/K8UxHdwQQighD////9vLHyfAHwAfhHbvJ8",
                Period = 1000
            });

            Assert.Equal("330", result.Fee);
        }

        [Fact]
        public async Task Should_Compress_Zstd()
        {
            var uncompressed = @"Lorem ipsum dolor sit amet";

            var compressed = await _client.Utils.CompressZstdAsync(new ParamsOfCompressZstd
            {
                Uncompressed = uncompressed.ToBase64String(), 
                Level = 21
            });

            Assert.Equal(@"KLUv/QCA0QAATG9yZW0gaXBzdW0gZG9sb3Igc2l0IGFtZXQ=", compressed.Compressed);
        }

        [Fact]
        public async Task Should_Decompress_Zstd()
        {
            var compressed = @"KLUv/QCA0QAATG9yZW0gaXBzdW0gZG9sb3Igc2l0IGFtZXQ=";

            var decompressed = await _client.Utils.DecompressZstdAsync(new ParamsOfDecompressZstd
            {
                Compressed = compressed
            });

            Assert.Equal(@"Lorem ipsum dolor sit amet", decompressed.Decompressed.FromBase64String());
        }
    }
}
