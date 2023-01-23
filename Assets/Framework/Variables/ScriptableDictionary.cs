using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Variables {
    public class ScriptableDictionary<TKey, TValue> : ScriptableVariable<Dictionary<TKey, TValue>>, ISerializationCallbackReceiver {
        [SerializeField] private List<TKey> keys = new();
        [SerializeField] private List<TValue> values = new();

        private void Reset() {
            value = new Dictionary<TKey, TValue>();
        }

        public void OnBeforeSerialize() {
            keys = new List<TKey>(value.Keys);
            values = new List<TValue>(value.Values);
        }

        public void OnAfterDeserialize() {
            value.Clear();
            for (var i = 0; i < Math.Min(keys.Count, values.Count); i++) {
                value.Add(keys[i], values[i]);
            }
        }
    }
}