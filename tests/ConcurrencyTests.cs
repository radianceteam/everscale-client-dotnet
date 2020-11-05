using System;
using System.Linq;
using System.Threading.Tasks;
using TonSdk.Modules;
using Xunit;
using Xunit.Abstractions;

namespace TonSdk.Tests
{
    public class ConcurrencyTests
    {
        private readonly ITonClient _client;

        public ConcurrencyTests(ITestOutputHelper outputHelper)
        {
            _client = TonClient.Create(new XUnitTestLogger(outputHelper));
        }

        [Fact]
        public async Task Should_Perform_Multiple_Calls_Concurrently()
        {
            const int numCalls = 100000; // local PC got crazy at 100k
            var results = await Task.WhenAll(Enumerable
                .Range(0, numCalls)
                .Select(_ => Task.Run(async () =>
                {
                    var data = Guid.NewGuid().ToString();
                    var result = await _client.Crypto.Sha256Async(new ParamsOfHash
                    {
                        Data = data.ToBase64String()
                    });
                    return result != null && data
                        .Sha256()
                        .Equals(result.Hash);
                })).ToArray());

            Assert.Equal(numCalls, results.Length);
            Assert.All(results, Assert.True);
        }
    }
}
