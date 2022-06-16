#nullable enable
using DarkFrontier.Controllers.New;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Game.Essentials;
using DarkFrontier.Items.Equipment;
using DarkFrontier.Items.Segments;
using DarkFrontier.Items.Structures.New;
using DarkFrontier.UI.Indicators.Selectors;
using DarkFrontier.Utils;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Items.Structures
{
    public class StructureComponent : MonoBehaviour, ISelectable
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

        [ReadOnly]
        public new Rigidbody rigidbody = null!;
        
        [ReadOnly]
        public new UnityEngine.Camera camera = null!;

        public bool SelectorDirty => false;
        
        public void Initialize()
        {
            if(_initialized) return;
            _idRegistry = Singletons.Get<IdRegistry>();
            _structureRegistry = Singletons.Get<StructureRegistry>();
            _detectableRegistry = Singletons.Get<DetectableRegistry>();
            _playerController = Singletons.Get<PlayerController>();
            rigidbody = gameObject.AddOrGet<Rigidbody>();
            camera = Singletons.Get<UnityEngine.Camera>();
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
            if((Instance.CurrentHp = Mathf.Clamp(Instance.CurrentHp, 0, Instance.MaxHp)) == 0)
            {
                // TODO destroy structure
            }
            for(int i = 0, l = Instance.Segments.Length; i < l; i++)
            {
                Instance.Segments[i].Tick(deltaTime);
            }
            if(_playerController.Player != this)
            {
                Instance.Controller.Tick(this);
            }
        }

        public void FixedTick(float deltaTime)
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
        }

        public void RecalculateMaxHp()
        {
            if(Instance == null) return;
            Instance.MaxHp = 0;
            for(int i = 0, l = Instance.Segments.Length; i < l; i++)
            {
                Instance.MaxHp += Instance.Segments[i].Instance?.Prototype.poolHp ?? 0;
            }
        }

        public void TakeDamage(Damage damage)
        {
            if(Instance == null) return;
            var scaledTotal = damage.field / (1 + Instance.ShieldFieldResist.Value)
                              + damage.explosive / (1 + Instance.ShieldExplosiveResist.Value)
                              + damage.particle / (1 + Instance.ShieldParticleResist.Value)
                              + damage.kinetic / (1 + Instance.ShieldKineticResist.Value);
            if(scaledTotal <= Instance.Shield)
            {
                Instance.Shield -= scaledTotal;
            }
            else
            {
                var absorbed = Instance.Shield / scaledTotal;
                Instance.Shield = 0;
                var scaledRemainder = damage.field / (1 + Instance.HullFieldResist.Value)
                                      + damage.explosive / (1 + Instance.HullExplosiveResist.Value)
                                      + damage.particle / (1 + Instance.HullParticleResist.Value)
                                      + damage.kinetic / (1 + Instance.HullKineticResist.Value);
                scaledRemainder *= 1 - absorbed;
                Instance.CurrentHp -= scaledRemainder;
            }
        }
        
        public bool CanBeSelectedBy(StructureComponent other)
        {
            return true;
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