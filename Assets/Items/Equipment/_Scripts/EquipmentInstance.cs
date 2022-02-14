#nullable enable
using System;
using DarkFrontier.Attributes;
using DarkFrontier.Items._Scripts;
using DarkFrontier.Items.Structures;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Items.Equipment._Scripts
{
    public class EquipmentInstance : ItemInstance, IEquatable<EquipmentInstance>, IDetectable
    {
        [field: SerializeReference, ReadOnly]
        public EquipmentComponent? Component { get; private set; }
        
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

        public bool SetComponent(EquipmentComponent component)
        {
            Component = component;
            return true;
        }

        public bool RemoveComponent()
        {
            Component = null;
            return true;
        }
        
        public bool IsDetected(StructureInstance structure)
        {
            return true;
        }

        public VisualElement CreateSelector()
        {
            return new VisualElement();
        }

        public Vector3 GetSelectorPosition()
        {
            return Vector3.zero;
        }
        
        public VisualElement CreateSelected()
        {
            return new VisualElement();
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
