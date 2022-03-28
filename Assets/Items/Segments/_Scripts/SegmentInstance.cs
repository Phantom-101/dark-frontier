#nullable enable
using System;
using System.Collections.Generic;
using DarkFrontier.Attributes;
using DarkFrontier.Items._Scripts;
using DarkFrontier.Items.Equipment;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Items.Segments
{
    public class SegmentInstance : ItemInstance, IEquatable<SegmentInstance>
    {
        [field: SerializeReference, ReadOnly]
        public SegmentComponent? Component { get; private set; }
        
        public new SegmentPrototype Prototype => (SegmentPrototype)base.Prototype;

        [field: SerializeReference] [JsonProperty("hp")]
        public float Hp { get; private set; }

        [field: SerializeReference, ReadOnly]
        public EquipmentComponent[] Equipment { get; private set; } = Array.Empty<EquipmentComponent>();
        
        [field: SerializeReference] [JsonProperty("equipment")]
        public Dictionary<string, EquipmentInstance> EquipmentRecords { get; set; } = new();
        
        public void ClearEquipment() => Equipment = Array.Empty<EquipmentComponent>();
        
        public void FindEquipment(GameObject gameObject) => Equipment = gameObject.GetComponentsInChildren<EquipmentComponent>();

        public SegmentInstance()
        {
        }

        public SegmentInstance(SegmentPrototype prototype) : base(prototype)
        {
        }
        
        public virtual void Apply(SegmentAuthoring authoring)
        {
            Id = authoring.id;
            Name = authoring.name;
        }
        
        public void Equip(string equipment, EquipmentInstance? instance)
        {
            for(int i = 0, l = Equipment.Length; i < l; i++)
            {
                var equipmentComponent = Equipment[i];
                if(equipmentComponent.Name == equipment)
                {
                    equipmentComponent.Instance?.OnUnequipped(Equipment[i]);
                    equipmentComponent.Set(instance);
                    equipmentComponent.Enable();
                    equipmentComponent.Instance?.OnEquipped(Equipment[i]);
                    break;
                }
            }
        }

        public virtual void OnEquipped(SegmentComponent component)
        {
        }

        public virtual void OnUnequipped(SegmentComponent component)
        {
        }
        
        public override void ToSerialized()
        {
            base.ToSerialized();

            EquipmentRecords.Clear();
            for(int i = 0, l = Equipment.Length; i < l; i++)
            {
                if(Equipment[i].Instance != null)
                {
                    EquipmentRecords.Add(Equipment[i].Name, Equipment[i].Instance!);
                }
            }
        }

        public bool Equals(SegmentInstance? other)
        {
            return other != null && ReferenceEquals(this, other);
        }

        public override bool Equals(object? obj)
        {
            if(ReferenceEquals(null, obj)) return false;
            if(ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((SegmentInstance)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
