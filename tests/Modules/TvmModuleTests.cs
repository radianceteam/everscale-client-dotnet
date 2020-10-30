using System;
using Xunit.Abstractions;

namespace TonSdk.Tests.Modules
{
    public class TvmModuleTests : IDisposable
    {
        private readonly ITonClient _client;

        public TvmModuleTests(ITestOutputHelper outputHelper)
        {
            _client = TonClient.Create(new XUnitTestLogger(outputHelper));
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        // TODO: implement!
    }
}
