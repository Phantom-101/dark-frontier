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
    public class EquipmentComponent : MonoBehaviour, ISelectable
    {
        [field: SerializeField, ReadOnly]
        public SegmentComponent? Segment { get; private set; }

        public StructureComponent? Structure => Segment == null ? null : Segment.Structure;

        [field: SerializeReference]
        public EquipmentInstance? Instance { get; private set; }

        public EquipmentAdaptor? Adaptor
        {
            get => _adaptor;
            private set
            {
                _adaptor = value;
                if (_adaptor != null)
                {
                    _adaptor.slot = this;
                }
            }
        }

        [SerializeReference]
        private EquipmentAdaptor? _adaptor;

        public string Id => Adaptor!.id;
        
        [field: SerializeField]
        public string Name { get; private set; } = "";

        [field: SerializeField, TextArea]
        public string Description { get; private set; } = "";

        [field: SerializeField]
        public EquipmentPrototype[] Compatible { get; private set; } = Array.Empty<EquipmentPrototype>();

        private IdRegistry _idRegistry = null!;
        private DetectableRegistry _detectableRegistry = null!;
        
        [ReadOnly]
        public new UnityEngine.Camera camera = null!;

        public bool SelectorDirty { get; private set; }
        public bool IndicatorDirty { get; private set; }

        public void Set(EquipmentInstance? instance)
        {
            _idRegistry = Singletons.Get<IdRegistry>();
            _detectableRegistry = Singletons.Get<DetectableRegistry>();
            
            if (Instance != null)
            {
                transform.DestroyChildren();
            
                _idRegistry.Unregister(this);
                _detectableRegistry.Unregister(this);

                Adaptor = null;
            }
            
            Instance = instance;
            
            if (Instance != null)
            {
                Adaptor = Instance.NewAdaptor();
                
                _idRegistry.Register(Id, this);
                _detectableRegistry.Register(this);
                
                if(Instance.Prototype.prefab != null)
                {
                    Instantiate(Instance.Prototype.prefab, transform);
                }
                
                Segment = GetComponentInParent<SegmentComponent>();
                camera = Singletons.Get<UnityEngine.Camera>();
            }
        }
        
        public void Tick(float deltaTime)
        {
            if(Instance == null) return;
            
            Instance.OnTick(this, deltaTime);
        }
        
        public bool CanBeSelectedBy(StructureComponent other)
        {
            return Segment != null && Segment.CanBeSelectedBy(other);
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

        public VisualElement CreateIndicator()
        {
            IndicatorDirty = false;
            return Instance?.CreateIndicator() ?? new VisualElement();
        }

        public void UpdateIndicator()
        {
            Instance?.UpdateIndicator(this);
        }
    }
}
