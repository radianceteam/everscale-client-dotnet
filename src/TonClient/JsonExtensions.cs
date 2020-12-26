using Newtonsoft.Json.Linq;

namespace TonSdk
{
    public static class JsonExtensions
    {
        public static JToken ToJson(this object obj)
        {
            return JObject.FromObject(obj);
        }
    }
}
