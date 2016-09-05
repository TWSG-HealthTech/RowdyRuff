using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RowdyRuff.Common.JSON
{
    public class JsonObjectAsStringConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            return token.ToString();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(JTokenType);
        }
    }
}
