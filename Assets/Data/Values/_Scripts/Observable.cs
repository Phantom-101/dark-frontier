#nullable enable
using System;
using System.Collections.Generic;

namespace DarkFrontier.Data.Values
{
    public class Observable<T> : IObservable<T>
    {
        public virtual T Value
        {
            get => _value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value)) return;
                _value = value;
                Notify();
            }
        }

        private T _value;

        public event EventHandler<T>? OnNotify;

        public Observable(T value)
        {
            _value = value;
        }

        protected virtual void Notify()
        {
            OnNotify?.Invoke(this, _value);
        }
    }
}