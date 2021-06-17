using System.Collections.Generic;

public static class DictionaryExtensionMethods {
    public static bool TryAdd<K, V> (this IDictionary<K, V> dict, K key, V value) {
        if (dict.ContainsKey (key)) return false;
        dict[key] = value;
        return true;
    }

    public static V TryGet<K, V> (this IDictionary<K, V> dict, K key, V value) {
        if (dict.TryGetValue (key, out V res)) return res;
        return value;
    }

    public static void TryCopyTo<K, V> (this IDictionary<K, V> dict, IDictionary<K, V> target) {
        foreach (K key in dict.Keys) target.TryAdd (key, dict[key]);
    }
}
