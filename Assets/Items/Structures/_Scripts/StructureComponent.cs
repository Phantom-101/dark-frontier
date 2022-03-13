#nullable enable
using DarkFrontier.Foundation.Services;
using DarkFrontier.UI.Indicators.Selectors;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Items.Structures
{
    public class StructureComponent : MonoBehaviour, IDetectable
    {
        [field: SerializeReference]
        public StructureInstance? Instance { get; private set; }

        private StructureRegistry _structureRegistry = null!;
        private DetectableRegistry _detectableRegistry = null!;
        
        private bool _initialized;
        private bool _registered;
        private bool _enabled;
        
        public void Initialize()
        {
            if(_initialized) return;
            
            _structureRegistry = Singletons.Get<StructureRegistry>();
            _detectableRegistry = Singletons.Get<DetectableRegistry>();
            
            _initialized = true;
        }

        public void Set(StructureInstance? instance)
        {
            Disable();
            Unregister();

            Instance = instance;
            
            Register();
        }
        
        private void Register()
        {
            if(!_initialized || _registered || Instance == null) return;
            
            _structureRegistry.Add(this);
            
            _registered = true;
        }

        private void Unregister()
        {
            if(!_initialized || !_registered || Instance == null) return;
            
            _structureRegistry.Remove(this);
            
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
            
            Instance.FindSegments(gameObject);
            for(int i = 0, li = Instance.Segments.Length; i < li; i++)
            {
                Instance.Segments[i].Initialize(this);
                for(int j = 0, lj = Instance.SegmentRecords.Length; j < lj; j++)
                {
                    if(Instance.Segments[i].Name == Instance.SegmentRecords[j]?.Name)
                    {
                        Instance.Segments[i].Set(Instance.SegmentRecords[j]!.Instance);
                        Instance.Segments[i].Enable();
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
            
            Instance.ClearSegments();
            
            _detectableRegistry.Detectables.Remove(this);
            
            Instance.ToSerialized();

            _enabled = false;
        }
        
        public bool IsDetected(StructureInstance structure)
        {
            return true;
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