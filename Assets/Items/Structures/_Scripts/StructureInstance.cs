using System;
using System.Runtime.Serialization;
using DarkFrontier.Factions;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Items._Scripts;
using DarkFrontier.Items.Segments;
using DarkFrontier.Positioning.Sectors;
using Newtonsoft.Json;
using UnityEngine;


namespace DarkFrontier.Items.Structures
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class StructureInstance : ItemInstance, IEquatable<StructureInstance>
    {
        public StructureComponent? Component;

        public new StructurePrototype Prototype => (StructurePrototype)base.Prototype;

        [field: SerializeReference]
        [JsonProperty("pool-hp")]
        public float PoolHp { get; private set; }

        [field: SerializeReference]
        [JsonProperty("segments")]
        public SegmentRecord?[] Segments { get; set; } = Array.Empty<SegmentRecord?>();

        [field: SerializeReference]
        public Faction? Faction { get; private set; }

        [JsonProperty("faction-id")]
        private string _factionId = "";

        [field: SerializeReference]
        public SectorInstance? Sector { get; private set; }

        [JsonProperty("sector-id")]
        private string _sectorId = "";

        [field: SerializeReference]
        public StructureComponent? Selected { get; private set; }

        [JsonProperty("selected-id")]
        private string _selectedId = "";

        public StructureInstance()
        {
        }

        public StructureInstance(StructurePrototype prototype) : base(prototype)
        {
        }

        private void PreSerialize()
        {
            _factionId = Faction?.Id ?? "";
            _sectorId = "";
            _selectedId = Selected == null ? "" : Selected.instance?.Id ?? "";
        }

        private void PostDeserialize()
        {
            Faction = _factionId.Length > 0 ? Singletons.Get<FactionManager>().Registry.Find(_factionId) : Faction;
            Sector = _sectorId.Length > 0 ? null : Sector;
            Selected = _selectedId.Length > 0 ? null : Selected;
        }

        public bool Equals(StructureInstance? other)
        {
            return other is object && ReferenceEquals(this, other);
        }

        public override bool Equals(object? obj)
        {
            if(ReferenceEquals(null, obj)) return false;
            if(ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((StructureInstance)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
