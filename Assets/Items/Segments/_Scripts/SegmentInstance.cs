#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using DarkFrontier.Controllers.New;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Items._Scripts;
using DarkFrontier.Items.Equipment;
using DarkFrontier.UI.Indicators.Modifiers;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Items.Segments
{
    [Serializable]
    public class SegmentInstance : ItemInstance, IEquatable<SegmentInstance>
    {
        public new SegmentPrototype Prototype => (SegmentPrototype)base.Prototype;

        public int Rating
        {
            get
            {
                var ret = Prototype.rating;
                var equipment = Equipment.Values.ToArray();
                for (int i = 0, l = equipment.Length; i < l; i++)
                {
                    ret += equipment[i].Prototype.rating;
                }
                return ret;
            }
        }
        
        [field: SerializeReference] [JsonProperty("hp")]
        public float Hp { get; private set; }
        
        [field: SerializeReference] [JsonProperty("equipment")]
        public Dictionary<string, EquipmentInstance> Equipment { get; set; } = new();
        
        public event EventHandler<string>? SubTreeChanged;
        
        private VisualElement _selector = null!;
        private VisualElement _selected = null!;
        private VisualElement _unselected = null!;
        private RadialProgressBar _hull = null!;
        
        public SegmentInstance()
        {
        }

        public SegmentInstance(SegmentPrototype prototype) : base(prototype)
        {
        }
        
        public SegmentAdaptor NewAdaptor() => new();
        
        public virtual void OnEquipped(SegmentComponent component)
        {
        }

        public virtual void OnUnequipped(SegmentComponent component)
        {
        }
        
        public bool Equip(string equipment, EquipmentInstance? instance)
        {
            var layout = new SegmentLayout(Prototype);
            for (int i = 0, l = layout.equipment!.Count; i < l; i++)
            {
                if (layout.equipment[i].component!.Name == equipment)
                {
                    // TODO check for dependency segment
                    if (Array.IndexOf(layout.equipment[i].component!.Compatible, instance) != -1)
                    {
                        if (instance == null)
                        {
                            Equipment.Remove(equipment);
                        }
                        else
                        {
                            Equipment[equipment] = instance;
                        }
                        SubTreeChanged?.Invoke(this, equipment);
                        return true;
                    }
                }
            }
            return false;
        }
        
        public virtual VisualElement CreateSelector()
        {
            _selector = Prototype.selectorElement!.CloneTree();
            _selector.Q<Label>("name").text = Prototype.name;
            _selected = _selector.Q("selected");
            _unselected = _selector.Q("unselected");
            _hull = _selector.Q<RadialProgressBar>("hull");
            return _selector;
        }

        public virtual void UpdateSelector(SegmentComponent component, bool selected)
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
                        _hull.Value = Prototype.hp == 0 ? 0 : Mathf.Clamp01(Hp / Prototype.hp) / 4;
                        _hull.MarkDirtyRepaint();
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
        
        public virtual SegmentSerializable ToSerializable(SegmentComponent component)
        {
            return new SegmentSerializable();
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
