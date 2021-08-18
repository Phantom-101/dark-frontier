using DarkFrontier.Structures;
using System;
using UnityEngine;

namespace DarkFrontier.Foundation {
    [Serializable]
    public class IdRetriever<T> : INotifier {
        public event EventHandler Notifier;

        public StringValueNotifierF Id { get => id; }
        [SerializeField] private StringValueNotifierF id = new StringValueNotifierF ();
        public T Cached { get => value; }
        [SerializeReference] private T value;
        private bool dirty;

        public IdRetriever () {
            Id.Notifier += OnIdChanged;
        }

        public T Value (Func<string, T> getter) {
            if (dirty) {
                value = getter (Id.Value);
                dirty = false;
            }
            return value;
        }

        private void OnIdChanged (object sender, EventArgs e) {
            dirty = true;
            Notifier?.Invoke (this, EventArgs.Empty);
        }
    }

    [Serializable] public class StructureRetriever : IdRetriever<Structure> { }
    [Serializable] public class FactionRetriever : IdRetriever<Faction> { }
    [Serializable] public class SectorRetriever : IdRetriever<Sector> { }
}