#nullable enable
using System;
using DarkFrontier.Attributes;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Items.Structures;
using DarkFrontier.UI.Indicators.Selectors;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Items.Segments
{
    public class SegmentComponent : MonoBehaviour, IDetectable
    {
        [field: SerializeReference, ReadOnly]
        public StructureComponent? Structure { get; private set; }

        [field: SerializeReference]
        public SegmentInstance? Instance { get; private set; }

        [field: SerializeReference]
        public string Name { get; private set; } = "";

        [field: SerializeReference, TextArea]
        public string Description { get; private set; } = "";

        [field: SerializeReference]
        public SegmentPrototype[] Compatible { get; private set; } = Array.Empty<SegmentPrototype>();
        
        private DetectableRegistry _detectableRegistry = null!;
        
        private bool _initialized;
        private bool _registered;
        private bool _enabled;
        
        public void Initialize(StructureComponent component)
        {
            if(_initialized) return;
            
            Structure = component;

            _detectableRegistry = Singletons.Get<DetectableRegistry>();

            _initialized = true;
        }
        
        public void Set(SegmentInstance? instance)
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
            
            Instance.FindEquipment(gameObject);
            for(int i = 0, li = Instance.Equipment.Length; i < li; i++)
            {
                Instance.Equipment[i].Initialize(this);
                for(int j = 0, lj = Instance.EquipmentRecords.Length; j < lj; j++)
                {
                    if(Instance.Equipment[i].Name == Instance.EquipmentRecords[j]?.Name)
                    {
                        Instance.Equipment[j].Set(Instance.EquipmentRecords[j]!.Instance);
                        Instance.Equipment[i].Enable();
                    }
                }
            }
            
            _detectableRegistry.Detectables.Add(this);
            
            _enabled = true;
        }

        private void Disable()
        {
            if(!_initialized || !_enabled || Instance == null) return;

            for(int i = 0, l = transform.childCount; i < l; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            
            Instance.ClearEquipment();
            
            _detectableRegistry.Detectables.Remove(this);
            
            Instance.ToSerialized();

            _enabled = false;
        }
        
        public bool IsDetected(StructureInstance structure)
        {
            return Structure != null && Structure.IsDetected(structure);
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