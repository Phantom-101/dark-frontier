#nullable enable
using System;
using System.Collections.Generic;
using DarkFrontier.Attributes;
using DarkFrontier.Items._Scripts;
using DarkFrontier.Items.Equipment;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Items.Segments
{
    public class SegmentInstance : ItemInstance, IEquatable<SegmentInstance>
    {
        public new SegmentPrototype Prototype => (SegmentPrototype)base.Prototype;

        [field: SerializeReference] [JsonProperty("hp")]
        public float Hp { get; private set; }

        [field: SerializeReference, ReadOnly]
        public EquipmentComponent[] Equipment { get; private set; } = Array.Empty<EquipmentComponent>();
        
        [field: SerializeReference] [JsonProperty("equipment")]
        public Dictionary<string, EquipmentInstance> EquipmentRecords { get; set; } = new();
        
        private VisualElement _selector = null!;
        private VisualElement _selected = null!;
        private VisualElement _unselected = null!;
        
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

        public virtual void OnEquipped(SegmentComponent component)
        {
        }

        public virtual void OnUnequipped(SegmentComponent component)
        {
        }
        
        public virtual VisualElement CreateSelector()
        {
            _selector = Prototype.selectorElement!.CloneTree();
            _selector.Q<Label>("name").text = Prototype.name;
            _selected = _selector.Q("selected");
            _unselected = _selector.Q("unselected");
            return _selector;
        }

        public virtual void UpdateSelector(SegmentComponent component, bool selected)
        {
            var position = component.camera.WorldToViewportPoint(component.transform.position);
            if(position.z > 0)
            {
                _selector.style.visibility = Visibility.Visible;
                _selector.style.left = new StyleLength(new Length(position.x * 100, LengthUnit.Percent));
                _selector.style.top = new StyleLength(new Length(100 - position.y * 100, LengthUnit.Percent));
                
                _selected.style.visibility = selected ? Visibility.Visible : Visibility.Hidden;
                _unselected.style.visibility = selected ? Visibility.Hidden : Visibility.Visible;
                _unselected.pickingMode = selected ? PickingMode.Ignore : PickingMode.Position;
            }
            else
            {
                _selector.style.visibility = Visibility.Hidden;
            }
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
