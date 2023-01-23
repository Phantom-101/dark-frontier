using System.Collections.Generic;

namespace Framework.Variables {
    public class ScriptableList<T> : ScriptableVariable<List<T>> {
        private void Reset() {
            value = new List<T>();
        }
    }
}