using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TonSdk.Modules;
using Xunit;
using Xunit.Abstractions;

namespace TonSdk.Tests.Modules
{
    public class DebotModuleTests4 : IClassFixture<DebotFixture<TestDebot4>>
    {
        private readonly DebotFixture<TestDebot4> _fixture;
        private readonly ILogger _logger;

        public DebotModuleTests4(DebotFixture<TestDebot4> fixture, ITestOutputHelper outputHelper)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _logger = new XUnitTestLogger(outputHelper);
        }

        [EnvDependentFact(Skip = "TODO: fix")]
        public async Task Test_Debot4()
        {
            var browser = await _fixture.GetDebotBrowserAsync(_logger);

            var targetBoc = await _fixture.Debot.Client.DownloadAccountAsync(_fixture.Debot.TargetAddr);
            Assert.NotNull(targetBoc);

            var account = await _fixture.Debot.Client.Boc.ParseAccountAsync(new ParamsOfParse
            {
                Boc = targetBoc
            });

            Assert.Equal(0, account.Parsed.Value<int>("acc_type"));

            await browser.ExecuteAsync(new List<DebotStep>(), new List<string>
            {
                "Target contract deployed.",
                "Enter 1",
                "getData",
                "setData(128)",
                "Sign external message:",
                "Transaction succeeded",
                "setData2(129)"
            });

            targetBoc = await _fixture.Debot.Client.DownloadAccountAsync(_fixture.Debot.TargetAddr);
            Assert.NotNull(targetBoc);

            account = await _fixture.Debot.Client.Boc.ParseAccountAsync(new ParamsOfParse
            {
                Boc = targetBoc
            });

            Assert.Equal(1, account.Parsed.Value<int>("acc_type"));

            await _fixture.Debot.Client.AssertGetMethodAsync(
                _fixture.Debot.TargetAddr,
                _fixture.Debot.TargetAbi,
                "getData",
                new
                {
                    key = 1
                }, new
                {
                    num = "0x0000000000000000000000000000000000000000000000000000000000000081"
                });
        }
    }
}
