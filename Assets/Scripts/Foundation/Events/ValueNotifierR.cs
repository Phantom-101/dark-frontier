using System;
using UnityEngine;

namespace DarkFrontier.Foundation.Events {
    [Serializable]
    public class ValueNotifierR<TValue> : IValueNotifier<TValue> {
        public event EventHandler<EventArgs> Notifier;

        public TValue Value {
            get => value;
            set {
                this.value = value;
                Notifier?.Invoke (this, EventArgs.Empty);
            }
        }
        [SerializeReference] private TValue value;

        public ValueNotifierR () {
            value = default;
        }

        public ValueNotifierR (TValue value) {
            this.value = value;
        }
    }

    [Serializable] public class StringValueNotifierR : ValueNotifierR<string> { }
}
