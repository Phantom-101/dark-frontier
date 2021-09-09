using DarkFrontier.Factions;
using DarkFrontier.Foundation.Events;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Locations;
using DarkFrontier.Structures;
using System;
using UnityEngine;

#nullable enable
namespace DarkFrontier.Foundation.Identification {
    [Serializable]
    public class IdGetter<T> : INotifier where T : class {
        public event EventHandler<EventArgs>? Notifier;

        public StringValueNotifierF Id { get => id; }
        [SerializeField] private StringValueNotifierF id = new StringValueNotifierF ();

        private readonly Func<string, T?> getter;

        public T? Value { get => GetValue (); }
        [SerializeReference] private T? value;

        public IdGetter (Func<string, T?> getter) {
            this.getter = getter;

            Id.Notifier += OnIdChanged;
        }

        private T? GetValue () {
            if (value == null) {
                value = getter (Id.Value);
            }
            return value;
        }

        private void OnIdChanged (object sender, EventArgs e) {
            value = null;
            Notifier?.Invoke (this, EventArgs.Empty);
        }
    }

    [Serializable]
    public class StructureGetter : IdGetter<Structure> {
        public StructureGetter () : base (Getter) { }
        private static Structure? Getter (string id) => Singletons.Get<StructureManager> ().Registry.Find (id);
    }

    [Serializable]
    public class FactionGetter : IdGetter<Faction> {
        public FactionGetter () : base (Getter) { }
        private static Faction? Getter (string id) => Singletons.Get<FactionManager> ().Registry.Find (id);
    }

    [Serializable]
    public class SectorGetter : IdGetter<Sector> {
        public SectorGetter () : base (Getter) { }
        private static Sector? Getter (string id) => Singletons.Get<SectorManager> ().Registry.Find (id);
    }
}
#nullable restore
