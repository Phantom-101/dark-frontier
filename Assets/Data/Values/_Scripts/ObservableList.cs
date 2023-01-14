#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;

namespace DarkFrontier.Data.Values
{
    public class ObservableList<T> : IObservable<List<T>>, IEnumerable<T>
    {
        public virtual List<T> Value
        {
            get => new(_value);
            set
            {
                if (EqualityComparer<List<T>>.Default.Equals(_value, value)) return;
                _value = value;
                Notify();
            }
        }

        private List<T> _value;
        
        public event EventHandler<List<T>>? OnNotify;

        public ObservableList()
        {
            _value = new List<T>();
        }

        public ObservableList(IEnumerable<T> from)
        {
            _value = new List<T>(from);
        }

        public void Add(T item)
        {
            _value.Add(item);
            Notify();
        }

        public void AddRange(IEnumerable<T> collection)
        {
            using var enumerator = collection.GetEnumerator();
            if (!enumerator.MoveNext()) return;
            _value.Add(enumerator.Current);
            while (enumerator.MoveNext())
            {
                _value.Add(enumerator.Current);
            }
            Notify();
        }

        public void Clear()
        {
            if (_value.Count == 0) return;
            _value.Clear();
            Notify();
        }

        public void Insert(int index, T item)
        {
            _value.Insert(index, item);
            Notify();
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            using var enumerator = collection.GetEnumerator();
            if (!enumerator.MoveNext()) return;
            _value.Insert(index++, enumerator.Current);
            while (enumerator.MoveNext())
            {
                _value.Insert(index++, enumerator.Current);
            }
            Notify();
        }

        public bool Remove(T item)
        {
            if (!_value.Remove(item)) return false;
            Notify();
            return true;
        }

        public int RemoveAll(Predicate<T> match)
        {
            var ret = _value.RemoveAll(match);
            if (ret > 0)
            {
                Notify();
            }
            return ret;
        }

        public void RemoveAt(int index)
        {
            _value.RemoveAt(index);
            Notify();
        }

        public void RemoveRange(int index, int count)
        {
            if (count < 1) return;
            _value.RemoveRange(index, count);
            Notify();
        }
        
        protected virtual void Notify()
        {
            OnNotify?.Invoke(this, new List<T>(_value));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}