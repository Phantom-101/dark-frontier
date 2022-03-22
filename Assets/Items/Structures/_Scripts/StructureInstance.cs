#nullable enable
using System;
using System.Collections.Generic;
using DarkFrontier.Attributes;
using DarkFrontier.Data.Values;
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
    public class StructureInstance : ItemInstance, IEquatable<StructureInstance>
    {
        public new StructurePrototype Prototype => (StructurePrototype)base.Prototype;
        
        [field: SerializeReference, ReadOnly]
        public float MaxHp { get; private set; }

        [field: SerializeReference] [JsonProperty("pool-hp")]
        public float CurrentHp { get; private set; }
        
        [field: SerializeReference, ReadOnly]
        public SegmentComponent[] Segments { get; private set; } = Array.Empty<SegmentComponent>();
        
        [field: SerializeReference] [JsonProperty("segments")]
        public SegmentRecord?[] SegmentRecords { get; private set; } = Array.Empty<SegmentRecord?>();

        public void ClearSegments() => Segments = Array.Empty<SegmentComponent>();

        public void FindSegments(GameObject gameObject) => Segments = gameObject.GetComponentsInChildren<SegmentComponent>();

        [field: SerializeReference]
        public CapacitorValue Capacitor { get; private set; } = new(new MutableValue<float>(0));

        [field: SerializeReference]
        public Faction? Faction { get; private set; }

        [field: SerializeReference] [JsonProperty("faction-id")]
        public string FactionId { get; private set; } = "";

        [field: SerializeReference]
        public SectorComponent? Sector { get; private set; }

        [field: SerializeReference] [JsonProperty("sector-id")]
        public string SectorId { get; private set; } = "";
        
        [field: SerializeReference]
        public StructureComponent? Selected { get; private set; }

        [field: SerializeReference] [JsonProperty("selected-id")]
        public string SelectedId { get; private set; } = "";
        
        public StructureInstance()
        {
        }

        public StructureInstance(StructurePrototype prototype) : base(prototype)
        {
        }

        public override void ToSerialized()
        {
            base.ToSerialized();
            
            FactionId = Faction?.Id ?? "";
            SectorId = Sector == null ? "" : Sector.Instance?.Id ?? "";
            SelectedId = Selected == null ? "" : Selected.Instance?.Id ?? "";
            
            var l = Segments.Length;
            List<SegmentRecord> records = new(l);
            for(var i = 0; i < l; i++)
            {
                if(Segments[i].Instance != null)
                {
                    records.Add(new SegmentRecord(Segments[i].Name, Segments[i].Instance));
                }
            }
            SegmentRecords = records.ToArray();
        }

        public override void FromSerialized()
        {
            base.FromSerialized();
            
            Faction = FactionId.Length > 0 ? Singletons.Get<FactionManager>().Registry.Find(FactionId) : Faction;
            Sector = SectorId.Length > 0 ? null : Sector;
            Selected = SelectedId.Length > 0 ? null : Selected;
        }

        public bool Equals(StructureInstance? other)
        {
            return other != null && ReferenceEquals(this, other);
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

        [Serializable]
        public class CapacitorValue : MutableValue<float>
        {
            [field: SerializeField]
            public MutableValue<float> Max { get; private set; }
            
            public CapacitorValue(MutableValue<float> max) : base(0)
            {
                Max = max;
            }

            public override void Update()
            {
                base.Update();
                if(Value > Max.Value) Value = Max.Value;
                if(Value < 0) Value = 0;
            }
        }
    }
}