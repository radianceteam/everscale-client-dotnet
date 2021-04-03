using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Serialization;

namespace TonSdk
{
    internal class TonSerializer
    {
        private readonly ILogger _logger;

        private readonly JsonSerializerSettings _defaultSerializerSettings;
        private readonly JsonSerializerSettings _polymorphicTypeSerializerSettings;

        public TonSerializer(ILogger logger = null)
        {
            _logger = logger ?? DummyLogger.Instance;
            _defaultSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            _polymorphicTypeSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Objects
            };
            _polymorphicTypeSerializerSettings.Converters.Add(PolymorphicTypeConverter.Instance);
        }

        public T Deserialize<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                _logger.Warning("Empty JSON passed to deserialize method");
                return default;
            }

            var type = typeof(T);
            if (type == typeof(JToken))
            {
                object token = JObject.Parse(json);
                return (T)token;
            }
            return JsonConvert.DeserializeObject<T>(json,
                type.IsTonPolymorphicAbstractType() ||
                type.IsTonPolymorphicAbstractTypeArray()
                    ? _polymorphicTypeSerializerSettings
                    : _defaultSerializerSettings);
        }

        public T Deserialize<T>(JToken t)
        {
            if (t == null)
            {
                _logger.Warning("null token passed to deserialize method");
                return default;
            }

            if (typeof(T) == typeof(JToken))
            {
                object token = t;
                return (T)token;
            }
            return t.ToObject<T>(JsonSerializer.Create(
                typeof(T).IsTonPolymorphicAbstractType()
                ? _polymorphicTypeSerializerSettings
                : _defaultSerializerSettings));
        }

        public string Serialize(object any)
        {
            if (any == null)
            {
                _logger.Warning("Null passed to serialize method");
                return "null";
            }

            return JsonConvert.SerializeObject(any,
                any.GetType().IsTonPolymorphicConcreteType() || 
                any.GetType().IsTonPolymorphicAbstractTypeArray()
                    ? _polymorphicTypeSerializerSettings
                    : _defaultSerializerSettings);
        }

        public JToken SerializeToken(object any)
        {
            if (any == null)
            {
                _logger.Warning("Null passed to serialize method");
                return "null";
            }

            var token = JObject.FromObject(any);
            if (any.GetType().IsTonPolymorphicConcreteType())
            {
                token.Add("type", any.GetType().Name);
            }
            return token;
        }
    }

    internal static class TypeExtensions
    {
        public static bool IsTonPolymorphicAbstractType(this Type type)
        {
            return type.IsAbstract && type.GetNestedTypes()
                .All(t => t.BaseType == type); // TODO: optimize? cache?
        }

        public static bool IsTonPolymorphicConcreteType(this Type type)
        {
            return type.BaseType?.GetNestedTypes().Contains(type) == true; // TODO: optimize? cache?
        }

        public static bool IsTonPolymorphicAbstractTypeArray(this Type type)
        {
            return type.IsArray && type.GetElementType().IsTonPolymorphicAbstractType();
        }
    }

    internal class PolymorphicTypeConverter : JsonConverter
    {
        public override bool CanWrite => true;
        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsTonPolymorphicConcreteType() ||
                   objectType.IsTonPolymorphicAbstractType();
        }

        public override void WriteJson(JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            var o = (JObject)JToken.FromObject(value);
            o.Add(new JProperty("type", value.GetType().Name));
            o.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)

        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            var obj = JObject.Load(reader);
            var contract = FindContract(obj, serializer, objectType.GetNestedTypes());
            if (contract == null)
            {
                throw new JsonSerializationException("no contract found for " + obj);
            }
            if (existingValue == null || !contract.UnderlyingType.IsInstanceOfType(existingValue))
            {
                existingValue = contract.DefaultCreator();
            }
            using (var sr = obj.CreateReader())
            {
                serializer.Populate(sr, existingValue);
            }
            return existingValue;
        }

        private static JsonObjectContract FindContract(
            JToken obj,
            JsonSerializer serializer,
            IEnumerable<Type> derivedTypes)
        {
            var typeName = obj.Value<string>("type");
            if (string.IsNullOrEmpty(typeName))
            {
                return null;
            }
            var type = derivedTypes.FirstOrDefault(t => t.Name == typeName);
            if (type == null)
            {
                return null;
            }
            return serializer.ContractResolver
                .ResolveContract(type) as JsonObjectContract;
        }

        public static PolymorphicTypeConverter Instance = new PolymorphicTypeConverter();
    }
}
