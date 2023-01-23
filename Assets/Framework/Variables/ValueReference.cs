using System;

namespace Framework.Variables {
    [Serializable]
    public class ValueReference<T> {
        public bool useConstant = true;
        public T constant;
        public ScriptableVariable<T> variable;

        public ValueReference() { }

        public ValueReference(T value) {
            constant = value;
        }

        public T Value => useConstant ? constant : variable.value;

        public static implicit operator T(ValueReference<T> reference) {
            return reference.Value;
        }
    }
}