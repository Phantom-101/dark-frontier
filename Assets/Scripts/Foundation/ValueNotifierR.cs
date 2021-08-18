using System;
using UnityEngine;

namespace DarkFrontier.Foundation {
    [Serializable]
    public class ValueNotifierR<T> : IValueNotifier<T> {
        public event EventHandler Notifier;

        public T Value {
            get => value;
            set {
                this.value = value;
                Notifier?.Invoke (this, EventArgs.Empty);
            }
        }
        [SerializeReference] private T value;

        public ValueNotifierR () {
            value = default;
        }

        public ValueNotifierR (T value) {
            this.value = value;
        }
    }

    [Serializable] public class StringValueNotifierR : ValueNotifierR<string> { }
}
