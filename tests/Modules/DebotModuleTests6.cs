using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TonSdk.Modules;
using Xunit;
using Xunit.Abstractions;

namespace TonSdk.Tests.Modules
{
    public class DebotModuleTests6 : IClassFixture<DebotFixture<TestDebot6>>
    {
        private readonly DebotFixture<TestDebot6> _fixture;
        private readonly ILogger _logger;

        public DebotModuleTests6(DebotFixture<TestDebot6> fixture, ITestOutputHelper outputHelper)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _logger = new XUnitTestLogger(outputHelper);
        }

        [EnvDependentFact(Skip = "TODO: fix this test sometimes")]
        public async Task Test_Debot_Json_Approve()
        {
            var browser = await _fixture.GetDebotBrowserAsync(_logger);
            var debotAddress = _fixture.Debot.Address;
            await browser.ExecuteWithDetailsAsync(new List<DebotStep>(),
                new DebotInfo
                {
                    Name = "testDebot6",
                    Version = "0.1.0",
                    Publisher = "TON Labs",
                    Caption = "Test for approve callback and signing handle",
                    Author = "TON Labs",
                    Support = "0:0000000000000000000000000000000000000000000000000000000000000000",
                    Hello = "testDebot6",
                    Language = "en",
                    Dabi = ((Abi.Contract) _fixture.Debot.Abi).Value.ToJson().ToString(),
                    Icon = "",
                    Interfaces = new[]
                    {
                        "0x8796536366ee21852db56dccb60bc564598b618c865fc50c8b1ab740bba128e3",
                        "0xc13024e101c95e71afb1f5fa6d72f633d51e721de0320d73dfd6121a54e4d40a"
                    }
                }, new List<ExpectedTransaction>
                {
                    new()
                    {
                        Dst = debotAddress,
                        Out = new List<Spending>(),
                        Setcode = false,
                        Signkey = _fixture.Debot.Keys.Public,
                        Approved = true
                    },
                    new()
                    {
                        Dst = debotAddress,
                        Out = new List<Spending>
                        {
                            new()
                            {
                                Amount = 10000000000,
                                Dst = debotAddress
                            }
                        },
                        Setcode = false,
                        Signkey = _fixture.Debot.Keys.Public,
                        Approved = false
                    },
                    new()
                    {
                        Dst = debotAddress,
                        Out = new List<Spending>
                        {
                            new()
                            {
                                Amount = 2200000000,
                                Dst = debotAddress
                            },
                            new()
                            {
                                Amount = 3500000000,
                                Dst = "0:0000000000000000000000000000000000000000000000000000000000000000"
                            }
                        },
                        Setcode = false,
                        Signkey = _fixture.Debot.Keys.Public,
                        Approved = true
                    }
                }, new List<string>
                {
                    "Send1 succeeded",
                    "Send2 rejected"
                });
        }
    }
}
