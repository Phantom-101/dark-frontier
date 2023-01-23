using UnityEngine;

namespace Framework.Variables {
    public class VariableSetter<T> : MonoBehaviour {
        public ScriptableVariable<T> variable;
        public T value;

        private void Awake() {
            variable.value = value;
        }
    }
}