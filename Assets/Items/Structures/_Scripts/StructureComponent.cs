#nullable enable
using DarkFrontier.Attributes;
using DarkFrontier.Foundation.Services;
using DarkFrontier.UI.Indicators.Selectors;
using DarkFrontier.Utils;
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
        
        [SerializeField, ReadOnly]
        private bool _initialized;
        
        [SerializeField, ReadOnly]
        private bool _registered;
        
        [SerializeField, ReadOnly]
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
            Instance.FromSerialized(this);
            if(Instance.Prototype.prefab != null)
            {
                Instantiate(Instance.Prototype.prefab, transform);
            }
            Instance.FindSegments(gameObject);
            for(int i = 0, li = Instance.Segments.Length; i < li; i++)
            {
                Instance.Segments[i].Initialize(this);
                if(Instance.SegmentRecords.ContainsKey(Instance.Segments[i].Name))
                {
                    Instance.Segments[i].Set(Instance.SegmentRecords[Instance.Segments[i].Name]);
                    Instance.Segments[i].Enable();
                }
            }
            _detectableRegistry.Detectables.Add(this);
            _enabled = true;
        }

        private void Disable()
        {
            if(!_initialized || !_enabled || Instance == null) return;
            transform.DestroyChildren();
            Instance.ClearSegments();
            _detectableRegistry.Detectables.Remove(this);
            Instance.ToSerialized(this);
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