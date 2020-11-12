using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TonSdk
{
    public class TonClientException : Exception
    {
        public int? Code { get; private set; }

        public TonClientException()
        {
        }

        public TonClientException(string message) : base(message)
        {
        }

        public TonClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public static TonClientException FromJson(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return new TonClientException("<empty JSON>");
            }

            try
            {
                return FromJson(JObject.Parse(input));
            }
            catch (JsonReaderException)
            {
                return new TonClientException(input);
            }
        }

        public static TonClientException FromJson(JToken token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            var data = token.Value<JToken>("data");
            var message = token.Value<string>("message");
            var code = token.Value<int?>("code");

            return new TonClientException(!string
                    .IsNullOrEmpty(message) ? message : token.ToString())
                .WithCode(code)
                .WithData(data);
        }

        private TonClientException WithCode(int? code)
        {
            Code = code;
            return this;
        }

        private TonClientException WithData(JToken dataToken)
        {
            var dict = dataToken?.ToObject<Dictionary<string, object>>();
            if (dict != null)
            {
                foreach (var entry in dict)
                {
                    Data.Add(entry.Key, entry.Value);
                }
            }
            return this;
        }
    }
}
