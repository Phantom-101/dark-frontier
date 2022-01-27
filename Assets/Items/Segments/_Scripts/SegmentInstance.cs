using System;
using DarkFrontier.Items._Scripts;
using DarkFrontier.Items.Equipment;
using Newtonsoft.Json;
using UnityEngine;


namespace DarkFrontier.Items.Segments
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class SegmentInstance : ItemInstance, IEquatable<SegmentInstance>
    {
        public new SegmentPrototype Prototype => (SegmentPrototype)base.Prototype;

        [field: SerializeReference]
        [JsonProperty("hp")]
        public float Hp { get; private set; }

        [field: SerializeReference]
        [JsonProperty("equipment")]
        public EquipmentRecord?[] Equipment { get; set; } = Array.Empty<EquipmentRecord?>();

        public SegmentInstance()
        {
        }

        public SegmentInstance(SegmentPrototype prototype) : base(prototype)
        {
        }

        public bool Equals(SegmentInstance? other)
        {
            return other is object && ReferenceEquals(this, other);
        }

        public override bool Equals(object? obj)
        {
            if(obj is null) return false;
            if(ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((SegmentInstance)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
