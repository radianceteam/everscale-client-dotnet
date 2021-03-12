using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace TonSdk.Tests.Modules
{
    public class DebotModulePairTests : IClassFixture<DebotFixture<TestDebotPair>>
    {
        private readonly DebotFixture<TestDebotPair> _fixture;
        private readonly ILogger _logger;

        public DebotModulePairTests(DebotFixture<TestDebotPair> fixture, ITestOutputHelper outputHelper)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _logger = new XUnitTestLogger(outputHelper);
        }

        [EnvDependentFact]
        public async Task Test_Debot_Invoke_Msgs()
        {
            var browser = await _fixture.GetDebotBrowserAsync(_logger);
            await browser.ExecuteAsync(new List<DebotStep>(), new List<string>
            {
                "Invoking Debot B",
                "DebotB receives question: What is your name?",
                "DebotA receives answer: My name is DebotB"
            });
        }
    }
}
