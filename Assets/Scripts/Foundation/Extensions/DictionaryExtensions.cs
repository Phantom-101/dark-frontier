using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;

#nullable enable
namespace DarkFrontier.Foundation.Extensions {
    public static class DictionaryExtensions {
        public static V TryGet<K, V> (this IDictionary<K, V> dict, K key, V value) {
            if (dict.TryGetValue (key, out V res)) return res;
            return value;
        }

        public static void TryCopyTo<K, V> (this IDictionary<K, V> dict, IDictionary<K, V> target) {
            foreach (K key in dict.Keys) target.TryAdd (key, dict[key]);
        }

        public static D ToSerializable<K, V, D> (this IDictionary<K, V> dict) where D : SerializableDictionaryBase<K, V> {
            D ret = (Activator.CreateInstance (typeof (D)) as D)!;
            foreach (KeyValuePair<K, V> pair in dict) ret[pair.Key] = pair.Value;
            return ret;
        }
    }
}
#nullable restore
