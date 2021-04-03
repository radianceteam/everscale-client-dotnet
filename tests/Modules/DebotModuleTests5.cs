using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace TonSdk.Tests.Modules
{
    public class DebotModuleTests5 : IClassFixture<DebotFixture<TestDebot5>>
    {
        private readonly DebotFixture<TestDebot5> _fixture;
        private readonly ILogger _logger;

        public DebotModuleTests5(DebotFixture<TestDebot5> fixture, ITestOutputHelper outputHelper)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _logger = new XUnitTestLogger(outputHelper);
        }

        [EnvDependentFact]
        public async Task Test_Debot_Sdk_Get_Accounts_By_Hash()
        {
            var browser = await _fixture.GetDebotBrowserAsync(_logger);
            var count = await _fixture.Debot.Client.CountAccountsByCodeHashAsync(_fixture.Debot.Tvc);
            await browser.ExecuteAsync(new List<DebotStep>(), new List<string>
            {
                $"{count} contracts."
            });
        }
    }
}
