using System.Threading.Tasks;
using TonSdk;
using Xunit;
using Xunit.Abstractions;

namespace ton_client_dotnet_tests
{
    public class VersionTest
    {
        private readonly XUnitTestLogger _logger;

        public VersionTest(ITestOutputHelper outputHelper)
        {
            _logger = new XUnitTestLogger(outputHelper);
        }

        [Fact]
        public async Task TestVersionReturnedByTonClient()
        {
            using var client = await TonClient.CreateAsync(new TonClientConfig
            {
                Logger = _logger
            });
            var result = await client.Client.GetVersionAsync();
            Assert.NotEmpty(result.Version);
        }
    }
}
