using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace TonSdk.Tests.Modules
{
    public class DebotModuleTests : IClassFixture<DebotFixture<TestDebot>>
    {
        private readonly DebotFixture<TestDebot> _fixture;
        private readonly ILogger _logger;

        public DebotModuleTests(DebotFixture<TestDebot> fixture, ITestOutputHelper outputHelper)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _logger = new XUnitTestLogger(outputHelper);
        }

        [EnvDependentFact]
        public async Task Test_Debot_Goto()
        {
            var browser = await _fixture.GetDebotBrowserAsync(_logger);
            await browser.ExecuteAsync(new List<DebotStep>
                {
                    new DebotStep { Choice = 1, Outputs = new List<string> { "Test Goto Action" } },
                    new DebotStep { Choice = 1, Outputs = new List<string> { "Debot Tests" } },
                    new DebotStep { Choice = TestDebotBrowser.ExitChoice, Outputs = new List<string>() }
                });
        }

        [EnvDependentFact]
        public async Task Test_Debot_Print()
        {
            var browser = await _fixture.GetDebotBrowserAsync(_logger);
            await browser.ExecuteAsync(new List<DebotStep>
                {
                    new DebotStep { Choice = 2, Outputs = new List<string> { "Test Print Action", "test2: instant print", "test instant print" } },
                    new DebotStep { Choice = 1, Outputs = new List<string> { "test simple print" } },
                    new DebotStep { Choice = 2, Outputs = new List<string> { $"integer=1,addr={_fixture.Debot.TargetAddr},string=test_string_1" } },
                    new DebotStep { Choice = 3, Outputs = new List<string> { "Debot Tests" } },
                    new DebotStep { Choice = TestDebotBrowser.ExitChoice }
                });
        }

        [EnvDependentFact]
        public async Task Test_Debot_RunAct()
        {
            var browser = await _fixture.GetDebotBrowserAsync(_logger);
            await browser.ExecuteAsync(new List<DebotStep>
                {
                    new DebotStep { Choice = 3, Outputs = new List<string> { "Test Run Action" } },
                    new DebotStep { Choice = 1, Inputs = new List<string>{ "-1:1111111111111111111111111111111111111111111111111111111111111111" }, Outputs = new List<string> { "Test Instant Run", "test1: instant run 1", "test2: instant run 2" } },
                    new DebotStep { Choice = 1, Outputs = new List<string> { "Test Run Action" } },
                    new DebotStep { Choice = 2, Inputs = new List<string> { "hello" } },
                    new DebotStep { Choice = 3, Outputs = new List<string> { "integer=2,addr=-1:1111111111111111111111111111111111111111111111111111111111111111,string=hello" } },
                    new DebotStep { Choice = 4, Outputs = new List<string> { "Debot Tests" } },
                    new DebotStep { Choice = TestDebotBrowser.ExitChoice }
                });
        }

        [EnvDependentFact]
        public async Task Test_Debot_Run_Method()
        {
            var browser = await _fixture.GetDebotBrowserAsync(_logger);
            await browser.ExecuteAsync(new List<DebotStep>
                {
                    new DebotStep { Choice = 4, Outputs = new List<string>{ "Test Run Method Action" } },
                    new DebotStep { Choice = 1 },
                    new DebotStep { Choice = 2, Outputs = new List<string> { "data=64" } },
                    new DebotStep { Choice = 3, Outputs = new List<string> { "Debot Tests" } },
                    new DebotStep { Choice = TestDebotBrowser.ExitChoice }
                });
        }

        [EnvDependentFact]
        public async Task Test_Debot_Send_Msg()
        {
            var browser = await _fixture.GetDebotBrowserAsync(_logger);
            await browser.ExecuteAsync(new List<DebotStep>
                {
                    new DebotStep { Choice = 5, Outputs = new List<string>{ "Test Send Msg Action" } },
                    new DebotStep { Choice = 1, Outputs = new List<string> { "Sending message {}", "Transaction succeeded." } },
                    new DebotStep { Choice = 2 },
                    new DebotStep { Choice = 3, Outputs = new List<string> { "data=100" } },
                    new DebotStep { Choice = 4, Outputs = new List<string> { "Debot Tests" } },
                    new DebotStep { Choice = TestDebotBrowser.ExitChoice }
                });
        }

        [EnvDependentFact]
        public async Task Test_Debot_Invoke_Debot()
        {
            var browser = await _fixture.GetDebotBrowserAsync(_logger);
            await browser.ExecuteAsync(new List<DebotStep>
                {
                    new DebotStep { Choice = 6, Inputs = new List<string>{ _fixture.Debot.Address }, Outputs = new List<string>{ "Test Invoke Debot Action", "enter debot address:" } },
                    new DebotStep { Choice = 1, Inputs = new List<string>{ _fixture.Debot.Address }, Outputs = new List<string>{ "Test Invoke Debot Action", "enter debot address:" },
                        Invokes = new List<List<DebotStep>> { new List<DebotStep>
                        {
                            new DebotStep
                            {
                                Choice = 1,
                                Outputs = new List<string>{ "Print test string", "Debot is invoked" }
                            },
                            new DebotStep
                            {
                                Choice = 1,
                                Outputs = new List<string>{ "Sending message {}", "Transaction succeeded." }
                            }
                        }
                        } },
                    new DebotStep { Choice = 2, Outputs = new List<string> { "Debot Tests" } },
                    new DebotStep { Choice = TestDebotBrowser.ExitChoice }
                });
        }

        [EnvDependentFact]
        public async Task Test_Debot_Engine_Calls()
        {
            var browser = await _fixture.GetDebotBrowserAsync(_logger);
            await browser.ExecuteAsync(new List<DebotStep>
                {
                    new DebotStep { Choice = 7, Outputs = new List<string>{ "Test Engine Calls" } },
                    new DebotStep { Choice = 1 },
                    new DebotStep { Choice = 2 },
                    new DebotStep { Choice = 3 },
                    new DebotStep { Choice = 4 },
                    new DebotStep { Choice = 5 },
                    new DebotStep { Choice = 6, Outputs = new List<string>{ "Debot Tests" } },
                    new DebotStep { Choice = TestDebotBrowser.ExitChoice }
                });
        }

        [EnvDependentFact]
        public async Task Test_Debot_Interface_Call()
        {
            var browser = await _fixture.GetDebotBrowserAsync(_logger);
            await browser.ExecuteAsync(new List<DebotStep>
                {
                    new DebotStep { Choice = 8, Outputs = new List<string>{ "", "test1 - call interface" } },
                    new DebotStep { Choice = 1, Outputs = new List<string>{ "Debot Tests" } },
                    new DebotStep { Choice = TestDebotBrowser.ExitChoice }
                });
        }
    }
}
