#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;

namespace DarkFrontier.Data.Values
{
    public class ObservableDictionary<TKey, TValue> : IObservable<Dictionary<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        public virtual Dictionary<TKey, TValue> Value
        {
            get => new(_value);
            set
            {
                if (EqualityComparer<Dictionary<TKey, TValue>>.Default.Equals(_value, value)) return;
                _value = value;
                Notify();
            }
        }

        private Dictionary<TKey, TValue> _value;
        
        public event EventHandler<Dictionary<TKey, TValue>>? OnNotify;

        public ObservableDictionary()
        {
            _value = new Dictionary<TKey, TValue>();
        }

        public ObservableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> from)
        {
            _value = new Dictionary<TKey, TValue>(from);
        }

        public void Add(TKey key, TValue value)
        {
            _value.Add(key, value);
            Notify();
        }
        
        protected virtual void Notify()
        {
            OnNotify?.Invoke(this, new Dictionary<TKey, TValue>(_value));
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
