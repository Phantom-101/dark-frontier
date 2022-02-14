#nullable enable
using System;
using DarkFrontier.Attributes;
using DarkFrontier.Items._Scripts;
using DarkFrontier.Items.Equipment;
using DarkFrontier.Items.Structures;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Items.Segments
{
    public class SegmentInstance : ItemInstance, IEquatable<SegmentInstance>, IDetectable
    {
        [field: SerializeReference, ReadOnly]
        public SegmentComponent? Component { get; private set; }
        
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
        
        public bool SetComponent(SegmentComponent component)
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

        public override void PreSerialize()
        {
            base.PreSerialize();
            if(Component == null) return;
            int l;
            Equipment = new EquipmentRecord?[l = Component.Equipment.Length];
            for(var i = 0; i < l; i++)
            {
                if(Component.Equipment[i].Instance != null)
                {
                    Component.Equipment[i].Instance!.PreSerialize();
                    Equipment[i] = new EquipmentRecord(Component.Equipment[i].Name, Component.Equipment[i].Instance);
                }
            }
        }

        public override void PostDeserialize()
        {
            base.PostDeserialize();
            if(Component == null) return;
            for(int i = 0, l = Component.Equipment.Length; i < l; i++)
            {
                Component.Equipment[i].Instance?.PostDeserialize();
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
