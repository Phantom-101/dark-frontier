using DarkFrontier.Foundation.Events;
using System;
using UnityEngine;

#nullable enable
namespace DarkFrontier.Foundation.Identification {
    [Serializable]
    public class IdGetter<T> : INotifier where T : class {
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
    }
}
#nullable restore
