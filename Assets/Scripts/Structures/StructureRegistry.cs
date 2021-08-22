using DarkFrontier.Foundation.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DarkFrontier.Structures {
    [Serializable]
    public class StructureRegistry {
        public StructureIdMap StructureDictionary { get => structures; }
        [SerializeField] private StructureIdMap structures = new StructureIdMap ();
        public List<Structure> Structures { get => structures.Values.ToList (); }

        public void Add (Structure structure) => structures[structure.Id] = structure;
        public bool TryAdd (Structure structure) => structures.TryAdd (structure.Id, structure);
        public bool Remove (string structureId) => structures.Remove (structureId);
        public bool Remove (Structure structure) => Remove (structure.Id);

        public bool Has (string structureId) => structures.ContainsKey (structureId);
        public Structure Find (string structureId) => structures.TryGet (structureId, null);

        public List<Structure> FindAllWithName (string structureName) => Structures.FindAll (e => e.name == structureName);
        public List<Structure> FindAllWithSector (string sectorId) => Structures.FindAll (e => e.Sector.Id.Value == sectorId);
        public List<Structure> FindAllWithFaction (string factionId) => Structures.FindAll (e => e.Faction.Id.Value == factionId);
    }
}
