using Newtonsoft.Json;
using System.Threading.Tasks;
using Serilog;
using TonClient.Examples.Lib;
using TonSdk;
using TonSdk.Extensions.NodeSe;
using TonSdk.Modules;

namespace TonClient.DebotExample
{
    public class Program
    {
        public const string DebotTarget = "testDebotTarget";
        public const string Debot = "testDebot";

        public static void Main(string[] _)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("debot.log")
                .CreateLogger();

            RunDebotAsync().Wait();
        }

        private static async Task RunDebotAsync()
        {
            using var client = TonClientNodeSe.Create(new SerilogLogger());

            var targetAbi = TonUtil.LoadAbi($"contracts/{DebotTarget}.abi.json");
            var targetTvc = TonUtil.LoadTvc($"contracts/{DebotTarget}.tvc");
            var debotAbi = TonUtil.LoadAbi($"contracts/{Debot}.abi.json");
            var debotTvc = TonUtil.LoadTvc($"contracts/{Debot}.tvc");

            var keys = await client.Crypto.GenerateRandomSignKeysAsync();

            var targetAddr = await client.DeployWithGiverAsync(new ParamsOfEncodeMessage
            {
                Abi = targetAbi,
                DeploySet = new DeploySet
                {
                    Tvc = targetTvc
                },
                Signer = new Signer.Keys
                {
                    KeysProperty = keys
                },
                CallSet = new CallSet
                {
                    FunctionName = "constructor"
                }
            });

            Log.Information("Target addr: {Addr}", targetAddr);

            var debotAddr = await client.DeployWithGiverAsync(new ParamsOfEncodeMessage
            {
                Abi = debotAbi,
                DeploySet = new DeploySet
                {
                    Tvc = debotTvc
                },
                Signer = new Signer.Keys
                {
                    KeysProperty = keys
                },
                CallSet = new CallSet
                {
                    FunctionName = "constructor",
                    Input = new
                    {
                        debotAbi = JsonConvert.SerializeObject((debotAbi as Abi.Contract).Value).ToHexString(),
                        targetAbi = JsonConvert.SerializeObject((targetAbi as Abi.Contract).Value).ToHexString(),
                        targetAddr
                    }.ToJson()
                }
            });

            Log.Information("Debot addr: {Addr}", debotAddr);

            var debot = new DemoDebot(client, debotAddr, keys);
            await debot.StartAsync();
        }
    }
}
