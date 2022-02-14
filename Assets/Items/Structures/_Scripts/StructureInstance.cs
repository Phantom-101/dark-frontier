#nullable enable
using System;
using DarkFrontier.Attributes;
using DarkFrontier.Factions;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Items._Scripts;
using DarkFrontier.Items.Equipment._Scripts;
using DarkFrontier.Items.Segments;
using DarkFrontier.Positioning.Sectors;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Items.Structures
{
    public class StructureInstance : ItemInstance, IEquatable<StructureInstance>, IDetectable
    {
        [field: SerializeReference, ReadOnly]
        public StructureComponent? Component { get; private set; }

        public new StructurePrototype Prototype => (StructurePrototype)base.Prototype;

        [field: SerializeReference, ReadOnly]
        public float MaxHp { get; private set; }

        [field: SerializeReference]
        [JsonProperty("pool-hp")]
        public float CurrentHp { get; private set; }

        [field: SerializeReference]
        [JsonProperty("segments")]
        public SegmentRecord?[] Segments { get; set; } = Array.Empty<SegmentRecord?>();

        [field: SerializeReference]
        public Faction? Faction { get; private set; }

        [JsonProperty("faction-id")]
        private string _factionId = "";

        [field: SerializeReference]
        public SectorInstance? Sector { get; private set; }

        [JsonProperty("sector-id")]
        private string _sectorId = "";

        [field: SerializeReference]
        public StructureInstance? Selected { get; private set; }

        [JsonProperty("selected-id")]
        private string _selectedId = "";

        public StructureInstance()
        {
        }

        public StructureInstance(StructurePrototype prototype) : base(prototype)
        {
        }

        public bool SetComponent(StructureComponent component)
        {
            Component = component;
            return true;
        }
        
        public void ChangeSegment(SegmentComponent segmentComponent, SegmentInstance newInstance)
        {
            segmentComponent.RemoveInstance();
            if(newInstance.Component != null)
            {
                newInstance.Component.RemoveInstance();
            }
            segmentComponent.SetInstance(newInstance);
            UpdateMaxHp();
        }

        public void ChangeEquipment(EquipmentComponent equipmentComponent, EquipmentInstance newInstance)
        {
            
        }

        public void UpdateMaxHp()
        {
            MaxHp = 0;
            if(Component == null) return;
            for(int i = 0, l = Component.Segments.Length; i < l; i++)
            {
                MaxHp += Component.Segments[i].Instance?.Prototype.hp ?? 0;
            }
        }

        public bool IsDetected(StructureInstance structure)
        {
            return true;
        }

        public VisualElement CreateSelector()
        {
            return Prototype.selectorElement == null ? new VisualElement() : Prototype.selectorElement.CloneTree();
        }

        public Vector3 GetSelectorPosition()
        {
            return Component == null ? Vector3.zero : UnityEngine.Camera.main!.WorldToViewportPoint(Component.transform.position);
        }

        public VisualElement CreateSelected()
        {
            return new VisualElement();
        }

        public override void PreSerialize()
        {
            base.PreSerialize();
            _factionId = Faction?.Id ?? "";
            _sectorId = "";
            _selectedId = Selected == null ? "" : Selected?.Id ?? "";
            
            if(Component == null) return;
            int l;
            Segments = new SegmentRecord?[l = Component.Segments.Length];
            for(var i = 0; i < l; i++)
            {
                if(Component.Segments[i].Instance != null)
                {
                    Component.Segments[i].Instance!.PreSerialize();
                    Segments[i] = new SegmentRecord(Component.Segments[i].Name, Component.Segments[i].Instance);
                }
            }
        }

        public override void PostDeserialize()
        {
            base.PostDeserialize();
            Faction = _factionId.Length > 0 ? Singletons.Get<FactionManager>().Registry.Find(_factionId) : Faction;
            Sector = _sectorId.Length > 0 ? null : Sector;
            Selected = _selectedId.Length > 0 ? null : Selected;

            if(Component == null) return;
            for(int i = 0, l = Component.Segments.Length; i < l; i++)
            {
                Component.Segments[i].Instance?.PostDeserialize();
            }
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