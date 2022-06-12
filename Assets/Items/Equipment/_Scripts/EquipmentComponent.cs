#nullable enable
using System;
using System.Linq;
using DarkFrontier.Attributes;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Game.Essentials;
using DarkFrontier.Items.Segments;
using DarkFrontier.Items.Structures;
using DarkFrontier.UI.Indicators.Selectors;
using DarkFrontier.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Items.Equipment
{
    public class EquipmentComponent : MonoBehaviour, IId, IDetectable
    {
        [field: SerializeField, ReadOnly]
        public SegmentComponent? Segment { get; private set; }

        public StructureComponent? Structure => Segment == null ? null : Segment.Structure;

        [field: SerializeReference]
        public EquipmentInstance? Instance { get; private set; }

        public string Id => Instance?.Id ?? string.Empty;
        
        [field: SerializeField]
        public string Name { get; private set; } = "";

        [field: SerializeField, TextArea]
        public string Description { get; private set; } = "";

        [field: SerializeField]
        public EquipmentPrototype[] Compatible { get; private set; } = Array.Empty<EquipmentPrototype>();

        [SerializeField, ReadOnly]
        private bool _initialized;
        
        [SerializeField, ReadOnly]
        private bool _registered;
        
        [SerializeField, ReadOnly]
        private bool _enabled;
        
        private IdRegistry _idRegistry = null!;
        private DetectableRegistry _detectableRegistry = null!;
        private UnityEngine.Camera _camera = null!;

        public void Initialize(SegmentComponent component)
        {
            if(_initialized) return;
            Segment = component;
            _idRegistry = Singletons.Get<IdRegistry>();
            _detectableRegistry = Singletons.Get<DetectableRegistry>();
            _camera = Singletons.Get<UnityEngine.Camera>();
            _initialized = true;
        }

        public void Equip(EquipmentInstance? instance)
        {
            if(instance != null && Compatible.Length != 0 && !Compatible.Contains(instance.Prototype)) return;
            instance?.OnUnequipped(this);
            Set(instance);
            Enable();
            instance?.OnEquipped(this);
        }
        
        public void Set(EquipmentInstance? instance)
        {
            if(instance != null && Compatible.Length != 0 && !Compatible.Contains(instance.Prototype)) return;
            Disable();
            Unregister();
            Instance = instance;
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
            _detectableRegistry.Register(this);
            _enabled = true;
        }

        private void Disable()
        {
            if(!_initialized || !_enabled || Instance == null) return;
            transform.DestroyChildren();
            _detectableRegistry.Unregister(this);
            Instance.ToSerialized();
            _enabled = false;
        }
        
        public void Tick(float deltaTime)
        {
            if(Instance == null) return;
            
            Instance.OnTick(this, deltaTime);
        }
        
        public bool IsDetectedBy(StructureComponent structure)
        {
            return Segment != null && Segment.IsDetectedBy(structure);
        }

        public VisualElement CreateSelector()
        {
            var element = Instance!.Prototype.selectorElement!.CloneTree();
            element.Q("selected").Q<Label>("name").text = Instance?.Prototype.name ?? "";
            return element;
        }

        public void UpdateSelector(VisualElement selector, bool selected)
        {
            var position = _camera.WorldToViewportPoint(transform.position);
            if(position.z > 0)
            {
                selector.style.visibility = Visibility.Visible;
                selector.style.left = new StyleLength(new Length(position.x * 100, LengthUnit.Percent));
                selector.style.top = new StyleLength(new Length(100 - position.y * 100, LengthUnit.Percent));
                    
                selector.Q("selected").style.visibility = selected ? Visibility.Visible : Visibility.Hidden;
                selector.Q("unselected").style.visibility = selected ? Visibility.Hidden : Visibility.Visible;
                selector.Q("unselected").pickingMode = selected ? PickingMode.Ignore : PickingMode.Position;
            }
            else
            {
                selector.style.visibility = Visibility.Hidden;
            }
        }
    }
}
