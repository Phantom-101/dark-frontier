#nullable enable
using System;
using DarkFrontier.Items._Scripts;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Items.Equipment
{
    public class EquipmentInstance : ItemInstance, IEquatable<EquipmentInstance>
    {
        public new EquipmentPrototype Prototype => (EquipmentPrototype)base.Prototype;
        
        [field: SerializeReference] [JsonProperty("hp")]
        public float Hp { get; private set; }
        
        public EquipmentInstance()
        {
        }
        
        public EquipmentInstance(EquipmentPrototype prototype) : base(prototype)
        {
        }
        
        public virtual void Apply(EquipmentAuthoring authoring)
        {
            Id = authoring.id;
            Name = authoring.name;
        }
        
        public virtual void OnEquipped(EquipmentComponent component)
        {
        }

        public virtual void OnUnequipped(EquipmentComponent component)
        {
        }

        public virtual void OnTick(EquipmentComponent component, float deltaTime)
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
