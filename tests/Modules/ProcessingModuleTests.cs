using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TonSdk.Extensions.NodeSe;
using TonSdk.Modules;
using Xunit;
using Xunit.Abstractions;

namespace TonSdk.Tests.Modules
{
    public class ProcessingModuleTests : IDisposable
    {
        private readonly ITonClient _client;

        public ProcessingModuleTests(ITestOutputHelper outputHelper)
        {
            _client = TestClient.Create(new XUnitTestLogger(outputHelper));
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        [EnvDependentFact]
        public async Task Should_Wait_For_Message()
        {
            var (abi, tvc) = TestClient.Package("Events");
            var keys = await _client.Crypto.GenerateRandomSignKeysAsync();

            var encoded = await _client.Abi.EncodeMessageAsync(new ParamsOfEncodeMessage
            {
                Abi = abi,
                DeploySet = new DeploySet
                {
                    Tvc = tvc
                },
                CallSet = new CallSet
                {
                    FunctionName = "constructor",
                    Header = new FunctionHeader
                    {
                        Pubkey = keys.Public
                    }
                },
                Signer = new Signer.Keys
                {
                    KeysProperty = keys
                }
            });

            await _client.GetGramsFromGiverAsync(encoded.Address);

            var events = new List<ProcessingEvent>();

            Task ProcessingCallback(ProcessingEvent @event, int code)
            {
                Assert.Equal(100, code);
                Assert.NotNull(@event);
                events.Add(@event);
                return Task.CompletedTask;
            }

            var result = await _client.Processing.SendMessageAsync(new ParamsOfSendMessage
            {
                Message = encoded.Message,
                Abi = abi,
                SendEvents = true
            }, ProcessingCallback);

            try
            {
                var output = await _client.Processing.WaitForTransactionAsync(new ParamsOfWaitForTransaction
                {
                    Message = encoded.Message,
                    ShardBlockId = result.ShardBlockId,
                    SendEvents = true,
                    Abi = abi
                }, ProcessingCallback);

                Assert.NotNull(output);
                Assert.Empty(output.OutMessages);
                Assert.NotNull(output.Decoded.Output);
                Assert.NotNull(output.Decoded.OutMessages);
                Assert.Empty(output.Decoded.OutMessages);
                Assert.Null(output.Decoded.Output);
            }
            catch (TonClientException /* message expired */)
            {
            }

            using var enumerator = events.GetEnumerator();
            Assert.True(enumerator.MoveNext());
            Assert.IsType<ProcessingEvent.WillFetchFirstBlock>(enumerator.Current);
            Assert.True(enumerator.MoveNext());
            Assert.IsType<ProcessingEvent.WillSend>(enumerator.Current);
            Assert.True(enumerator.MoveNext());
            Assert.IsType<ProcessingEvent.DidSend>(enumerator.Current);
            Assert.True(enumerator.MoveNext());
            do
            {
                Assert.IsType<ProcessingEvent.WillFetchNextBlock>(enumerator.Current);
            } while (enumerator.MoveNext());
        }
    }
}
