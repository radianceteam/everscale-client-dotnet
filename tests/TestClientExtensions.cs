using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
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
    }
}
