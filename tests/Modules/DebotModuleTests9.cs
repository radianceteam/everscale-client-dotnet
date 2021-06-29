using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TonSdk.Modules;
using Xunit;
using Xunit.Abstractions;

namespace TonSdk.Tests.Modules
{
    public class DebotModuleTests9 : IClassFixture<DebotFixture<TestDebot9>>
    {
        private readonly DebotFixture<TestDebot9> _fixture;
        private readonly ILogger _logger;

        public DebotModuleTests9(DebotFixture<TestDebot9> fixture, ITestOutputHelper outputHelper)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _logger = new XUnitTestLogger(outputHelper);
        }

        [EnvDependentFact(Skip = "No time for fixing it")]
        public async Task Test_Debot_Transaction_Chain()
        {
            var browser = await _fixture.GetDebotBrowserAsync(_logger);
            await browser.ExecuteWithDetailsAsync(new List<DebotStep>(),
                new DebotInfo
                {
                    Name = "TestDeBot9",
                    Version = "0.1.0",
                    Publisher = "TON Labs",
                    Caption = "TestDeBot9",
                    Author = "TON Labs",
                    Support = "0:0000000000000000000000000000000000000000000000000000000000000000",
                    Hello = "TestDeBot9",
                    Language = "en",
                    Dabi = ((Abi.Contract) _fixture.Debot.Abi).Value.ToJson().ToString(),
                    Icon = "",
                    Interfaces = new[]
                    {
                        "0x8796536366ee21852db56dccb60bc564598b618c865fc50c8b1ab740bba128e3"
                    }
                }, new List<ExpectedTransaction>(),
                new List<string> {"Test passed"});
        }
    }
}
