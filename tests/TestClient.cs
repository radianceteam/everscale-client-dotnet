using System;
using System.IO;
using System.Reflection;
using TonSdk.Modules;

namespace TonSdk.Tests
{
    internal static class TestClient
    {
        public const string NodeSeNetworkAddressEnvVar = "TON_NETWORK_ADDRESS";
        public const string AbiVersionEnvVar = "ABI_VERSION";
        public const string TestDebotTarget = "testDebotTarget";
        public const string TestDebot = "testDebot";

        public static ITonClient Create(ILogger logger = null)
        {
            var serverAddress = NodeSeNetworkAddress;
            return TonClient.Create(!string.IsNullOrEmpty(serverAddress)
            ? new ClientConfig
            {
                Network = new NetworkConfig
                {
                    ServerAddress = serverAddress
                }
            } : null, logger);
        }

        public static Tuple<Abi, string> Package(string name, int? version = TonClient.DefaultAbiVersion)
        {
            return new Tuple<Abi, string>(
                Abi(name, version),
                Tvc(name, version));
        }

        public static Abi Abi(string name, int? version = TonClient.DefaultAbiVersion)
        {
            return TonUtil.LoadAbi(GetFilePath($"contracts/abi_v{version}/{name}.abi.json"));
        }

        public static string Tvc(string name, int? version = TonClient.DefaultAbiVersion)
        {
            return TonUtil.LoadTvc(GetFilePath($"contracts/abi_v{version}/{name}.tvc"));
        }

        public static Abi GiverAbi => UseNodeSe
            ? Abi("Giver", 1)
            : Abi("GiverWallet");

        public static string NodeSeNetworkAddress =>
            Environment.GetEnvironmentVariable(NodeSeNetworkAddressEnvVar);

        public static bool UseNodeSe => !string.IsNullOrEmpty(NodeSeNetworkAddress);

        public static int AbiVersion => int.Parse(Environment.GetEnvironmentVariable(AbiVersionEnvVar) ?? TonClient.DefaultAbiVersion.ToString());

        public static string GiverAddress => UseNodeSe
            ? "0:841288ed3b55d9cdafa806807f02a0ae0c169aa5edfe88a789a6482429756a94"
            : "0:2bb4a0e8391e7ea8877f4825064924bd41ce110fce97e939d3323999e1efbb13";

        private static string GetFilePath(string fileName)
        {
            var codeBaseUrl = new Uri(typeof(TestClient).GetTypeInfo().Assembly.CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            var dirPath = Path.GetDirectoryName(codeBasePath);
            return Path.Combine(dirPath, fileName);
        }
    }
}
