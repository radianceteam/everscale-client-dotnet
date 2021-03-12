using System;
using System.Threading.Tasks;
using TonSdk.Extensions.NodeSe;
using TonSdk.Modules;

namespace TonSdk.Tests
{
    internal static class TestClient
    {
        public const string AbiVersionEnvVar = "ABI_VERSION";

        public static ITonClient Create(ILogger logger = null)
        {
            var serverAddress = TonClientNodeSe.NodeSeNetworkAddress;
            return !string.IsNullOrEmpty(serverAddress)
                ? TonClientNodeSe.Create(logger)
                : TonClient.Create(logger);
        }

        public static Tuple<Abi, string> Package(string name, int? version = TonClient.DefaultAbiVersion)
        {
            return new Tuple<Abi, string>(
                Abi(name, version),
                Tvc(name, version));
        }

        public static Abi Abi(string name, int? version = TonClient.DefaultAbiVersion)
        {
            return TonUtil.LoadAbi($"contracts/abi_v{version}/{name}.abi.json");
        }

        public static string Tvc(string name, int? version = TonClient.DefaultAbiVersion)
        {
            return TonUtil.LoadTvc($"contracts/abi_v{version}/{name}.tvc");
        }

        public static int AbiVersion => int.Parse(Environment.GetEnvironmentVariable(AbiVersionEnvVar) ?? TonClient.DefaultAbiVersion.ToString());

        public static async Task<KeyPair> GenerateSignKeysAsync()
        {
            using var client = TestClient.Create();
            return await client.Crypto.GenerateRandomSignKeysAsync();
        }
    }
}
