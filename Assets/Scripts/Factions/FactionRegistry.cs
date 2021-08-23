using DarkFrontier.Foundation.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DarkFrontier.Factions {
    [Serializable]
    public class FactionRegistry {
        public FactionIdMap FactionDictionary { get => factions; }
        [SerializeField] private FactionIdMap factions = new FactionIdMap ();
        public List<Faction> Factions { get => factions.Values.ToList (); }

        public void Add (Faction faction) => factions.Add (faction.Id, faction);
        public bool TryAdd (Faction faction) => factions.TryAdd (faction.Id, faction);
        public void Set (Faction faction) => factions[faction.Id] = faction;
        public bool Remove (Faction faction) => Remove (faction.Id);
        public bool Remove (string factionId) => factions.Remove (factionId);
        public void Clear () => factions.Clear ();

        public bool Has (string factionId) => factions.ContainsKey (factionId);
        public Faction Find (string factionId) => factions.TryGet (factionId, null);

        public List<Faction> FindAllWithName (string factionName) => Factions.FindAll (e => e.Name == factionName);
        public Faction FindWithProperty (string structureId) => Factions.Find (e => e.Property.Has (structureId));
    }
}
