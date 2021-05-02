using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TonSdk.Modules;
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
        public async Task Test_Debot_Inner_Interfaces()
        {
            var browser = await _fixture.GetDebotBrowserAsync(_logger);
            await browser.ExecuteWithDetailsAsync(new List<DebotStep>(),
                new DebotInfo
                {
                    Name = "TestSdk",
                    Version = "0.4.0",
                    Publisher = "TON Labs",
                    Caption = "Test for SDK interface",
                    Author = "TON Labs",
                    Support = "0:0000000000000000000000000000000000000000000000000000000000000000",
                    Hello = "Hello, I'm a test.",
                    Language = "en",
                    Dabi = ((Abi.Contract)_fixture.Debot.Abi).Value.ToJson().ToString(),
                    Icon = "",
                    Interfaces = new string[] {
                        "0x8796536366ee21852db56dccb60bc564598b618c865fc50c8b1ab740bba128e3"
                    }
                },
                new List<ExpectedTransaction>(),
                new List<string>
                {
                    "test substring1 passed",
                    "test substring2 passed",
                    "test mnemonicDeriveSignKeys passed",
                    "test genRandom passed",
                    "test naclbox passed",
                    "test naclKeypairFromSecret passed",
                    "test hex encode passed",
                    "test base64 encode passed",
                    "test mnemonic passed",
                    "test naclboxopen passed",
                    "test account passed",
                    "test hdkeyXprv passed",
                    "test sign hash passed",
                    "test hex decode passed",
                    "test base64 decode passed"
                });
        }
    }
}
