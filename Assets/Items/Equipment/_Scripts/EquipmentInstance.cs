using System;
using DarkFrontier.Items._Scripts;
using Newtonsoft.Json;
using UnityEngine;


namespace DarkFrontier.Items.Equipment._Scripts
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class EquipmentInstance : ItemInstance, IEquatable<EquipmentInstance>
    {
        public new EquipmentPrototype Prototype => (EquipmentPrototype)base.Prototype;
        
        [field: SerializeReference]
        [JsonProperty("hp")]
        public float Hp { get; private set; }
        
        public EquipmentInstance()
        {
        }
        
        public EquipmentInstance(EquipmentPrototype prototype) : base(prototype)
        {
        }

        public bool Equals(EquipmentInstance? other)
        {
            return !ReferenceEquals(null, other) && ReferenceEquals(this, other);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((EquipmentInstance)obj);
        }
        
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
