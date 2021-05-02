using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TonSdk.Modules;
using Xunit;
using Xunit.Abstractions;

namespace TonSdk.Tests.Modules
{
    public class DebotModuleTests7 : IClassFixture<DebotFixture<TestDebot7>>
    {
        private readonly DebotFixture<TestDebot7> _fixture;
        private readonly ILogger _logger;

        public DebotModuleTests7(DebotFixture<TestDebot7> fixture, ITestOutputHelper outputHelper)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _logger = new XUnitTestLogger(outputHelper);
        }

        [EnvDependentFact]
        public async Task Test_Debot_Json_Interface()
        {
            var browser = await _fixture.GetDebotBrowserAsync(_logger);
            await browser.ExecuteWithDetailsAsync(new List<DebotStep>(),
                new DebotInfo
                {
                    Name = "Test DeBot 7",
                    Version = "0.1.0",
                    Publisher = "TON Labs",
                    Caption = "Test for Json interface",
                    Author = "TON Labs",
                    Support = "0:0000000000000000000000000000000000000000000000000000000000000000",
                    Hello = "Test DeBot 7",
                    Language = "en",
                    Dabi = ((Abi.Contract)_fixture.Debot.Abi).Value.ToJson().ToString(),
                    Icon = "",
                    Interfaces = new string[] {
                        "0x8796536366ee21852db56dccb60bc564598b618c865fc50c8b1ab740bba128e3",
                        "0x442288826041d564ccedc579674f17c1b0a3452df799656a9167a41ab270ec19"
                    }
                }, new List<ExpectedTransaction>(), new List<string>());
        }
    }
}
