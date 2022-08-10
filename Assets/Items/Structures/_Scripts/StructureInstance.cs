#nullable enable
using System;
using System.Collections.Generic;
using DarkFrontier.Attributes;
using DarkFrontier.Controllers.New;
using DarkFrontier.Data.Values;
using DarkFrontier.Factions;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Game.Essentials;
using DarkFrontier.Items._Scripts;
using DarkFrontier.Items.Equipment;
using DarkFrontier.Items.Segments;
using DarkFrontier.Positioning.Sectors;
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

        [field: SerializeField, ReadOnly] [JsonProperty("position")]
        public Vector3 Position { get; private set; }
        
        [field: SerializeField, ReadOnly] [JsonProperty("rotation")]
        public Vector3 Rotation { get; private set; }

        [field: SerializeField, ReadOnly] [JsonProperty("rating")]
        public int Rating { get; set; }

        [field: SerializeField, ReadOnly] [JsonProperty("pool-hp-max")]
        public MutableFloat MaxHp { get; set; } = new(0);

        [field: SerializeField] [JsonProperty("pool-hp")]
        public float CurrentHp { get; set; }

        [field: SerializeField] [JsonProperty("hull-field-resist")]
        public MutableFloat HullFieldResist { get; private set; } = new(0);
        
        [field: SerializeField] [JsonProperty("hull-explosive-resist")]
        public MutableFloat HullExplosiveResist { get; private set; } = new(0);
        
        [field: SerializeField] [JsonProperty("hull-particle-resist")]
        public MutableFloat HullParticleResist { get; private set; } = new(0);
        
        [field: SerializeField] [JsonProperty("hull-kinetic-resist")]
        public MutableFloat HullKineticResist { get; private set; } = new(0);

        [field: SerializeField, ReadOnly]
        public SegmentComponent[] Segments { get; private set; } = Array.Empty<SegmentComponent>();
        
        [field: SerializeReference] [JsonProperty("segments")]
        public Dictionary<string, SegmentInstance> SegmentRecords { get; private set; } = new();

        public EquipmentComponent[] Equipment
        {
            get
            {
                List<EquipmentComponent> components = new();
                for(int i = 0, l = Segments.Length; i < l; i++)
                {
                    if(Segments[i].Instance != null)
                    {
                        components.AddRange(Segments[i].Instance!.Equipment);
                    }
                }
                return components.ToArray();
            }
        }

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
        
        [field: SerializeField] [JsonProperty("target-linear")]
        public Vector3 LinearTarget { get; set; }
        
        [field: SerializeField] [JsonProperty("target-angular")]
        public Vector3 AngularTarget { get; set; }
        
        [field: SerializeField] [JsonProperty("shielding")]
        public MutableFloat Shielding { get; private set; } = new(0);
        
        [field: SerializeField] [JsonProperty("reinforcement")]
        public MutableFloat Reinforcement { get; private set; } = new(0);
        
        [field: SerializeField] [JsonProperty("shield")]
        public float Shield { get; set; }
        
        [field: SerializeField] [JsonProperty("shield-field-resist")]
        public MutableFloat ShieldFieldResist { get; private set; } = new(0);
        
        [field: SerializeField] [JsonProperty("shield-explosive-resist")]
        public MutableFloat ShieldExplosiveResist { get; private set; } = new(0);
        
        [field: SerializeField] [JsonProperty("shield-particle-resist")]
        public MutableFloat ShieldParticleResist { get; private set; } = new(0);
        
        [field: SerializeField] [JsonProperty("shield-kinetic-resist")]
        public MutableFloat ShieldKineticResist { get; private set; } = new(0);
        
        [field: SerializeReference]
        public Faction? Faction { get; private set; }

        [field: SerializeField] [JsonProperty("faction-id")]
        public string FactionId { get; private set; } = string.Empty;

        [field: SerializeField]
        public SectorComponent? Sector { get; private set; }

        [field: SerializeField] [JsonProperty("sector-id")]
        public string SectorId { get; private set; } = string.Empty;
        
        [field: SerializeField]
        public ISelectable? Selected { get; set; }

        [field: SerializeField] [JsonProperty("selected-id")]
        public string SelectedId { get; private set; } = string.Empty;

        [field: SerializeReference] [JsonProperty("controller")]
        public Controller Controller { get; private set; } = new();
        
        private VisualElement _selector = null!;
        private VisualElement _selected = null!;
        private VisualElement _unselected = null!;
        private RadialProgressBar _hull = null!;
        private RadialProgressBar _shield = null!;
        
        public void ClearSegments() => Segments = Array.Empty<SegmentComponent>();

        public void FindSegments(GameObject gameObject) => Segments = gameObject.GetComponentsInChildren<SegmentComponent>();
        
        public StructureInstance()
        {
        }

        public StructureInstance(StructurePrototype prototype) : base(prototype)
        {
        }
        
        public virtual void Apply(StructureAuthoring authoring)
        {
            Id = authoring.id;
            Name = authoring.name;
            var transform = authoring.transform;
            Position = transform.localPosition;
            Rotation = transform.localEulerAngles;
            CurrentHp = authoring.hp;
            FactionId = authoring.faction;
            SectorId = authoring.sector;
            SelectedId = authoring.selected;
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
        
        public void ToSerialized(StructureComponent component)
        {
            ToSerialized();
            var transform = component.transform;
            Position = transform.localPosition;
            Rotation = transform.localEulerAngles;
        }
        
        public override void ToSerialized()
        {
            base.ToSerialized();
            FactionId = Faction?.Id ?? string.Empty;
            SectorId = Sector == null ? string.Empty : Sector.Instance?.Id ?? string.Empty;
            SelectedId = Selected == null ? string.Empty : Selected.Id;
            SegmentRecords.Clear();
            for(int i = 0, l = Segments.Length; i < l; i++)
            {
                if(Segments[i].Instance != null)
                {
                    SegmentRecords.Add(Segments[i].Name, Segments[i].Instance!);
                }
            }
        }

        public void FromSerialized(StructureComponent component)
        {
            FromSerialized();
            var transform = component.transform;
            transform.localPosition = Position;
            transform.localEulerAngles = Rotation;
        }

        public override void FromSerialized()
        {
            base.FromSerialized();
            Faction = string.IsNullOrEmpty(FactionId) ? Faction : Singletons.Get<FactionManager>().Registry.Find(FactionId);
            Sector = string.IsNullOrEmpty(SectorId) ? Sector : Singletons.Get<IdRegistry>().Get<SectorComponent>(SectorId);
            Selected = string.IsNullOrEmpty(SelectedId) ? Selected : Singletons.Get<IdRegistry>().Get<ISelectable>(SelectedId);
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