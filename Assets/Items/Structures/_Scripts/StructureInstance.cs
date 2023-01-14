#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using DarkFrontier.Attributes;
using DarkFrontier.Controllers.New;
using DarkFrontier.Data.Values;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Items._Scripts;
using DarkFrontier.Items.Segments;
using DarkFrontier.UI.Indicators.Modifiers;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Items.Structures
{
    [Serializable]
    public class StructureInstance : ItemInstance, IEquatable<StructureInstance>
    {
        public new StructurePrototype Prototype => (StructurePrototype)base.Prototype;

        public int Rating
        {
            get
            {
                var ret = 0;
                var segments = Segments.Values.ToArray();
                for (int i = 0, l = segments.Length; i < l; i++)
                {
                    ret += segments[i].Rating;
                }
                return ret;
            }
        }

        [field: SerializeField, ReadOnly] [JsonProperty("pool-hp-max")]
        public MutableFloat MaxHp { get; private set; } = new(0);

        [field: SerializeField] [JsonProperty("pool-hp")]
        public float CurrentHp { get; set; }

        [field: SerializeReference] [JsonProperty("segments")]
        public Dictionary<string, SegmentInstance> Segments { get; private set; } = new();

        [field: SerializeField] [JsonProperty("generation")]
        public MutableFloat Generation { get; private set; } = new(0);
        
        [field: SerializeField] [JsonProperty("capacitance")]
        public MutableFloat Capacitance { get; private set; } = new(0);
        
        [field: SerializeField] [JsonProperty("capacitor")]
        public float Capacitor { get; private set; }
        
        [field: SerializeField] [JsonProperty("speed-linear")]
        public MutableFloat LinearSpeed { get; private set; } = new(0);
        
        [field: SerializeField] [JsonProperty("speed-angular")]
        public MutableFloat AngularSpeed { get; private set; } = new(0);
        
        [field: SerializeField] [JsonProperty("acceleration-linear")]
        public MutableFloat LinearAcceleration { get; private set; } = new(0);
        
        [field: SerializeField] [JsonProperty("acceleration-angular")]
        public MutableFloat AngularAcceleration { get; private set; } = new(0);

        [field: SerializeField] [JsonProperty("shielding")]
        public MutableFloat Shielding { get; private set; } = new(0);
        
        [field: SerializeField] [JsonProperty("reinforcement")]
        public MutableFloat Reinforcement { get; private set; } = new(0);
        
        [field: SerializeField] [JsonProperty("shield")]
        public float Shield { get; set; }
        
        [field: SerializeField] [JsonProperty("hull-field-resist")]
        public MutableFloat HullFieldResist { get; private set; } = new(0);
        
        [field: SerializeField] [JsonProperty("hull-explosive-resist")]
        public MutableFloat HullExplosiveResist { get; private set; } = new(0);
        
        [field: SerializeField] [JsonProperty("hull-particle-resist")]
        public MutableFloat HullParticleResist { get; private set; } = new(0);
        
        [field: SerializeField] [JsonProperty("hull-kinetic-resist")]
        public MutableFloat HullKineticResist { get; private set; } = new(0);
        
        [field: SerializeField] [JsonProperty("shield-field-resist")]
        public MutableFloat ShieldFieldResist { get; private set; } = new(0);
        
        [field: SerializeField] [JsonProperty("shield-explosive-resist")]
        public MutableFloat ShieldExplosiveResist { get; private set; } = new(0);
        
        [field: SerializeField] [JsonProperty("shield-particle-resist")]
        public MutableFloat ShieldParticleResist { get; private set; } = new(0);
        
        [field: SerializeField] [JsonProperty("shield-kinetic-resist")]
        public MutableFloat ShieldKineticResist { get; private set; } = new(0);

        public event EventHandler<string>? SubTreeChanged;

        private VisualElement _selector = null!;
        private VisualElement _selected = null!;
        private VisualElement _unselected = null!;
        private RadialProgressBar _hull = null!;
        private RadialProgressBar _shield = null!;
        
        public StructureInstance()
        {
        }

        public StructureInstance(StructurePrototype prototype) : base(prototype)
        {
        }

        public StructureAdaptor NewAdaptor() => new();
        
        public bool Equip(string segment, SegmentInstance? instance)
        {
            var layout = new StructureLayout(Prototype);
            for (int i = 0, l = layout.segments!.Count; i < l; i++)
            {
                if (layout.segments[i].component!.Name == segment)
                {
                    // TODO check for dependency segment
                    if (Array.IndexOf(layout.segments[i].component!.Compatible, instance) != -1)
                    {
                        if (instance == null)
                        {
                            Segments.Remove(segment);
                        }
                        else
                        {
                            Segments[segment] = instance;
                        }
                        UpdateMaxHp();
                        SubTreeChanged?.Invoke(this, segment);
                        return true;
                    }
                }
            }
            return false;
        }

        private void UpdateMaxHp()
        {
            MaxHp.baseValue = 0;
            var segments = Segments.Values.ToArray();
            for (int i = 0, l = segments.Length; i < l; i++)
            {
                MaxHp.baseValue += segments[i].Prototype.poolHp;
            }
        }

        public virtual VisualElement CreateSelector()
        {
            _selector = Prototype.selectorElement!.CloneTree();
            _selector.Q<Label>("name").text = Prototype.name;
            _selected = _selector.Q("selected");
            _unselected = _selector.Q("unselected");
            _hull = _selector.Q<RadialProgressBar>("hull");
            _shield = _selector.Q<RadialProgressBar>("shield");
            return _selector;
        }

        public virtual void UpdateSelector(StructureComponent component, bool selected)
        {
            if (component == Singletons.Get<PlayerController>().Player)
            {
                _selector.style.visibility = Visibility.Hidden;
            }
            else
            {
                var position = component.camera.WorldToViewportPoint(component.transform.position);
                if(position.z > 0)
                {
                    _selector.style.visibility = Visibility.Visible;
                    _selector.style.left = new StyleLength(new Length(position.x * 100, LengthUnit.Percent));
                    _selector.style.top = new StyleLength(new Length(100 - position.y * 100, LengthUnit.Percent));
                
                    if(selected)
                    {
                        _selected.style.visibility = Visibility.Visible;
                        _unselected.style.visibility = Visibility.Hidden;
                        _unselected.pickingMode = PickingMode.Ignore;
                        _hull.Value = MaxHp == 0 ? 0 : Mathf.Clamp01(CurrentHp / MaxHp) / 4;
                        _shield.Value = Shielding.Value == 0 ? 0 : Mathf.Clamp01(Shield / Shielding.Value) / 4;
                        _hull.MarkDirtyRepaint();
                        _shield.MarkDirtyRepaint();
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
        
        public virtual StructureSerializable ToSerializable(StructureComponent component)
        {
            var t = component.transform;
            return new StructureSerializable
            {
                instance = this,
                position = t.localPosition,
                rotation = t.localEulerAngles
            };
        }
        
        public bool Equals(StructureInstance? other)
        {
            return other != null && ReferenceEquals(this, other);
        }

        public override bool Equals(object? obj)
        {
            if(ReferenceEquals(null, obj)) return false;
            if(ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((StructureInstance)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}