#nullable enable
using System;
using System.Collections.Generic;
using DarkFrontier.Attributes;
using DarkFrontier.Items._Scripts;
using DarkFrontier.Items.Equipment;
using DarkFrontier.Items.Equipment._Scripts;
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
        public EquipmentRecord?[] EquipmentRecords { get; set; } = Array.Empty<EquipmentRecord?>();
        
        public void ClearEquipment() => Equipment = Array.Empty<EquipmentComponent>();
        
        public void FindEquipment(GameObject gameObject) => Equipment = gameObject.GetComponentsInChildren<EquipmentComponent>();

        public SegmentInstance()
        {
        }

        public SegmentInstance(SegmentPrototype prototype) : base(prototype)
        {
        }

        public override void ToSerialized()
        {
            base.ToSerialized();

            var l = Equipment.Length;
            List<EquipmentRecord> records = new();
            for(var i = 0; i < l; i++)
            {
                if(Equipment[i].Instance != null)
                {
                    records.Add(new EquipmentRecord(Equipment[i].Name, Equipment[i].Instance));
                }
            }
            EquipmentRecords = records.ToArray();
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
