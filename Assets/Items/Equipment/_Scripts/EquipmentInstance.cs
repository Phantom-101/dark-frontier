#nullable enable
using System;
using DarkFrontier.Items._Scripts;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Items.Equipment
{
    public class EquipmentInstance : ItemInstance, IEquatable<EquipmentInstance>
    {
        public new EquipmentPrototype Prototype => (EquipmentPrototype)base.Prototype;
        
        [field: SerializeReference] [JsonProperty("hp")]
        public float Hp { get; private set; }

        private VisualElement _selector = null!;
        private VisualElement _selected = null!;
        private VisualElement _unselected = null!;
        
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

        public virtual VisualElement CreateSelector()
        {
            _selector = Prototype.selectorElement!.CloneTree();
            _selector.Q<Label>("name").text = Prototype.name;
            _selected = _selector.Q("selected");
            _unselected = _selector.Q("unselected");
            return _selector;
        }

        public virtual void UpdateSelector(EquipmentComponent component, bool selected)
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
        
        public virtual VisualElement CreateIndicator()
        {
            return Prototype.indicatorElement == null ? new VisualElement() : Prototype.indicatorElement.CloneTree();
        }

        public virtual void UpdateIndicator(EquipmentComponent component)
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
