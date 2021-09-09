using System;

namespace DarkFrontier.Foundation.Events {
    [Serializable]
    public class ValueNotifier<TValue> : IValueNotifier<TValue> {
        public event EventHandler<EventArgs> Notifier;

        public TValue Value {
            get => value;
            set {
                this.value = value;
                Notifier?.Invoke (this, EventArgs.Empty);
            }
        }
        private TValue value;

        public ValueNotifier () {
            value = default;
        }

        public ValueNotifier (TValue value) {
            this.value = value;
        }
    }
}
