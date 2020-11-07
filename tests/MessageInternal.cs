using Newtonsoft.Json;

namespace TonSdk.Tests
{
    internal class MessageInternal
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("cell")]
        public string CellBase64 { get; set; }

        [JsonProperty("msg_type")]
        public MessageType Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public enum MessageType
    {
        Internal = 0,
        ExternalInbound,
        ExternalOutbound,
        Unknown
    }
}
