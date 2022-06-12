#nullable enable
using DarkFrontier.Controllers.New;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Game.Essentials;
using DarkFrontier.Items.Equipment;
using DarkFrontier.Items.Segments;
using DarkFrontier.UI.Indicators.Selectors;
using DarkFrontier.Utils;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Items.Structures
{
    public class StructureComponent : MonoBehaviour, IId, IDetectable
    {
        [field: SerializeReference]
        public StructureInstance? Instance { get; private set; }

        public string Id => Instance?.Id ?? string.Empty;
        
        [SerializeField, Attributes.ReadOnly]
        private bool _initialized;
        
        [SerializeField, Attributes.ReadOnly]
        private bool _registered;
        
        [SerializeField, Attributes.ReadOnly]
        private bool _enabled;
        
        private IdRegistry _idRegistry = null!;
        private StructureRegistry _structureRegistry = null!;
        private DetectableRegistry _detectableRegistry = null!;
        private PlayerController _playerController = null!;
        private UnityEngine.Camera _camera = null!;
        
        [ReadOnly]
        public new Rigidbody rigidbody = null!;
        
        public void Initialize()
        {
            if(_initialized) return;
            _idRegistry = Singletons.Get<IdRegistry>();
            _structureRegistry = Singletons.Get<StructureRegistry>();
            _detectableRegistry = Singletons.Get<DetectableRegistry>();
            _playerController = Singletons.Get<PlayerController>();
            rigidbody = gameObject.AddOrGet<Rigidbody>();
            _camera = Singletons.Get<UnityEngine.Camera>();
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
            _structureRegistry.Register(this);
            _detectableRegistry.Register(this);
            _enabled = true;
        }

        private void Disable()
        {
            if(!_initialized || !_enabled || Instance == null) return;
            transform.DestroyChildren();
            Instance.ClearSegments();
            _detectableRegistry.Unregister(this);
            _structureRegistry.Unregister(this);
            Instance.ToSerialized(this);
            _enabled = false;
        }
        
        public void Equip(string segment, SegmentInstance? instance)
        {
            if(!_initialized || !_enabled || Instance == null) return;
            for(int i = 0, l = Instance.Segments.Length; i < l; i++)
            {
                var segmentComponent = Instance.Segments[i];
                if(segmentComponent.Name == segment)
                {
                    segmentComponent.Equip(instance);
                    segmentComponent.Enable();
                    break;
                }
            }
        }

        public void Equip(string segment, string equipment, EquipmentInstance? instance)
        {
            if(!_initialized || !_enabled || Instance == null) return;
            for(int i = 0, li = Instance.Segments.Length; i < li; i++)
            {
                var segmentComponent = Instance.Segments[i];
                if(segmentComponent.Name == segment)
                {
                    segmentComponent.Equip(equipment, instance);
                    break;
                }
            }
        }

        public void Tick(float deltaTime)
        {
            if(Instance == null) return;

            var normLinear = Instance.LinearTarget.sqrMagnitude > 1 ? Instance.LinearTarget.normalized : Instance.LinearTarget;
            var curLinear = rigidbody.velocity;
            var targetLinear = transform.TransformVector(normLinear * Instance.LinearSpeed.Value);
            var offsetLinear = targetLinear - curLinear;
            var deltaLinear = offsetLinear.normalized * Instance.LinearAcceleration.Value * deltaTime;
            rigidbody.AddForce(offsetLinear.sqrMagnitude < deltaLinear.sqrMagnitude ? offsetLinear : deltaLinear, ForceMode.VelocityChange);

            var normAngular = Instance.AngularTarget.sqrMagnitude > 1 ? Instance.AngularTarget.normalized : Instance.AngularTarget;
            var curAngular = rigidbody.angularVelocity;
            var targetAngular = transform.TransformDirection(normAngular * Instance.AngularSpeed.Value);
            var offsetAngular = targetAngular - curAngular;
            var deltaAngular = offsetAngular.normalized * Instance.AngularAcceleration.Value * deltaTime;
            rigidbody.AddTorque(offsetAngular.sqrMagnitude < deltaAngular.sqrMagnitude ? offsetAngular : deltaAngular, ForceMode.VelocityChange);
            
            if(_playerController.Player != this)
            {
                Instance.Controller.Tick(this);
            }

            for(int i = 0, l = Instance.Segments.Length; i < l; i++)
            {
                Instance.Segments[i].Tick(deltaTime);
            }
        }
        
        public bool IsDetectedBy(StructureComponent structure)
        {
            return true;
        }

        public VisualElement CreateSelector()
        {
            var element = Instance!.Prototype.selectorElement!.CloneTree();
            element.Q("selected").Q<Label>("name").text = Instance?.Prototype.name ?? "";
            return element;
        }

        public void UpdateSelector(VisualElement selector, bool selected)
        {
            if(_playerController.Player != null && IsDetectedBy(_playerController.Player))
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
            else
            {
                selector.style.visibility = Visibility.Hidden;
            }
        }
    }
}