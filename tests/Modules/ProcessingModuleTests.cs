using System;
using System.Collections.Generic;
using System.Numerics;
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

        [EnvDependentFact]
        public async Task Test_Fees()
        {
            var (abi, tvc) = TestClient.Package("GiverV2", 2);
            var keys = await _client.Crypto.GenerateRandomSignKeysAsync();

            var address = await _client.DeployWithGiverAsync(new ParamsOfEncodeMessage
            {
                Abi = abi,
                DeploySet = new DeploySet
                {
                    Tvc = tvc
                },
                CallSet = new CallSet
                {
                    FunctionName = "constructor"
                },
                Signer = new Signer.Keys
                {
                    KeysProperty = keys
                }
            });

            var @params = new ParamsOfEncodeMessage
            {
                Abi = abi,
                Address = address,
                CallSet = new CallSet
                {
                    FunctionName = "sendTransaction",
                    Input = new
                    {
                        dest = address,
                        value = 100000000u,
                        bounce = false
                    }.ToJson()
                },
                Signer = new Signer.Keys
                {
                    KeysProperty = keys
                }
            };

            var account = (await _client.FetchAccountAsync(address))["boc"]?.ToString();
            var message = await _client.Abi.EncodeMessageAsync(@params);

            var localResult = await _client.Tvm.RunExecutorAsync(new ParamsOfRunExecutor
            {
                Account = new AccountForExecutor.Account
                {
                    Boc = account
                },
                Message = message.Message
            });

            var runResult = await _client.Processing.ProcessMessageAsync(new ParamsOfProcessMessage
            {
                MessageEncodeParams = @params,
                SendEvents = false
            });

            Assert.Equal(localResult.Fees.GasFee, runResult.Fees.GasFee);
            Assert.Equal(localResult.Fees.OutMsgsFwdFee, runResult.Fees.OutMsgsFwdFee);
            Assert.Equal(localResult.Fees.InMsgFwdFee, runResult.Fees.InMsgFwdFee);
            Assert.Equal(localResult.Fees.TotalOutput, runResult.Fees.TotalOutput);
            Assert.Equal(localResult.Fees.TotalOutput, new BigInteger(100000000u));
            Assert.Equal(localResult.Fees.TotalAccountFees - localResult.Fees.StorageFee,
                localResult.Fees.TotalAccountFees - localResult.Fees.StorageFee);
            Assert.True(runResult.Fees.StorageFee >= localResult.Fees.StorageFee);
            Assert.True(runResult.Fees.OutMsgsFwdFee > 0);
            Assert.True(runResult.Fees.InMsgFwdFee > 0);
            Assert.True(runResult.Fees.TotalAccountFees > 0);
        }
    }
}
