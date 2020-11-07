using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using TonSdk.Modules;

namespace TonSdk.Tests
{
    internal static class TestClient
    {
        public const string NodeSeNetworkAddressEnvVar = "TON_NETWORK_ADDRESS";
        public const string AbiVersionEnvVar = "ABI_VERSION";
        public const int DefaultAbiVersion = 2;

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

        public static Tuple<Abi, string> Package(string name, int? version = DefaultAbiVersion)
        {
            return new Tuple<Abi, string>(
                Abi(name, version),
                Tvc(name, version));
        }

        public static Abi Abi(string name, int? version = DefaultAbiVersion)
        {
            using var stream = TestFiles.OpenStream($"contracts/abi_v{version}/{name}.abi.json");
            using var reader = new StreamReader(stream, Encoding.UTF8);
            return new Abi.Contract
            {
                Value = new JsonSerializer().Deserialize<AbiContract>(new JsonTextReader(reader))
            };
        }

        public static string Tvc(string name, int? version = DefaultAbiVersion)
        {
            using var stream = TestFiles.OpenStream($"contracts/abi_v{version}/{name}.tvc");
            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public static Abi GiverAbi => UseNodeSe
            ? Abi("Giver", 1)
            : Abi("GiverWallet");

        public static string NodeSeNetworkAddress =>
            Environment.GetEnvironmentVariable(NodeSeNetworkAddressEnvVar);

        public static bool UseNodeSe => !string.IsNullOrEmpty(NodeSeNetworkAddress);

        public static int AbiVersion => int.Parse(Environment.GetEnvironmentVariable(AbiVersionEnvVar) ?? DefaultAbiVersion.ToString());

        public static string GiverAddress => UseNodeSe
            ? "0:841288ed3b55d9cdafa806807f02a0ae0c169aa5edfe88a789a6482429756a94"
            : "0:2bb4a0e8391e7ea8877f4825064924bd41ce110fce97e939d3323999e1efbb13";
    }
}
