#nullable enable
using System.Collections.Generic;
using DarkFrontier.Controllers.New;
using DarkFrontier.Foundation.Extensions;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Game.Essentials;
using DarkFrontier.Items.Equipment;
using DarkFrontier.Items.Segments;
using DarkFrontier.Items.Structures.New;
using DarkFrontier.Positioning.Sectors;
using DarkFrontier.UI.Indicators.Interactions;
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

        public StructureAdaptor? Adaptor
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
        private StructureAdaptor? _adaptor;

        public string Id => Adaptor!.id;
        
        [ReadOnly]
        public new Rigidbody rigidbody = null!;
        
        [ReadOnly]
        public new UnityEngine.Camera camera = null!;
        
        public SegmentComponent[] Segments { get; private set; } = null!;

        public EquipmentComponent[] Equipment
        {
            get
            {
                var ret = new List<EquipmentComponent>();
                for (int i = 0, l = Segments.Length; i < l; i++)
                {
                    ret.AddRange(Segments[i].Equipment);
                }
                return ret.ToArray();
            }
        }

        private IdRegistry _idRegistry = null!;
        private StructureRegistry _structureRegistry = null!;
        private DetectableRegistry _detectableRegistry = null!;
        private PlayerController _playerController = null!;

        public bool SelectorDirty => false;

        public void Set(StructureInstance? instance)
        {
            _idRegistry = Singletons.Get<IdRegistry>();
            _structureRegistry = Singletons.Get<StructureRegistry>();
            _detectableRegistry = Singletons.Get<DetectableRegistry>();
            
            if (Instance != null)
            {
                transform.DestroyChildren();
            
                _idRegistry.Unregister(this);
                _structureRegistry.Unregister(this);
                _detectableRegistry.Unregister(this);

                Adaptor = null;
            }

            Instance = instance;
            
            if (Instance != null)
            {
                Adaptor = Instance.NewAdaptor();
                
                _idRegistry.Register(Id, this);
                _structureRegistry.Register(this);
                _detectableRegistry.Register(this);
                
                if(Instance.Prototype.prefab != null)
                {
                    Instantiate(Instance.Prototype.prefab, transform);
                }
                
                Segments = GetComponentsInChildren<SegmentComponent>();
                Adaptor.sector = GetComponentInParent<SectorComponent>();
                Adaptor.controller = new Controller();
                rigidbody = gameObject.AddOrGet<Rigidbody>();
                camera = Singletons.Get<UnityEngine.Camera>();
                _playerController = Singletons.Get<PlayerController>();
                
                for(int i = 0, li = Segments.Length; i < li; i++)
                {
                    Segments[i].Set(Instance.Segments.TryGet(Segments[i].Name, null));
                }
            }
        }
        
        public void Set(StructureSerializable serializable)
        {
            Set(serializable.instance);
            
            var t = transform;
            t.localPosition = serializable.position;
            t.localEulerAngles = serializable.rotation;
        }

        public void Tick(float deltaTime)
        {
            if(Instance == null) return;
            
            if((Instance.CurrentHp = Mathf.Clamp(Instance.CurrentHp, 0, Instance.MaxHp.Value)) == 0)
            {
                // TODO destroy structure
            }
            for(int i = 0, l = Segments.Length; i < l; i++)
            {
                Segments[i].Tick(deltaTime);
            }
            if(_playerController.Player != this)
            {
                Adaptor!.controller.Tick(this);
            }
        }

        public void FixedTick(float deltaTime)
        {
            if(Instance == null) return;

            var offsetLinear = transform.TransformVector(Adaptor!.linearTarget * Instance.LinearSpeed.Value) - rigidbody.velocity;
            var deltaLinear = Instance.LinearAcceleration.Value * deltaTime * offsetLinear.normalized;
            rigidbody.AddForce(offsetLinear.sqrMagnitude < deltaLinear.sqrMagnitude ? offsetLinear : deltaLinear, ForceMode.VelocityChange);

            var offsetAngular = transform.TransformDirection(Adaptor.angularTarget * Instance.AngularSpeed.Value) - rigidbody.angularVelocity;
            var deltaAngular = Instance.AngularAcceleration.Value * deltaTime * offsetAngular.normalized;
            rigidbody.AddTorque(offsetAngular.sqrMagnitude < deltaAngular.sqrMagnitude ? offsetAngular : deltaAngular, ForceMode.VelocityChange);
        }

        public void TakeDamage(StructureComponent? source, Damage damage)
        {
            if(Instance == null) return;
            
            var taken = TakeDamage(damage);
            if (source == _playerController.Player)
            {
                Singletons.Get<InteractionList>().AddInteraction(new AttackInteraction(this, true, taken));
            }
            else if (this == _playerController.Player)
            {
                Singletons.Get<InteractionList>().AddInteraction(new AttackInteraction(source, false, taken));
            }
        }
        
        private float TakeDamage(Damage damage)
        {
            if(Instance == null) return 0;
            
            var scaledTotal = damage.field / (1 + Instance.ShieldFieldResist.Value)
                              + damage.explosive / (1 + Instance.ShieldExplosiveResist.Value)
                              + damage.particle / (1 + Instance.ShieldParticleResist.Value)
                              + damage.kinetic / (1 + Instance.ShieldKineticResist.Value);
            if(scaledTotal <= Instance.Shield)
            {
                Instance.Shield -= scaledTotal;
                return scaledTotal;
            }
            var absorbed = Instance.Shield / scaledTotal;
            var depleted = Instance.Shield;
            Instance.Shield = 0;
            var scaledRemainder = damage.field / (1 + Instance.HullFieldResist.Value)
                                  + damage.explosive / (1 + Instance.HullExplosiveResist.Value)
                                  + damage.particle / (1 + Instance.HullParticleResist.Value)
                                  + damage.kinetic / (1 + Instance.HullKineticResist.Value);
            scaledRemainder *= 1 - absorbed;
            Instance.CurrentHp -= scaledRemainder;
            return depleted + scaledRemainder;
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