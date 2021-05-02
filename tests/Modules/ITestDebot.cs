using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TonSdk.Extensions.NodeSe;
using TonSdk.Modules;

namespace TonSdk.Tests.Modules
{
    public interface ITestDebot : IDisposable
    {
        string Name { get; }

        ITonClient Client { get; }

        Abi Abi { get; }

        string Tvc { get; }

        string Address { get; }

        KeyPair Keys { get; }

        Task InitAsync(ILogger logger);
    }

    public abstract class AbstractTestDebot : ITestDebot
    {
        public const string Target = "testDebotTarget";

        public string TargetAddr { get; protected set; }

        public Abi TargetAbi { get; protected set; }

        public ITonClient Client { get; private set; }

        public KeyPair Keys { get; private set; }

        public Abi Abi { get; private set; }

        public string Tvc { get; private set; }

        public string Address { get; private set; }

        public abstract string Name { get; }

        public virtual async Task InitAsync(ILogger logger)
        {
            Client = TestClient.Create(logger);

            Keys = await Client.Crypto.GenerateRandomSignKeysAsync();

            (Abi, Tvc) = TestClient.Package(Name);

            Address = await Client.DeployWithGiverAsync(new ParamsOfEncodeMessage
            {
                Abi = Abi,
                DeploySet = new DeploySet
                {
                    Tvc = Tvc
                },
                Signer = new Signer.Keys
                {
                    KeysProperty = Keys
                },
                CallSet = new CallSet
                {
                    FunctionName = "constructor",
                    Input = await GetConstructorParamsAsync()
                }
            });

            await SetAbiAsync();
        }

        protected virtual Task<JToken> GetConstructorParamsAsync()
        {
            return Task.FromResult(new { }.ToJson());
        }

        protected virtual async Task SetAbiAsync()
        {
            await Client.NetProcessFunctionAsync(
                Address,
                Abi,
                "setAbi",
                new
                {
                    debotAbi = ((Abi.Contract)Abi).Value.ToJson().ToString().ToHexString()
                }.ToJson(),
                new Signer.Keys
                {
                    KeysProperty = Keys
                });
        }

        protected async Task<JToken> DeployTargetAbiAsync()
        {
            var (targetAbi, targetTvc) = TestClient.Package(Target);

            TargetAddr = await Client.DeployWithGiverAsync(new ParamsOfEncodeMessage
            {
                Abi = targetAbi,
                DeploySet = new DeploySet
                {
                    Tvc = targetTvc
                },
                Signer = new Signer.Keys
                {
                    KeysProperty = Keys
                },
                CallSet = new CallSet
                {
                    FunctionName = "constructor"
                }
            });

            TargetAbi = targetAbi;

            var serializer = new TonSerializer();

            return new
            {
                targetAbi = serializer.Serialize((targetAbi as Abi.Contract).Value).ToHexString(),
                targetAddr = TargetAddr
            }.ToJson();
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }

    public class TestDebot : AbstractTestDebot
    {
        public override string Name { get; } = "testDebot";

        protected override async Task<JToken> GetConstructorParamsAsync()
        {
            return await DeployTargetAbiAsync();
        }
    }

    public class TestDebot2 : AbstractTestDebot
    {
        public override string Name { get; } = "testDebot2";

        protected override Task<JToken> GetConstructorParamsAsync()
        {
            return Task.FromResult(new
            {
                pub = $"0x{Keys.Public}",
                sec = $"0x{Keys.Secret}"
            }.ToJson());
        }
    }

    public class TestDebot3 : AbstractTestDebot
    {
        public override string Name { get; } = "testDebot3";

        protected override async Task SetAbiAsync()
        {
            await Client.NetProcessFunctionAsync(
                Address,
                Abi,
                "setABI",
                new
                {
                    dabi = ((Abi.Contract)Abi).Value.ToJson().ToString().ToHexString()
                }.ToJson(),
                new Signer.Keys
                {
                    KeysProperty = Keys
                });
        }
    }

    public class TestDebot4 : AbstractTestDebot
    {
        public override string Name { get; } = "testDebot4";

        protected override async Task<JToken> GetConstructorParamsAsync()
        {
            var (targetAbi, targetTvc) = TestClient.Package(Target);

            TargetAddr = (await Client.Abi.EncodeMessageAsync(new ParamsOfEncodeMessage
            {
                Abi = targetAbi,
                DeploySet = new DeploySet
                {
                    Tvc = targetTvc
                },
                Signer = new Signer.Keys
                {
                    KeysProperty = Keys
                },
                CallSet = new CallSet
                {
                    FunctionName = "constructor"
                }
            })).Address;

            TargetAbi = targetAbi;

            await Client.GetGramsFromGiverAsync(TargetAddr);

            var serializer = new TonSerializer();

            return new
            {
                targetAbi = serializer.Serialize((targetAbi as Abi.Contract).Value).ToHexString(),
                targetAddr = TargetAddr
            }.ToJson();
        }

        protected override async Task SetAbiAsync()
        {
            await base.SetAbiAsync();

            await Client.NetProcessFunctionAsync(Address, Abi, "setImage", new
            {
                image = TestClient.Tvc(Target),
                pubkey = $"0x{Keys.Public}"
            }.ToJson(), new Signer.Keys { KeysProperty = Keys });
        }
    }

    public class TestDebot5 : AbstractTestDebot
    {
        public override string Name { get; } = "testDebot5";

        public int Count { get; } = 6;

        protected override async Task SetAbiAsync()
        {
            await Client.NetProcessFunctionAsync(
                Address,
                Abi,
                "setABI",
                new
                {
                    dabi = ((Abi.Contract)Abi).Value.ToJson().ToString().ToHexString()
                }.ToJson(),
                new Signer.Keys
                {
                    KeysProperty = Keys
                });

            var deployDebotParams = new ParamsOfEncodeMessage
            {
                Abi = Abi,
                DeploySet = new DeploySet
                {
                    Tvc = Tvc
                },
                CallSet = new CallSet
                {
                    FunctionName = "constructor",
                    Input = await GetConstructorParamsAsync()
                }
            };

            for (var i = 1; i < Count; ++i)
            {
                var keys = await Client.Crypto.GenerateRandomSignKeysAsync();
                deployDebotParams.Signer = new Signer.Keys
                {
                    KeysProperty = keys
                };

                await Client.DeployWithGiverAsync(deployDebotParams, 1_000_000_000);
            }
        }

        protected override async Task<JToken> GetConstructorParamsAsync()
        {
            var hash = await Client.GetCodeHashFromTvcAsync(Tvc);
            return new { codeHash = $"0x{hash}" }.ToJson();
        }
    }

    public class TestDebotPair : AbstractTestDebot
    {
        public override string Name { get; } = "tda";

        private ParamsOfEncodeMessage _targetDeployParams;

        protected override async Task<JToken> GetConstructorParamsAsync()
        {
            var (targetAbi, targetTvc) = TestClient.Package("tdb");

            _targetDeployParams = new ParamsOfEncodeMessage
            {
                Abi = targetAbi,
                DeploySet = new DeploySet
                {
                    Tvc = targetTvc
                },
                Signer = new Signer.Keys
                {
                    KeysProperty = Keys
                },
                CallSet = new CallSet
                {
                    FunctionName = "constructor"
                }
            };

            TargetAddr = (await Client.Abi.EncodeMessageAsync(_targetDeployParams)).Address;
            TargetAbi = targetAbi;

            await Client.GetGramsFromGiverAsync(TargetAddr);

            return new
            {
                targetAddr = TargetAddr
            }.ToJson();
        }

        protected override async Task SetAbiAsync()
        {
            await base.SetAbiAsync();

            await Client.DeployWithGiverAsync(_targetDeployParams, 1_000_000_000);

            await Client.NetProcessFunctionAsync(
                TargetAddr,
                TargetAbi,
                "setAbi",
                new
                {
                    debotAbi = ((Abi.Contract)TargetAbi).Value.ToJson().ToString().ToHexString()
                }.ToJson(),
                new Signer.Keys
                {
                    KeysProperty = Keys
                });
        }
    }

    public class TestDebot7 : AbstractTestDebot
    {
        public override string Name { get; } = "testDebot7";

        protected override async Task SetAbiAsync()
        {
            await Client.NetProcessFunctionAsync(
                Address,
                Abi,
                "setABI",
                new
                {
                    dabi = ((Abi.Contract)Abi).Value.ToJson().ToString().ToHexString()
                }.ToJson(),
                new Signer.Keys
                {
                    KeysProperty = Keys
                });
        }
    }
    public class TestDebot8 : AbstractTestDebot
    {
        public override string Name { get; } = "testDebot8";

        protected override async Task SetAbiAsync()
        {
            await Client.NetProcessFunctionAsync(
                Address,
                Abi,
                "setABI",
                new
                {
                    dabi = ((Abi.Contract)Abi).Value.ToJson().ToString().ToHexString()
                }.ToJson(),
                new Signer.Keys
                {
                    KeysProperty = Keys
                });
        }
    }
}
