using System;
using Newtonsoft.Json;
using TonSdk.Extensions.NodeSe;
using TonSdk.Modules;

namespace TonSdk.Tests
{
    public class DebotFixture : IDisposable
    {
        public ITonClient Client { get; }

        public string DebotAddr { get; }

        public string TargetAddr { get; }

        public KeyPair Keys { get; }

        public DebotFixture()
        {
            Client = TestClient.Create();

            Keys = Client.Crypto.GenerateRandomSignKeysAsync().Result;

            var (targetAbi, targetTvc) = TestClient.Package(TestClient.TestDebotTarget);
            var (debotAbi, debotTvc) = TestClient.Package(TestClient.TestDebot);

            var serializer = new TonSerializer();

            TargetAddr = Client.DeployWithGiverAsync(new ParamsOfEncodeMessage
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
            }).Result;

            DebotAddr = Client.DeployWithGiverAsync(new ParamsOfEncodeMessage
            {
                Abi = debotAbi,
                DeploySet = new DeploySet
                {
                    Tvc = debotTvc
                },
                Signer = new Signer.Keys
                {
                    KeysProperty = Keys
                },
                CallSet = new CallSet
                {
                    FunctionName = "constructor",
                    Input = new
                    {
                        targetAbi = serializer.Serialize((targetAbi as Abi.Contract).Value).ToHexString(),
                        targetAddr = TargetAddr
                    }.ToJson()
                }
            }).Result;

            Client.NetProcessFunctionAsync(
                DebotAddr,
                debotAbi,
                "setAbi",
                new
                {
                    debotAbi = ((Abi.Contract)debotAbi).Value.ToJson().ToString().ToHexString()
                }.ToJson(), new Signer.None()).Wait();
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}
