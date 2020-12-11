using Xunit;

namespace TonSdk.Tests
{
    public class StringExtensionTests
    {
        [Theory]
        [InlineData("Hello world!", "48656c6c6f20776f726c6421")]
        public void Should_Convert_String_To_Hex_String(string input, string hex)
        {
            Assert.Equal(hex, input.ToHexString());
        }
    }
}
