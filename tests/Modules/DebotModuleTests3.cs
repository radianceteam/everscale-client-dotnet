using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace TonSdk.Tests.Modules
{
    public class DebotModuleTests3 : IClassFixture<DebotFixture<TestDebot3>>
    {
        private readonly DebotFixture<TestDebot3> _fixture;
        private readonly ILogger _logger;

        public DebotModuleTests3(DebotFixture<TestDebot3> fixture, ITestOutputHelper outputHelper)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _logger = new XUnitTestLogger(outputHelper);
        }

        [EnvDependentFact]
        public async Task Test_Debot_Sdk_Interface()
        {
            var browser = await _fixture.GetDebotBrowserAsync(_logger);
            await browser.ExecuteAsync(new List<DebotStep>(), new List<string>
            {
                "test substring1 passed",
                "test substring2 passed",
                "test mnemonicDeriveSignKeys passed",
                "test genRandom passed",
                "test mnemonic passed",
                "test account passed",
                "test hdkeyXprv passed"
            });
        }
    }
}
