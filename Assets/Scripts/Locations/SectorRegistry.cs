using DarkFrontier.Foundation.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DarkFrontier.Locations {
    [Serializable]
    public class SectorRegistry {
        public SectorIdMap SectorDictionary { get => sectors; }
        [SerializeField] private SectorIdMap sectors = new SectorIdMap ();
        public List<Sector> Sectors { get => sectors.Values.ToList (); }

        public void Add (Sector sector) => sectors[sector.Id] = sector;
        public bool TryAdd (Sector sector) => sectors.TryAdd (sector.Id, sector);
        public bool Remove (string sectorId) => sectors.Remove (sectorId);
        public bool Remove (Sector sector) => Remove (sector.Id);

        public bool Has (string sectorId) => sectors.ContainsKey (sectorId);
        public Sector Find (string sectorId) => sectors.TryGet (sectorId, null);

        public List<Sector> FindAllWithName (string sectorName) => Sectors.FindAll (e => e.name == sectorName);
    }
}
