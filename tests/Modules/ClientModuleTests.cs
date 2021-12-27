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
            _client = TestClient.Create(new XUnitTestLogger(outputHelper));
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        [Fact]
        public async Task Should_Return_Version()
        {
            var result = await _client.Client.VersionAsync();
            Assert.NotEmpty(result.Version);
            Assert.Matches(@"\d+\.\d+\.\d+", result.Version);
            Assert.Equal("1.28.0", result.Version);
        }

        [Fact]
        public async Task Should_Return_ApiReference()
        {
            var result = await _client.Client.GetApiReferenceAsync();
            Assert.NotNull(result.Api);
        }

        [Fact]
        public async Task Should_Return_BuildInfo()
        {
            var result = await _client.Client.BuildInfoAsync();
            Assert.NotNull(result);
        }
    }
}
