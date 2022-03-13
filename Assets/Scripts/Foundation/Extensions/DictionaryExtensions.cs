#nullable enable
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;

namespace DarkFrontier.Foundation.Extensions {
    public static class DictionaryExtensions {
        public static TValue? TryGet<TKey, TValue> (this IDictionary<TKey, TValue> dict, TKey key, TValue? value)
        {
            return dict.TryGetValue (key, out var res) ? res : value;
        }

        public static void TryCopyTo<TKey, TValue> (this IDictionary<TKey, TValue> dict, IDictionary<TKey, TValue> target) {
            foreach (var key in dict.Keys) target.TryAdd (key, dict[key]);
        }

        public static TDict ToSerializable<TKey, TValue, TDict> (this IDictionary<TKey, TValue> dict) where TDict : SerializableDictionaryBase<TKey, TValue> {
            TDict ret = (Activator.CreateInstance (typeof (TDict)) as TDict)!;
            foreach (var (key, value) in dict) ret[key] = value;
            return ret;
        }
    }
}

