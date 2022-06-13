#nullable enable
using System;
using System.Linq;
using DarkFrontier.Attributes;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Game.Essentials;
using DarkFrontier.Items.Equipment;
using DarkFrontier.Items.Structures;
using DarkFrontier.UI.Indicators.Selectors;
using DarkFrontier.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Items.Segments
{
    public class SegmentComponent : MonoBehaviour, ISelectable
    {
        [field: SerializeReference, ReadOnly]
        public StructureComponent? Structure { get; private set; }

        [field: SerializeReference]
        public SegmentInstance? Instance { get; private set; }

        public string Id => Instance?.Id ?? string.Empty;
        
        [field: SerializeReference]
        public string Name { get; private set; } = "";

        [field: SerializeReference, TextArea]
        public string Description { get; private set; } = "";

        [field: SerializeReference]
        public SegmentPrototype[] Compatible { get; private set; } = Array.Empty<SegmentPrototype>();
        
        [field: SerializeField]
        public bool Required { get; private set; }
        
        [field: SerializeField]
        public SegmentComponent? Dependency { get; private set; }
        
        [SerializeField, ReadOnly]
        private bool _initialized;
        
        [SerializeField, ReadOnly]
        private bool _registered;
        
        [SerializeField, ReadOnly]
        private bool _enabled;
        
        private IdRegistry _idRegistry = null!;
        private DetectableRegistry _detectableRegistry = null!;

        [ReadOnly]
        public new UnityEngine.Camera camera = null!;
        
        public bool SelectorDirty { get; private set; }
        
        public void Initialize(StructureComponent component)
        {
            if(_initialized) return;
            Structure = component;
            _idRegistry = Singletons.Get<IdRegistry>();
            _detectableRegistry = Singletons.Get<DetectableRegistry>();
            camera = Singletons.Get<UnityEngine.Camera>();
            _initialized = true;
        }

        public void Equip(SegmentInstance? instance)
        {
            if(instance == null && Required || instance != null && (Dependency != null && Dependency.Instance == null || Compatible.Length != 0 && !Compatible.Contains(instance.Prototype))) return;
            instance?.OnUnequipped(this);
            Set(instance);
            Enable();
            instance?.OnEquipped(this);
        }
        
        public void Set(SegmentInstance? instance)
        {
            if(instance == null && Required || instance != null && (Dependency != null && Dependency.Instance == null || Compatible.Length != 0 && !Compatible.Contains(instance.Prototype))) return;
            Disable();
            Unregister();
            Instance = instance;
            SelectorDirty = true;
            Register();
        }
        
        private void Register()
        {
            if(!_initialized || _registered || Instance == null) return;
            _idRegistry.Register(this);
            _registered = true;
        }

        private void Unregister()
        {
            if(!_initialized || !_registered || Instance == null) return;
            _idRegistry.Unregister(this);
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
            Instance.FindEquipment(gameObject);
            for(int i = 0, li = Instance.Equipment.Length; i < li; i++)
            {
                Instance.Equipment[i].Initialize(this);
                if(Instance.EquipmentRecords.ContainsKey(Instance.Equipment[i].Name))
                {
                    Instance.Equipment[i].Set(Instance.EquipmentRecords[Instance.Equipment[i].Name]);
                    Instance.Equipment[i].Enable();
                }
            }
            _detectableRegistry.Register(this);
            _enabled = true;
        }

        private void Disable()
        {
            if(!_initialized || !_enabled || Instance == null) return;
            transform.DestroyChildren();
            Instance.ClearEquipment();
            _detectableRegistry.Unregister(this);
            Instance.ToSerialized();
            _enabled = false;
        }
        
        public void Equip(string equipment, EquipmentInstance? instance)
        {
            if(!_initialized || !_enabled || Instance == null) return;
            for(int i = 0, l = Instance.Equipment.Length; i < l; i++)
            {
                var equipmentComponent = Instance.Equipment[i];
                if(equipmentComponent.Name == equipment)
                {
                    equipmentComponent.Equip(instance);
                    break;
                }
            }
        }

        public void Tick(float deltaTime)
        {
            if(Instance == null) return;
            
            for(int i = 0, l = Instance.Equipment.Length; i < l; i++)
            {
                Instance.Equipment[i].Tick(deltaTime);
            }
        }
        
        public bool CanBeSelectedBy(StructureComponent other)
        {
            return Structure != null && Structure.CanBeSelectedBy(other);
        }

        public VisualElement CreateSelector()
        {
            SelectorDirty = false;
            return Instance?.CreateSelector() ?? new VisualElement();
        }

        public void UpdateSelector(bool selected)
        {
            Instance?.UpdateSelector(this, selected);
        }
    }
}