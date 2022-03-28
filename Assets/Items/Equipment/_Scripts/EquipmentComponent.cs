#nullable enable
using System;
using DarkFrontier.Attributes;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Items.Segments;
using DarkFrontier.Items.Structures;
using DarkFrontier.UI.Indicators.Selectors;
using DarkFrontier.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Items.Equipment
{
    public class EquipmentComponent : MonoBehaviour, IDetectable
    {
        [field: SerializeField, ReadOnly]
        public SegmentComponent? Segment { get; private set; }

        [field: SerializeReference]
        public EquipmentInstance? Instance { get; private set; }
        
        [field: SerializeField]
        public string Name { get; private set; } = "";

        [field: SerializeField, TextArea]
        public string Description { get; private set; } = "";

        [field: SerializeField]
        public EquipmentPrototype[] Compatible { get; private set; } = Array.Empty<EquipmentPrototype>();

        private DetectableRegistry _detectableRegistry = null!;
        
        [SerializeField, ReadOnly]
        private bool _initialized;
        
        [SerializeField, ReadOnly]
        private bool _registered;
        
        [SerializeField, ReadOnly]
        private bool _enabled;
        
        public void Initialize(SegmentComponent component)
        {
            if(_initialized) return;
            Segment = component;
            _detectableRegistry = Singletons.Get<DetectableRegistry>();
            _initialized = true;
        }

        public void Set(EquipmentInstance? instance)
        {
            Disable();
            Unregister();
            Instance = instance;
            Register();
        }
        
        private void Register()
        {
            if(!_initialized || _registered || Instance == null) return;
            _registered = true;
        }

        private void Unregister()
        {
            if(!_initialized || !_registered || Instance == null) return;
            _registered = false;
        }

        public void Enable()
        {
            if(!_initialized || _enabled || Instance == null) return;
            Instance.FromSerialized();
            if(Instance.Prototype.prefab != null)
            {
                Instantiate(Instance.Prototype.prefab, transform);
            }
            _detectableRegistry.Detectables.Add(this);
            _enabled = true;
        }

        private void Disable()
        {
            if(!_initialized || !_enabled || Instance == null) return;
            transform.DestroyChildren();
            _detectableRegistry.Detectables.Remove(this);
            Instance.ToSerialized();
            _enabled = false;
        }

        public void Equip(EquipmentInstance? instance)
        {
            Instance?.OnUnequipped(this);
            Set(instance);
            Enable();
            Instance?.OnEquipped(this);
        }
        
        public bool IsDetected(StructureInstance structure)
        {
            return Segment != null && Segment.IsDetected(structure);
        }

        public VisualElement CreateSelector()
        {
            return Instance == null || Instance.Prototype.selectorElement == null ? new VisualElement() : Instance.Prototype.selectorElement.CloneTree();
        }

        public Vector3 GetSelectorPosition()
        {
            return UnityEngine.Camera.main!.WorldToViewportPoint(transform.position);
        }
        
        public VisualElement CreateSelected()
        {
            return new VisualElement();
        }
    }
}
