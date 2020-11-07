using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using TonSdk.Modules;
using Xunit;

namespace TonSdk.Tests
{
    internal static class TestClientExtensions
    {
        public const string WalletAddress = "0:2bb4a0e8391e7ea8877f4825064924bd41ce110fce97e939d3323999e1efbb13";
        public const string GiverAddress = "0:841288ed3b55d9cdafa806807f02a0ae0c169aa5edfe88a789a6482429756a94";

        public static async Task<string> SignDetachedAsync(this ITonClient client, string data, KeyPair keyPair)
        {
            var signKeys = await client.Crypto.NaclSignKeypairFromSecretKeyAsync(new ParamsOfNaclSignKeyPairFromSecret
            {
                Secret = keyPair.Secret
            });

            var result = await client.Crypto.NaclSignDetachedAsync(new ParamsOfNaclSign
            {
                Secret = signKeys.Secret,
                Unsigned = data
            });

            return result.Signature;
        }

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
            var runResult = await client.NetProcessFunctionAsync(GiverAddress,
                TestClient.GiverAbi,
                "sendGrams",
                JObject.FromObject(new
                {
                    dest = account,
                    amount = value ?? 500_000_000ul,
                }),
                new Signer.None());

            foreach (var outMessage in runResult.OutMessages)
            {
                var parsed = await client.Boc.ParseMessageAsync(new ParamsOfParse
                {
                    Boc = outMessage
                });

                var message = parsed.Parsed.ToObject<MessageInternal>();
                Assert.NotNull(message);

                if (message.Type == MessageType.Internal)
                {
                    await client.Net.WaitForCollectionAsync(new ParamsOfWaitForCollection
                    {
                        Collection = "transactions",
                        Filter = JObject.FromObject(new
                        {
                            in_msg = new
                            {
                                eq = message.Id
                            }
                        }),
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

        public static async Task<JToken> FetchAccountAsync(this ITonClient client, string address)
        {
            var result = await client.Net.WaitForCollectionAsync(new ParamsOfWaitForCollection
            {
                Collection = "accounts",
                Filter = JObject.FromObject(new
                {
                    id = new { eq = address }
                }),
                Result = "id boc"
            });

            Assert.NotNull(result);
            return result.Result;
        }
    }
}
