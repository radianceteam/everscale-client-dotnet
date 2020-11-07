using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TonSdk.Modules;
using Xunit;
using Xunit.Abstractions;

namespace TonSdk.Tests.Modules
{
    public class NetModuleTests : IDisposable
    {
        private readonly ITonClient _client;
        private readonly ILogger _logger;

        public NetModuleTests(ITestOutputHelper outputHelper)
        {
            _logger = new XUnitTestLogger(outputHelper);
            _client = TestClient.Create(_logger);
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        [EnvDependentFact]
        public async Task Should_Query_Collection_Block_Signature()
        {
            var result = await _client.Net.QueryCollectionAsync(new ParamsOfQueryCollection
            {
                Collection = "blocks_signatures",
                Filter = JToken.FromObject(new { }),
                Result = "id",
                Limit = 1
            });

            Assert.NotNull(result);
        }

        [EnvDependentFact]
        public async Task Should_Query_Collection_All_Accounts()
        {
            var result = await _client.Net.QueryCollectionAsync(new ParamsOfQueryCollection
            {
                Collection = "accounts",
                Filter = JToken.FromObject(new { }),
                Result = "id balance"
            });

            Assert.NotNull(result);
            Assert.NotEmpty(result.Result);
        }

        [EnvDependentFact]
        public async Task Should_Query_Collection_Ranges()
        {
            var result = await _client.Net.QueryCollectionAsync(new ParamsOfQueryCollection
            {
                Collection = "messages",
                Filter = JToken.FromObject(new
                {
                    created_at = new
                    {
                        gt = 1562342740
                    }
                }),
                Result = "body created_at"
            });

            Assert.NotNull(result);
            Assert.NotEmpty(result.Result);
            Assert.True(result.Result[0].Value<ulong>("created_at") > 1562342740);
        }

        [EnvDependentFact]
        public async Task Should_Wait_For_Collection()
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var request = TestClient.Create(_logger).Net
                .WaitForCollectionAsync(new ParamsOfWaitForCollection
                {
                    Collection = "transactions",
                    Filter = JObject.FromObject(new
                    {
                        now = new { gt = now }
                    }),
                    Result = "id now"
                });

            var task = _client.GetGramsFromGiverAsync(TestClient.GiverAddress);

            var result = await request;
            Assert.NotNull(result);

            await task;
        }

        [EnvDependentFact]
        public async Task Should_Subscribe_For_Transactions_With_Address()
        {
            var keys = await _client.Crypto.GenerateRandomSignKeysAsync();

            var deployParams = new ParamsOfEncodeMessage
            {
                Abi = TestClient.Abi("Hello"),
                DeploySet = new DeploySet
                {
                    Tvc = TestClient.Tvc("Hello")
                },
                Signer = new Signer.Keys
                {
                    KeysProperty = keys
                },
                CallSet = new CallSet
                {
                    FunctionName = "constructor"
                }
            };

            var msg = await _client.Abi.EncodeMessageAsync(deployParams);
            var address = msg.Address;
            var transactionIds = new List<string>();

            var handle = await _client.Net.SubscribeCollectionAsync(new ParamsOfSubscribeCollection
            {
                Collection = "transactions",
                Filter = JObject.FromObject(new
                {
                    account_addr = new { eq = address },
                    status = new { eq = 3 } // Finalized
                }),
                Result = "id account_addr"
            }, (json, result) =>
            {
                Assert.Equal(100, result);
                Assert.NotNull(json);
                Assert.NotNull(json["result"]);
                Assert.Equal(address, json.SelectToken("result.account_addr"));
                transactionIds.Add((string)json.SelectToken("result.id"));
            });

            await _client.DeployWithGiverAsync(deployParams);

            // give some time for subscription to receive all data
            await Task.Delay(TimeSpan.FromMinutes(1));

            Assert.Equal(2, transactionIds.Count);
            Assert.All(transactionIds, Assert.NotEmpty);
            Assert.NotEqual(transactionIds[0], transactionIds[1]);

            await _client.Net.UnsubscribeAsync(new ResultOfSubscribeCollection
            {
                Handle = handle.Handle
            });
        }

        [EnvDependentFact]
        public async Task Should_Subscribe_For_Messages()
        {
            var messages = new List<string>();

            var handle = await _client.Net.SubscribeCollectionAsync(new ParamsOfSubscribeCollection
            {
                Collection = "messages",
                Filter = JObject.FromObject(new
                {
                    dst = new { eq = "1" }
                }),
                Result = "id"
            }, (json, result) =>
            {
                Assert.Equal(100, result);
                Assert.NotNull(json);
                Assert.NotNull(json["result"]);
                messages.Add(json["result"].ToString());
            });

            await _client.GetGramsFromGiverAsync(TestClient.GiverAddress);

            Assert.Empty(messages);

            await _client.Net.UnsubscribeAsync(new ResultOfSubscribeCollection
            {
                Handle = handle.Handle
            });
        }
    }
}
