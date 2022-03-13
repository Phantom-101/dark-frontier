#nullable enable
using DarkFrontier.Foundation.Services;
using DarkFrontier.Items.Structures;
using DarkFrontier.UI.Indicators.Selectors;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace DarkFrontier.Positioning.Sectors
{
    public class SectorComponent : MonoBehaviour, IDetectable
    {
        [field: SerializeReference]
        public SectorInstance? Instance { get; private set; }

        private DetectableRegistry _detectableRegistry = null!;
        
        private bool _initialized;
        private bool _registered;
        private bool _enabled;
        
        public void Initialize()
        {
            if(_initialized) return;
            
            _detectableRegistry = Singletons.Get<DetectableRegistry>();
            
            _initialized = true;
        }

        public void Set(SectorInstance? instance)
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
            
            SectorSpawner.Create(this);
            
            _detectableRegistry.Detectables.Add(this);
            
            _enabled = true;
        }

        private void Disable()
        {
            if(!_initialized || !_enabled || Instance == null) return;

            _detectableRegistry.Detectables.Remove(this);
            
            _enabled = false;
        }
        
        public bool IsDetected(StructureInstance structure)
        {
            return structure.Sector != this;
        }

        public VisualElement CreateSelector()
        {
            return Addressables.LoadAssetAsync<VisualTreeAsset>(Instance?.SelectorAddressableKey ?? "").WaitForCompletion().CloneTree();
        }

        public Vector3 GetSelectorPosition()
        {
            return Instance == null ? Vector3.zero : UnityEngine.Camera.main!.WorldToViewportPoint(Instance.Position);
        }

        public VisualElement CreateSelected()
        {
            return new VisualElement();
        }
    }
}
