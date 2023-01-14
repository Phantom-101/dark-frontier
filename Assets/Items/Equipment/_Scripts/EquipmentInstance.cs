#nullable enable
using System;
using DarkFrontier.Controllers.New;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Items._Scripts;
using DarkFrontier.UI.Indicators.Modifiers;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Items.Equipment
{
    [Serializable]
    public class EquipmentInstance : ItemInstance, IEquatable<EquipmentInstance>
    {
        public new EquipmentPrototype Prototype => (EquipmentPrototype)base.Prototype;
        
        [field: SerializeReference] [JsonProperty("hp")]
        public float Hp { get; private set; }

        private VisualElement _selector = null!;
        private VisualElement _selected = null!;
        private VisualElement _unselected = null!;
        private RadialProgressBar _durability = null!;
        
        public EquipmentInstance()
        {
        }
        
        public EquipmentInstance(EquipmentPrototype prototype) : base(prototype)
        {
        }
        
        public EquipmentAdaptor NewAdaptor() => new();
        
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
            _durability = _selector.Q<RadialProgressBar>("durability");
            return _selector;
        }

        public virtual void UpdateSelector(EquipmentComponent component, bool selected)
        {
            if (component.Structure == Singletons.Get<PlayerController>().Player)
            {
                _selector.style.visibility = Visibility.Hidden;
            }
            else
            {
                var position = component.camera.WorldToViewportPoint(component.transform.position);
                if (position.z > 0)
                {
                    _selector.style.visibility = Visibility.Visible;
                    _selector.style.left = new StyleLength(new Length(position.x * 100, LengthUnit.Percent));
                    _selector.style.top = new StyleLength(new Length(100 - position.y * 100, LengthUnit.Percent));

                    if (selected)
                    {
                        _selected.style.visibility = Visibility.Visible;
                        _unselected.style.visibility = Visibility.Hidden;
                        _unselected.pickingMode = PickingMode.Ignore;
                        _durability.Value = Prototype.hp == 0 ? 0 : Mathf.Clamp01(Hp / Prototype.hp) / 4;
                        _durability.MarkDirtyRepaint();
                    }
                    else
                    {
                        _selected.style.visibility = Visibility.Hidden;
                        _unselected.style.visibility = Visibility.Visible;
                        _unselected.pickingMode = PickingMode.Position;
                    }
                }
                else
                {
                    _selector.style.visibility = Visibility.Hidden;
                }
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
