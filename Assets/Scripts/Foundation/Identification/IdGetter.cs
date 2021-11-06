using DarkFrontier.Foundation.Events;
using System;
using UnityEngine;

#nullable enable
namespace DarkFrontier.Foundation.Identification {
    [Serializable]
    public class IdGetter<T> : INotifier, IEquatable<IdGetter<T>> where T : class {
        public event EventHandler<EventArgs>? Notifier;

        public StringValueNotifierF UId => iId;
#pragma warning disable IDE0044 // Add readonly modifier
        [SerializeField] private StringValueNotifierF iId = new StringValueNotifierF ();
#pragma warning restore IDE0044 // Add readonly modifier

        private readonly Func<string, T?> iGetter;

        public T? UValue => GetValue ();
        [SerializeReference] private T? value;

        public IdGetter (Func<string, T?> aGetter) {
            iGetter = aGetter;
            UId.Notifier += OnIdChanged;
        }

        private T? GetValue () {
            if (value == null) {
                value = iGetter (UId.Value);
            }
            return value;
        }

        private void OnIdChanged (object aSender, EventArgs aArgs) {
            value = null;
            Notifier?.Invoke (this, EventArgs.Empty);
        }
        
        public override bool Equals(object? aObj) {
            if (ReferenceEquals(null, aObj)) return false;
            if (ReferenceEquals(this, aObj)) return true;
            return aObj.GetType() == GetType() && Equals((IdGetter<T>) aObj);
        }

        public bool Equals(IdGetter<T>? aOther) {
            if (ReferenceEquals(null, aOther)) return false;
            if (ReferenceEquals(this, aOther)) return true;
            return UId.Value == aOther.UId.Value;
        }

        public override int GetHashCode() => UId.Value.GetHashCode();
    }
}
#nullable restore
