#nullable enable
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Framework.Persistence {
    [JsonConverter(typeof(PersistentDataConverter))]
    public class PersistentData {
        public string typeId = null!;
        public string id = null!;
        public readonly Dictionary<string, object> data = new();

        public bool Contains(string key) {
            return data.ContainsKey(key);
        }

        public object Get(string key) {
            return data[key];
        }

        public T Get<T>(string key) {
            return (T)data[key];
        }

        public int GetInt(string key) {
            return (int)(long)data[key];
        }

        public T[] GetArray<T>(string key) {
            return ((JArray)data[key]).ToObject<T[]>()!;
        }

        public List<T> GetList<T>(string key) {
            return ((JArray)data[key]).ToObject<List<T>>()!;
        }

        public void Add(string key, object value) {
            data.Add(key, value);
        }

        public void Set(string key, object value) {
            data[key] = value;
        }

        public void Remove(string key) {
            data.Remove(key);
        }
    }
}