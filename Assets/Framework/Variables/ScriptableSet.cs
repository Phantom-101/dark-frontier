using System.Collections.Generic;
using UnityEngine;

namespace Framework.Variables {
    public class ScriptableSet<T> : ScriptableVariable<HashSet<T>>, ISerializationCallbackReceiver {
        [SerializeField] private List<T> items = new();

        private void Reset() {
            value = new HashSet<T>();
        }

        public void OnBeforeSerialize() {
            items = new List<T>(value);
        }

        public void OnAfterDeserialize() {
            value = new HashSet<T>(items);
        }
    }
}