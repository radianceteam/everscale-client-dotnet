using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace TonSdk.Tests.Modules
{
    public class ClientModuleTests : IDisposable
    {
        private readonly ITonClient _client;

        public ClientModuleTests(ITestOutputHelper outputHelper)
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
        public async Task ShouldReturnVersion()
        {
            var result = await _client.Client.VersionAsync();
            Assert.NotEmpty(result.Version);
            Assert.Matches(@"\d+\.\d+\.\d+", result.Version);
        }

        [Fact]
        public async Task ShouldReturnGetApiReference()
        {
            var result = await _client.Client.GetApiReferenceAsync();
            Assert.NotNull(result.Api);
        }

        [Fact]
        public async Task ShouldReturnBuildInfo()
        {
            var result = await _client.Client.BuildInfoAsync();
            Assert.NotNull(result.BuildInfo);
        }
    }
}
