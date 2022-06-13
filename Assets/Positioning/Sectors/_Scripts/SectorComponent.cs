#nullable enable
using DarkFrontier.Attributes;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Game.Essentials;
using DarkFrontier.Items.Structures;
using DarkFrontier.UI.Indicators.Selectors;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace DarkFrontier.Positioning.Sectors
{
    public class SectorComponent : MonoBehaviour, ISelectable
    {
        [field: SerializeReference]
        public SectorInstance? Instance { get; private set; }

        public string Id => Instance?.Id ?? string.Empty;
        
        [SerializeField, ReadOnly]
        private bool _initialized;
        
        [SerializeField, ReadOnly]
        private bool _registered;
        
        [SerializeField, ReadOnly]
        private bool _enabled;
        
        [SerializeField, ReadOnly]
        private float _physicsTimer;
        
        private IdRegistry _idRegistry = null!;
        private SectorRegistry _sectorRegistry = null!;
        private DetectableRegistry _detectableRegistry = null!;
        
        [ReadOnly]
        public new UnityEngine.Camera camera = null!;

        public bool SelectorDirty => false;
        
        public void Initialize()
        {
            if(_initialized) return;
            _idRegistry = Singletons.Get<IdRegistry>();
            _sectorRegistry = Singletons.Get<SectorRegistry>();
            _detectableRegistry = Singletons.Get<DetectableRegistry>();
            camera = Singletons.Get<UnityEngine.Camera>();
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
            SectorSpawner.Create(this);
            _sectorRegistry.Register(this);
            _detectableRegistry.Register(this);
            Instance.UpdatePathfinder(gameObject);
            _enabled = true;
        }

        private void Disable()
        {
            if(!_initialized || !_enabled || Instance == null) return;
            _sectorRegistry.Unregister(this);
            _detectableRegistry.Unregister(this);
            _enabled = false;
        }

        public void Tick(float deltaTime)
        {
            if(!_initialized || !_enabled || Instance == null) return;

            if(Instance.Pathfinder != null)
            {
                Instance.Pathfinder.Tick();
            }

            _physicsTimer += deltaTime;
            if(_physicsTimer >= UnityEngine.Time.fixedDeltaTime)
            {
                _physicsTimer -= UnityEngine.Time.fixedDeltaTime;
                gameObject.scene.GetPhysicsScene().Simulate(UnityEngine.Time.fixedDeltaTime);
            }
        }
        
        public bool CanBeSelectedBy(StructureComponent other)
        {
            return other.Instance?.Sector != this;
        }

        public VisualElement CreateSelector()
        {
            return Instance?.CreateSelector() ?? new VisualElement();
        }

        public void UpdateSelector(bool selected)
        {
            Instance?.UpdateSelector(this, selected);
        }
    }
}
