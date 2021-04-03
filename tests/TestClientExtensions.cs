using System.Linq;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TonSdk.Modules;
using Xunit;

namespace TonSdk.Tests
{
    internal static class TestClientExtensions
    {
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

        public static async Task<JToken> FetchAccountAsync(this ITonClient client, string address)
        {
            var result = await client.Net.WaitForCollectionAsync(new ParamsOfWaitForCollection
            {
                Collection = "accounts",
                Filter = new
                {
                    id = new { eq = address }
                }.ToJson(),
                Result = "id boc"
            });

            Assert.NotNull(result);
            return result.Result;
        }

        public static async Task<string> DownloadAccountAsync(this ITonClient client, string addr)
        {
            var accounts = await client.Net.QueryCollectionAsync(new ParamsOfQueryCollection
            {
                Collection = "accounts",
                Filter = new
                {
                    id = new
                    {
                        eq = addr
                    }
                }.ToJson(),
                Result = "boc",
                Limit = 1
            });

            return accounts.Result.FirstOrDefault()?.Value<string>("boc");
        }

        public static async Task AssertGetMethodAsync(
            this ITonClient client,
            string addr,
            Abi abi,
            string func,
            object @params,
            object returns)
        {
            var accountBoc = await client.DownloadAccountAsync(addr);
            Assert.NotEmpty(accountBoc);

            var message = await client.Abi.EncodeMessageAsync(new ParamsOfEncodeMessage
            {
                Abi = abi,
                Signer = new Signer.None(),
                Address = addr,
                CallSet = new CallSet
                {
                    FunctionName = func,
                    Input = @params.ToJson()
                }
            });

            var result = await client.Tvm.RunTvmAsync(new ParamsOfRunTvm
            {
                Account = accountBoc,
                Message = message.Message,
                Abi = abi,
                ReturnUpdatedAccount = true
            });

            var output = result.Decoded.Output;
            Assert.NotNull(output);
            Assert.Equal(
                returns.ToJson().ToString(Formatting.None),
                output.ToString(Formatting.None));
        }

        public static async Task<string> GetCodeHashFromTvcAsync(this ITonClient client, string tvc)
        {
            var result = await client.Boc.GetCodeFromTvcAsync(new ParamsOfGetCodeFromTvc
            {
                Tvc = tvc
            });

            var hash = await client.Boc.GetBocHashAsync(new ParamsOfGetBocHash
            {
                Boc = result.Code
            });

            return hash.Hash;
        }

        public static async Task<int> CountAccountsByCodeHashAsync(this ITonClient client, string tvc)
        {
            var hash = await client.GetCodeHashFromTvcAsync(tvc);

            var result = await client.Net.QueryCollectionAsync(new ParamsOfQueryCollection
            {
                Collection = "accounts",
                Filter = new
                {
                    code_hash = new
                    {
                        eq = hash
                    }
                }.ToJson(),
                Result = "id"
            });

            return result.Result.Length;
        }
    }
}
