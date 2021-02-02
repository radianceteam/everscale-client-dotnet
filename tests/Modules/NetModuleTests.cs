using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TonSdk.Extensions.NodeSe;
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
                Filter = new { }.ToJson(),
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
                Filter = new { }.ToJson(),
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
                Filter = new
                {
                    created_at = new
                    {
                        gt = 1562342740
                    }
                }.ToJson(),
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
                    Filter = new
                    {
                        now = new { gt = now }
                    }.ToJson(),
                    Result = "id now"
                });

            var task = _client.GetGramsFromGiverAsync(TonClientNodeSe.GiverAddress);

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
                Filter = new
                {
                    account_addr = new { eq = address },
                    status = new { eq = 3 } // Finalized
                }.ToJson(),
                Result = "id account_addr"
            }, (json, result) =>
            {
                Assert.Equal(100, result);
                Assert.NotNull(json);
                Assert.NotNull(json["result"]);
                Assert.Equal(address, json.SelectToken("result.account_addr"));
                transactionIds.Add((string)json.SelectToken("result.id"));
                return Task.CompletedTask;
            });

            await _client.DeployWithGiverAsync(deployParams);

            // give some time for subscription to receive all data
            await Task.Delay(TimeSpan.FromSeconds(1));
            Assert.Equal(2, transactionIds.Distinct().Count());

            await _client.Net.UnsubscribeAsync(handle);
        }

        [EnvDependentFact]
        public async Task Should_Subscribe_For_Messages()
        {
            var messages = new List<string>();

            var handle = await _client.Net.SubscribeCollectionAsync(new ParamsOfSubscribeCollection
            {
                Collection = "messages",
                Filter = new
                {
                    dst = new { eq = "1" }
                }.ToJson(),
                Result = "id"
            }, (json, result) =>
            {
                Assert.Equal(100, result);
                Assert.NotNull(json);
                Assert.NotNull(json["result"]);
                messages.Add(json["result"].ToString());
                return Task.CompletedTask;
            });

            await _client.GetGramsFromGiverAsync(TonClientNodeSe.GiverAddress);

            Assert.Empty(messages);

            await _client.Net.UnsubscribeAsync(handle);
        }

        [EnvDependentFact]
        public async Task Should_Run_Query()
        {
            var info = await _client.Net.QueryAsync(new ParamsOfQuery
            {
                Query = "query{info{version}}"
            });

            Assert.NotNull(info);
            Assert.NotNull(info.Result);

            var version = info.Result["data"]?["info"]?["version"]?.ToString();
            Assert.NotNull(version);
            Assert.NotEmpty(version);
            Assert.Equal(3, version.Split(".").Length);
        }

        [EnvDependentFact]
        public async Task Should_Suspend_Resume()
        {
            var keys = await _client.Crypto.GenerateRandomSignKeysAsync();
            var (abi, tvc) = TestClient.Package("Hello");

            var deployParams = new ParamsOfEncodeMessage
            {
                Abi = abi,
                DeploySet = new DeploySet
                {
                    Tvc = tvc
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
            var notifications = new List<ClientError>();

            var subscriptionClient = TestClient.Create(_logger);

            var handle = await subscriptionClient.Net.SubscribeCollectionAsync(new ParamsOfSubscribeCollection
            {
                Collection = "transactions",
                Filter = new
                {
                    account_addr = new { eq = address },
                    status = new { eq = 3 } // Finalized
                }.ToJson(),
                Result = "id account_addr"
            }, (json, result) =>
            {
                switch (result)
                {
                    case 100: // OK
                        transactionIds.Add((string)json.SelectToken("result.id"));
                        break;

                    case 101: // Error
                        var clientError = new TonSerializer(_logger).Deserialize<ClientError>(json);
                        notifications.Add(clientError);
                        break;

                    default:
                        throw new NotSupportedException($"Response code ${result} not supported");
                }

                return Task.CompletedTask;
            });

            // send grams to create first transaction
            await _client.GetGramsFromGiverAsync(msg.Address);
            await Task.Delay(TimeSpan.FromSeconds(1));

            // check that transaction is received
            Assert.Single(transactionIds);

            // and no error notifications
            Assert.Empty(notifications);

            // suspend subscription
            await subscriptionClient.Net.SuspendAsync();

            // deploy to create second transaction
            await _client.Processing.ProcessMessageAsync(new ParamsOfProcessMessage
            {
                MessageEncodeParams = deployParams,
                SendEvents = false
            });

            // create second subscription while network is suspended
            var handle2 = await subscriptionClient.Net.SubscribeCollectionAsync(new ParamsOfSubscribeCollection
            {
                Collection = "transactions",
                Filter = new
                {
                    account_addr = new { eq = msg.Address },
                    status = new { eq = 3 } // Finalized
                }.ToJson(),
                Result = "id account_addr"
            }, (json, result) =>
            {
                switch (result)
                {
                    case 100: // OK
                        transactionIds.Add((string)json.SelectToken("result.id"));
                        break;

                    case 101: // Error
                        var clientError = new TonSerializer(_logger).Deserialize<ClientError>(json);
                        notifications.Add(clientError);
                        break;

                    default:
                        throw new NotSupportedException($"Response code ${result} not supported");
                }

                return Task.CompletedTask;
            });

            await Task.Delay(TimeSpan.FromMilliseconds(500));

            // check that second transaction is not received when subscription suspended
            Assert.Single(transactionIds);
            Assert.Equal(2, notifications.Count);
            Assert.All(notifications, n =>
                Assert.Equal((uint)NetErrorCode.NetworkModuleSuspended, n.Code));

            // resume subscription
            await subscriptionClient.Net.ResumeAsync();

            // run contract function to create new transaction
            await subscriptionClient.Processing.ProcessMessageAsync(new ParamsOfProcessMessage
            {
                MessageEncodeParams = new ParamsOfEncodeMessage
                {
                    Abi = abi,
                    Signer = new Signer.Keys
                    {
                        KeysProperty = keys
                    },
                    Address = msg.Address,
                    CallSet = new CallSet
                    {
                        FunctionName = "touch"
                    }
                },
                SendEvents = false
            });

            // give some time for subscription to receive all data
            await Task.Delay(TimeSpan.FromSeconds(10));

            // check that third transaction is now received after resume
            Assert.Equal(3, transactionIds.Count);
            Assert.Equal(2, transactionIds.Distinct().Count());

            // and both subscriptions received notification about resume
            Assert.Equal(4, notifications.Count);
            Assert.Equal(2, notifications.Count(n => n.Code == (uint)NetErrorCode.NetworkModuleSuspended));
            Assert.Equal(2, notifications.Count(n => n.Code == (uint)NetErrorCode.NetworkModuleResumed));

            await subscriptionClient.Net.UnsubscribeAsync(handle);
            await subscriptionClient.Net.UnsubscribeAsync(handle2);
        }

        [EnvDependentFact]
        public async Task Should_Find_Last_Shard_Block()
        {
            var block = await _client.Net.FindLastShardBlockAsync(new ParamsOfFindLastShardBlock
            {
                Address = TonClientNodeSe.GiverAddress
            });

            Assert.NotNull(block);
            Assert.NotEmpty(block.BlockId);
        }

        [EnvDependentFact]
        public async Task Should_Fetch_Endpoints()
        {
            var client = TonClient.Create(new ClientConfig
            {
                Network = new NetworkConfig
                {
                    Endpoints = new[] { "cinet.tonlabs.io", "cinet2.tonlabs.io/" }
                }
            });

            var result = await client.Net.FetchEndpointsAsync();
            Assert.NotNull(result);
            Assert.Equal(2, result.Endpoints.Length);
            Assert.Contains("https://cinet.tonlabs.io/", result.Endpoints);
            Assert.Contains("https://cinet2.tonlabs.io/", result.Endpoints);
        }

        [EnvDependentFact]
        public async Task Should_Set_Endpoints()
        {
            var client = TestClient.Create();

            await client.Net.SetEndpointsAsync(new EndpointsSet
            {
                Endpoints = new[] { "cinet.tonlabs.io", "cinet2.tonlabs.io/" }
            });

            var result = await client.Net.FetchEndpointsAsync();
            Assert.NotNull(result);
            Assert.Equal(2, result.Endpoints.Length);
            Assert.Contains("https://cinet.tonlabs.io/", result.Endpoints);
            Assert.Contains("https://cinet2.tonlabs.io/", result.Endpoints);
        }

        [EnvDependentFact]
        public async Task Should_Call_Batch_Query()
        {
            var batch = await _client.Net.BatchQueryAsync(new ParamsOfBatchQuery
            {
                Operations = new ParamsOfQueryOperation[]
                {
                    new ParamsOfQueryOperation.QueryCollection
                    {
                        Collection = "blocks_signatures",
                        Result = "id",
                        Limit = 1
                    },
                    new ParamsOfQueryOperation.AggregateCollection
                    {
                        Collection = "accounts",
                        Fields = new[]
                        {
                            new FieldAggregation
                            {
                                Field = "",
                                Fn = AggregationFn.COUNT
                            }
                        }
                    },
                    new ParamsOfQueryOperation.WaitForCollection
                    {
                        Collection = "transactions",
                        Filter = new
                        {
                            now = new
                            {
                                gt = 20
                            }
                        }.ToJson(),
                        Result = "id now"
                    }
                }
            });

            Assert.Equal(3, batch.Results.Length);
        }

        [EnvDependentFact]
        public async Task Should_Call_Aggregate_Collection()
        {
            var result = await _client.Net.AggregateCollectionAsync(new ParamsOfAggregateCollection
            {
                Collection = "accounts",
                Fields = new[]
                {
                    new FieldAggregation
                    {
                        Field = "",
                        Fn = AggregationFn.COUNT
                    }
                }
            });

            Assert.NotNull(result);
            Assert.NotNull(result.Values);
            Assert.NotEmpty(result.Values);
            Assert.NotNull(result.Values[0]);

            var count = result.Values[0].Value<int>();
            Assert.True(count > 0);
        }
    }
}
