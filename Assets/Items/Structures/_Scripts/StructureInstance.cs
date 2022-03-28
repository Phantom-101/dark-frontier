#nullable enable
using System;
using System.Collections.Generic;
using DarkFrontier.Attributes;
using DarkFrontier.Data.Values;
using DarkFrontier.Factions;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Items._Scripts;
using DarkFrontier.Items.Equipment;
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

        [field: SerializeField, ReadOnly] [JsonProperty("position")]
        public Vector3 Position { get; private set; }
        
        [field: SerializeField, ReadOnly] [JsonProperty("rotation")]
        public Vector3 Rotation { get; private set; }

        [field: SerializeField, ReadOnly] [JsonProperty("pool-hp-max")]
        public float MaxHp { get; private set; }

        [field: SerializeField] [JsonProperty("pool-hp")]
        public float CurrentHp { get; private set; }
        
        [field: SerializeField, ReadOnly]
        public SegmentComponent[] Segments { get; private set; } = Array.Empty<SegmentComponent>();
        
        [field: SerializeReference] [JsonProperty("segments")]
        public Dictionary<string, SegmentInstance> SegmentRecords { get; private set; } = new();

        public void ClearSegments() => Segments = Array.Empty<SegmentComponent>();

        public void FindSegments(GameObject gameObject) => Segments = gameObject.GetComponentsInChildren<SegmentComponent>();

        [field: SerializeReference] [JsonProperty("capacitor")]
        public CapacitorValue Capacitor { get; private set; } = new(new MutableValue<float>(0));

        [field: SerializeReference]
        public Faction? Faction { get; private set; }

        [field: SerializeField] [JsonProperty("faction-id")]
        public string FactionId { get; private set; } = "";

        [field: SerializeReference]
        public SectorComponent? Sector { get; private set; }

        [field: SerializeField] [JsonProperty("sector-id")]
        public string SectorId { get; private set; } = "";
        
        [field: SerializeReference]
        public StructureComponent? Selected { get; private set; }

        [field: SerializeField] [JsonProperty("selected-id")]
        public string SelectedId { get; private set; } = "";
        
        public StructureInstance()
        {
        }

        public StructureInstance(StructurePrototype prototype) : base(prototype)
        {
        }

        public StructureInstance(StructureAuthoring authoring) : base(authoring.prototype!)
        {
            Id = authoring.id;
            Name = authoring.name;
            var transform = authoring.transform;
            Position = transform.localPosition;
            Rotation = transform.localEulerAngles;
            FactionId = authoring.faction;
            SectorId = authoring.sector;
            SelectedId = authoring.selected;
        }

        public void Equip(string segment, SegmentInstance? instance)
        {
            for(int i = 0, l = Segments.Length; i < l; i++)
            {
                var segmentComponent = Segments[i];
                if(segmentComponent.Name == segment)
                {
                    segmentComponent.Instance?.OnUnequipped(Segments[i]);
                    segmentComponent.Set(instance);
                    segmentComponent.Enable();
                    segmentComponent.Instance?.OnEquipped(Segments[i]);
                    break;
                }
            }
        }

        public void Equip(string segment, string equipment, EquipmentInstance? instance)
        {
            for(int i = 0, li = Segments.Length; i < li; i++)
            {
                var segmentComponent = Segments[i];
                if(segmentComponent.Name == segment)
                {
                    if(segmentComponent.Instance != null)
                    {
                        for(int j = 0, lj = segmentComponent.Instance!.Equipment.Length; j < lj; j++)
                        {
                            var equipmentComponent = segmentComponent.Instance!.Equipment[j];
                            if(equipmentComponent.Name == equipment)
                            {
                                equipmentComponent.Instance?.OnUnequipped(equipmentComponent);
                                equipmentComponent.Set(instance);
                                equipmentComponent.Enable();
                                equipmentComponent.Instance?.OnEquipped(equipmentComponent);
                                break;
                            }
                        }
                    }
                    break;
                }
            }
        }
        
        public void ToSerialized(StructureComponent component)
        {
            ToSerialized();
            var transform = component.transform;
            Position = transform.localPosition;
            Rotation = transform.localEulerAngles;
        }
        
        public override void ToSerialized()
        {
            base.ToSerialized();
            FactionId = Faction?.Id ?? "";
            SectorId = Sector == null ? "" : Sector.Instance?.Id ?? "";
            SelectedId = Selected == null ? "" : Selected.Instance?.Id ?? "";
            SegmentRecords.Clear();
            for(int i = 0, l = Segments.Length; i < l; i++)
            {
                if(Segments[i].Instance != null)
                {
                    SegmentRecords.Add(Segments[i].Name, Segments[i].Instance!);
                }
            }
        }

        public void FromSerialized(StructureComponent component)
        {
            FromSerialized();
            var transform = component.transform;
            transform.localPosition = Position;
            transform.localEulerAngles = Rotation;
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
            [field: SerializeField] [JsonProperty("max")]
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