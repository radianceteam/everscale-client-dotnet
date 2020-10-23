using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TonSdk.Modules;

// Note: this is a manually created module class.
// The idea is to generate such a module classes automatically from api.json.

namespace TonSdk.Modules
{
    public sealed class ResultOfVersion
    {
        /// <summary>
        /// core version
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }
    }

    public interface IClientModule
    {
        Task<ResultOfVersion> GetVersionAsync();
    }

    internal sealed class ClientModule : IClientModule
    {
        private readonly TonClient _client;

        public ClientModule(TonClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<ResultOfVersion> GetVersionAsync()
        {
            var json = await _client.CallFunction("client.version");
            return JsonConvert.DeserializeObject<ResultOfVersion>(json);
        }
    }
}

namespace TonSdk
{
    public partial interface ITonClient
    {
        IClientModule Client { get; }
    }

    public partial class TonClient
    {
        private ClientModule _clientModule;

        public IClientModule Client => _clientModule ?? (_clientModule = new ClientModule(this));
    }
}
