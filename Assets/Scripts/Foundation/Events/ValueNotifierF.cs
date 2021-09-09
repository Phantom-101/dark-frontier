using System;
using UnityEngine;

namespace DarkFrontier.Foundation.Events {
    [Serializable]
    public class ValueNotifierF<TValue> : IValueNotifier<TValue> {
        public event EventHandler<EventArgs> Notifier;

        public TValue Value {
            get => value;
            set {
                this.value = value;
                Notifier?.Invoke (this, EventArgs.Empty);
            }
        }
        [SerializeField] private TValue value;

        public ValueNotifierF () {
            value = default;
        }

        public ValueNotifierF (TValue value) {
            this.value = value;
        }
    }

    [Serializable] public class StringValueNotifierF : ValueNotifierF<string> { }
}
