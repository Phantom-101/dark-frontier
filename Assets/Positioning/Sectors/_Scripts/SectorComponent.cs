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
    public class SectorComponent : MonoBehaviour, IId, IDetectable
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
        private UnityEngine.Camera _camera = null!;
        
        public void Initialize()
        {
            if(_initialized) return;
            _idRegistry = Singletons.Get<IdRegistry>();
            _sectorRegistry = Singletons.Get<SectorRegistry>();
            _detectableRegistry = Singletons.Get<DetectableRegistry>();
            _camera = Singletons.Get<UnityEngine.Camera>();
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
        
        public bool IsDetectedBy(StructureComponent structure)
        {
            return structure.Instance?.Sector != this;
        }

        public VisualElement CreateSelector()
        {
            var element = Addressables.LoadAssetAsync<VisualTreeAsset>(Instance!.SelectorAddressableKey).WaitForCompletion().CloneTree();
            element.Q("selected").Q<Label>("name").text = Instance?.Name ?? "";
            return element;
        }

        public void UpdateSelector(VisualElement selector, bool selected)
        {
            var position = Instance == null ? Vector3.zero : _camera.WorldToViewportPoint(Instance.Position);
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
