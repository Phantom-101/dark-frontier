using System;
using UnityEngine;

namespace DarkFrontier.Foundation {
    [Serializable]
    public class ValueNotifierF<T> : INotifier<T> {
        public event EventHandler Notifier;

        public T Value {
            get => value;
            set {
                this.value = value;
                Notifier?.Invoke (this, EventArgs.Empty);
            }
        }
        [SerializeField] private T value;

        public ValueNotifierF () {
            value = default;
        }

        public ValueNotifierF (T value) {
            this.value = value;
        }
    }

    [Serializable] public class StringValueNotifierF : ValueNotifierF<string> { }
}
