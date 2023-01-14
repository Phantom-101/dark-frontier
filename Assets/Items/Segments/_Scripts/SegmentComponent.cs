#nullable enable
using System;
using DarkFrontier.Attributes;
using DarkFrontier.Foundation.Extensions;
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
        public StructureComponent Structure { get; private set; } = null!;

        [field: SerializeReference]
        public SegmentInstance? Instance { get; private set; }

        public SegmentAdaptor? Adaptor
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
        private SegmentAdaptor? _adaptor;

        public string Id => Adaptor!.id;
        
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

        public bool SelectorDirty { get; private set; }

        [ReadOnly]
        public new UnityEngine.Camera camera = null!;
        
        public EquipmentComponent[] Equipment { get; private set; } = null!;
        
        private IdRegistry _idRegistry = null!;
        private DetectableRegistry _detectableRegistry = null!;

        public void Set(SegmentInstance? instance)
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
                
                Structure = GetComponentInParent<StructureComponent>();
                Equipment = GetComponentsInChildren<EquipmentComponent>();
                camera = Singletons.Get<UnityEngine.Camera>();
                
                for(int i = 0, li = Equipment.Length; i < li; i++)
                {
                    Equipment[i].Set(Instance.Equipment.TryGet(Equipment[i].Name, null));
                }
            }
        }

        public void Tick(float deltaTime)
        {
            if(Instance == null) return;
            
            for(int i = 0, l = Equipment.Length; i < l; i++)
            {
                Equipment[i].Tick(deltaTime);
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