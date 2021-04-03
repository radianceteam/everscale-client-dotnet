using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace TonSdk.Tests.Modules
{
    public class DebotModuleTests2 : IClassFixture<DebotFixture<TestDebot2>>
    {
        private readonly DebotFixture<TestDebot2> _fixture;
        private readonly ILogger _logger;

        public DebotModuleTests2(DebotFixture<TestDebot2> fixture, ITestOutputHelper outputHelper)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _logger = new XUnitTestLogger(outputHelper);
        }

        [EnvDependentFact(Skip = "TODO: fix")]
        public async Task Test_Debot_Msg_Interface()
        {
            var browser = await _fixture.GetDebotBrowserAsync(_logger);

            const int counterBefore = 10;
            const int counterAfter = 15;

            await _fixture.Debot.Client.AssertGetMethodAsync(
                _fixture.Debot.Address,
                _fixture.Debot.Abi,
                "counter",
                new { },
                new
                {
                    counter = counterBefore.ToString()
                });

            await browser.ExecuteAsync(new List<DebotStep>(), new List<string>
            {
                $"counter={counterBefore}",
                "Increment succeeded",
                $"counter={counterAfter}"
            });

            await _fixture.Debot.Client.AssertGetMethodAsync(
                _fixture.Debot.Address,
                _fixture.Debot.Abi,
                "counter",
                new { },
                new
                {
                    counter = counterAfter.ToString()
                });
        }
    }
}
