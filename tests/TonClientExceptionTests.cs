using Newtonsoft.Json;
using Xunit;

namespace TonSdk.Tests
{
    public class TonClientExceptionTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("test")]
        [InlineData(@"{""test"":""value""}")]
        public void FromJson_Should_Return_Exception_Even_If_Json_Is_Invalid(string json)
        {
            Assert.NotNull(TonClientException.FromJson(json));
        }

        [Fact]
        public void FromJson_Should_Use_Code_From_Json()
        {
            var json = JsonConvert.SerializeObject(new
            {
                code = 1001
            });

            var exception = TonClientException.FromJson(json);
            Assert.NotNull(exception);
            Assert.Equal(1001, exception.Code);
            Assert.NotEmpty(exception.Message);
            Assert.Empty(exception.Data);
        }

        [Fact]
        public void FromJson_Should_Use_Message_From_Json()
        {
            var json = JsonConvert.SerializeObject(new
            {
                code = 1001,
                message = "test message"
            });

            var exception = TonClientException.FromJson(json);
            Assert.NotNull(exception);
            Assert.Equal(1001, exception.Code);
            Assert.Equal("test message", exception.Message);
            Assert.Empty(exception.Data);
        }

        [Fact]
        public void FromJson_Should_Use_Data_From_Json()
        {
            var json = JsonConvert.SerializeObject(new
            {
                code = 1001,
                message = "test message",
                data = new
                {
                    version = "1.0.0"
                }
            });

            var exception = TonClientException.FromJson(json);
            Assert.NotNull(exception);
            Assert.Equal(1001, exception.Code);
            Assert.Equal("test message", exception.Message);
            Assert.True(exception.Data.Contains("version"));
            Assert.Equal("1.0.0", exception.Data["version"]);
        }
    }
}
