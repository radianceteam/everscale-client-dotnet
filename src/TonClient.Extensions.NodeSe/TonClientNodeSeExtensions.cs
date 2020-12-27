using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TonSdk.Modules;

namespace TonSdk.Extensions.NodeSe
{
    public static class TonClientNodeSeExtensions
    {
        public static Abi GiverAbi => TonUtil.LoadAbi("Giver.abi.json");

        public static async Task<string> DeployWithGiverAsync(this ITonClient client, ParamsOfEncodeMessage @params, ulong? value = null)
        {
            var msg = await client.Abi.EncodeMessageAsync(@params);
            await client.GetGramsFromGiverAsync(msg.Address, value);
            await client.Processing.ProcessMessageAsync(new ParamsOfProcessMessage
            {
                MessageEncodeParams = @params,
                SendEvents = false
            });
            return msg.Address;
        }

        public static async Task GetGramsFromGiverAsync(this ITonClient client, string account, ulong? value = null)
        {
            var runResult = await client.NetProcessFunctionAsync(TonClientNodeSe.GiverAddress,
                GiverAbi,
                "sendGrams",
                new
                {
                    dest = account,
                    amount = value ?? 500_000_000ul,
                }.ToJson(),
                new Signer.None());

            foreach (var outMessage in runResult.OutMessages)
            {
                var parsed = await client.Boc.ParseMessageAsync(new ParamsOfParse
                {
                    Boc = outMessage
                });

                var message = parsed.Parsed.ToObject<MessageInternal>();
                if (message?.Type == MessageType.Internal)
                {
                    await client.Net.WaitForCollectionAsync(new ParamsOfWaitForCollection
                    {
                        Collection = "transactions",
                        Filter = new
                        {
                            in_msg = new
                            {
                                eq = message.Id
                            }
                        }.ToJson(),
                        Result = "id"
                    });
                }
            }
        }

        public static async Task<ResultOfProcessMessage> NetProcessFunctionAsync(this ITonClient client,
            string address,
            Abi abi,
            string functionName,
            JToken input,
            Signer signer)
        {
            return await client
                .Processing.ProcessMessageAsync(new ParamsOfProcessMessage
                {
                    MessageEncodeParams = new ParamsOfEncodeMessage
                    {
                        Address = address,
                        Abi = abi,
                        CallSet = new CallSet
                        {
                            FunctionName = functionName,
                            Input = input
                        },
                        Signer = signer
                    },
                    SendEvents = false
                });
        }
    }
}
