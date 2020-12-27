using System;
using TonSdk.Modules;

namespace TonSdk.Extensions.NodeSe
{
    public static class TonClientNodeSe
    {
        public const string NodeSeNetworkAddressEnvVar = "TON_NETWORK_ADDRESS";

        public const string GiverAddress = "0:841288ed3b55d9cdafa806807f02a0ae0c169aa5edfe88a789a6482429756a94";

        public static string NodeSeNetworkAddress =>
            Environment.GetEnvironmentVariable(NodeSeNetworkAddressEnvVar);

        public static ITonClient Create(ILogger logger = null)
        {
            return TonClient.Create(new ClientConfig
            {
                Network = new NetworkConfig
                {
                    ServerAddress = NodeSeNetworkAddress
                }
            }, logger);
        }
    }
}
