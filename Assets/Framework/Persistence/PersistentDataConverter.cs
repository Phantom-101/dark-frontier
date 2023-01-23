#nullable enable
using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Framework.Persistence {
    public class PersistentDataConverter : JsonConverter<PersistentData> {
        private const string TypeIdKey = "typeId";
        private const string IDKey = "id";

        public override PersistentData? ReadJson(JsonReader reader, Type objectType, PersistentData? existingValue,
            bool hasExistingValue, JsonSerializer serializer) {
            var jObject = JObject.Load(reader);
            var ret = new PersistentData {
                typeId = jObject.Value<string>(TypeIdKey)!,
                id = jObject.Value<string>(IDKey)!
            };
            foreach (var property in jObject.Properties()) {
                if (property.Name != TypeIdKey && property.Name != IDKey) {
                    ret.data[property.Name] = property.Value;
                }
            }

            return ret;
        }

        public override void WriteJson(JsonWriter writer, PersistentData? value, JsonSerializer serializer) {
            if (value == null) {
                writer.WriteNull();
                return;
            }

            var jObject = new JObject {
                { TypeIdKey, value.typeId },
                { IDKey, value.id }
            };
            foreach (var key in value.data.Keys.Where(key => key != TypeIdKey && key != IDKey)) {
                jObject.Add(key, JToken.FromObject(value.data[key]));
            }

            jObject.WriteTo(writer);
        }
    }
}