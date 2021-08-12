using System;

namespace DarkFrontier.Foundation {
    [Serializable]
    public class ValueNotifier<T> : INotifier<T> {
        public event EventHandler Notifier;

        public T Value {
            get => value;
            set {
                this.value = value;
                Notifier?.Invoke (this, EventArgs.Empty);
            }
        }
        private T value;

        public ValueNotifier () {
            value = default;
        }

        public ValueNotifier (T value) {
            this.value = value;
        }
    }
}
