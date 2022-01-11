using System;
using System.Collections.Generic;
using System.Linq;
using DarkFrontier.Foundation.Extensions;
using UnityEngine;

namespace DarkFrontier.Structures {
    [Serializable]
    public class StructureRegistry {
        public StructureIdMap UStructureDictionary => iStructures;
#pragma warning disable IDE0044 // Add readonly modifier
        [SerializeField] private StructureIdMap iStructures = new StructureIdMap ();
#pragma warning restore IDE0044 // Add readonly modifier
        public List<Structure> UStructures => iStructures.Values.ToList ();

        public void Trim () => iStructures.Where (e => e.Value == null).ToList ().ForEach (e => iStructures.Remove (e.Key));

        public void Add (Structure aStructure) => iStructures.Add (aStructure.uId, aStructure);
        public bool TryAdd (Structure aStructure) => iStructures.TryAdd(aStructure.uId, aStructure);
        public void Set (Structure aStructure) => iStructures[aStructure.uId] = aStructure;
        public bool Remove (Structure aStructure) => Remove (aStructure.uId);
        public bool Remove (string aStructureId) => iStructures.Remove (aStructureId);
        public void Clear () => iStructures.Clear ();

        public bool Has (string aStructureId) => iStructures.ContainsKey (aStructureId);
        public Structure Find (string aStructureId) => iStructures.TryGet (aStructureId, null);

        public List<Structure> FindAllWithName (string aStructureName) => UStructures.FindAll (lStructure => lStructure.name == aStructureName);
        public List<Structure> FindAllWithSector (string aSectorId) => UStructures.FindAll (lStructure => lStructure.uSector.UId.Value == aSectorId);
        public List<Structure> FindAllWithFaction (string aFactionId) => UStructures.FindAll (lStructure => lStructure.uFaction.UId.Value == aFactionId);
    }
}
