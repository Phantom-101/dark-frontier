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
using DarkFrontier.Items.Segments;
using DarkFrontier.Positioning.Sectors;
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

        [field: SerializeField, ReadOnly] [JsonProperty("pool-hp-max")]
        public float MaxHp { get; private set; }

        [field: SerializeField] [JsonProperty("pool-hp")]
        public float CurrentHp { get; private set; }
        
        [field: SerializeField, ReadOnly]
        public SegmentComponent[] Segments { get; private set; } = Array.Empty<SegmentComponent>();
        
        [field: SerializeReference] [JsonProperty("segments")]
        public Dictionary<string, SegmentInstance> SegmentRecords { get; private set; } = new();

        [field: SerializeReference] [JsonProperty("generation")]
        public MutableValue<float> Generation { get; private set; } = new(0);
        
        [field: SerializeReference] [JsonProperty("capacitance")]
        public MutableValue<float> Capacitance { get; private set; } = new(0);
        
        [field: SerializeReference] [JsonProperty("capacitor")]
        public float Capacitor { get; private set; }
        
        [field: SerializeReference] [JsonProperty("speed-linear")]
        public MutableValue<float> LinearSpeed { get; private set; } = new(0);
        
        [field: SerializeReference] [JsonProperty("speed-angular")]
        public MutableValue<float> AngularSpeed { get; private set; } = new(0);
        
        [field: SerializeReference] [JsonProperty("acceleration-linear")]
        public MutableValue<float> LinearAcceleration { get; private set; } = new(0);
        
        [field: SerializeReference] [JsonProperty("acceleration-angular")]
        public MutableValue<float> AngularAcceleration { get; private set; } = new(0);
        
        [field: SerializeReference] [JsonProperty("target-linear")]
        public Vector3 LinearTarget { get; set; }
        
        [field: SerializeReference] [JsonProperty("target-angular")]
        public Vector3 AngularTarget { get; set; }
        
        [field: SerializeReference] [JsonProperty("shielding")]
        public MutableValue<float> Shielding { get; private set; } = new(0);
        
        [field: SerializeReference] [JsonProperty("reinforcement")]
        public MutableValue<float> Reinforcement { get; private set; } = new(0);
        
        [field: SerializeReference] [JsonProperty("shield")]
        public float Shield { get; private set; }
        
        [field: SerializeReference]
        public Faction? Faction { get; private set; }

        [field: SerializeField] [JsonProperty("faction-id")]
        public string FactionId { get; private set; } = "";

        [field: SerializeReference]
        public SectorComponent? Sector { get; private set; }

        [field: SerializeField] [JsonProperty("sector-id")]
        public string SectorId { get; private set; } = "";
        
        [field: SerializeReference]
        public ISelectable? Selected { get; set; }

        [field: SerializeField] [JsonProperty("selected-id")]
        public string SelectedId { get; private set; } = "";

        [field: SerializeReference] [JsonProperty("controller")]
        public Controller Controller { get; private set; } = new();
        
        private VisualElement _selector = null!;
        private VisualElement _selected = null!;
        private VisualElement _unselected = null!;
        
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
                
                _selected.style.visibility = selected ? Visibility.Visible : Visibility.Hidden;
                _unselected.style.visibility = selected ? Visibility.Hidden : Visibility.Visible;
                _unselected.pickingMode = selected ? PickingMode.Ignore : PickingMode.Position;
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
            FactionId = Faction?.Id ?? "";
            SectorId = Sector == null ? "" : Sector.Instance?.Id ?? "";
            SelectedId = Selected == null ? "" : Selected.Id;
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
            Faction = FactionId.Length > 0 ? Singletons.Get<FactionManager>().Registry.Find(FactionId) : Faction;
            Sector = SectorId.Length > 0 ? null : Sector;
            Selected = SelectedId.Length > 0 ? Singletons.Get<IdRegistry>().Get<ISelectable>(SelectedId) : Selected;
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