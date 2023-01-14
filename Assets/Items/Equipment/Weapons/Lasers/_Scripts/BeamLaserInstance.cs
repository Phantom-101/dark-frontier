#nullable enable
using System.Collections.Generic;
using DarkFrontier.Controllers.Intents;
using DarkFrontier.Controllers.New;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Game.Essentials;
using DarkFrontier.Items.Segments;
using DarkFrontier.Items.Structures;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Items.Equipment.Weapons.Lasers
{
    public class BeamLaserInstance : EquipmentInstance, IAttackIntent, ILaserProviders
    {
        public new BeamLaserPrototype Prototype => (BeamLaserPrototype)base.Prototype;

        [field: SerializeReference]
        public ISelectable? Target { get; private set; }
        
        [field: SerializeField] [JsonProperty("target-id")]
        public string? TargetId { get; private set; }

        [field: SerializeField] [JsonProperty("multiplier")]
        public float Multiplier { get; private set; } = 1;
        
        [field: SerializeField] [JsonProperty("delay")]
        public float Delay { get; private set; }

        public ILaserEndpointProvider Endpoint1Provider => new RelativeLaserEndpointProvider(_endpoint1, Vector3.zero);
        
        public ILaserEndpointProvider Endpoint2Provider => new RelativeLaserEndpointProvider(_endpoint2, Vector3.zero);
        
        public ILaserAlphaProvider AlphaProvider => new BeamLaserAlphaProvider(this);

        public ILaserWidthProvider WidthProvider => new ConstLaserWidthProvider(this);

        private Transform _endpoint1 = null!;
        private Transform _endpoint2 = null!;
        
        private VisualElement _indicator = null!;
        private VisualElement _center = null!;
        private VisualElement _side = null!;
        private VisualElement _bottom = null!;
        
        private PlayerController _playerController = null!;
        
        public BeamLaserInstance()
        {
        }
        
        public BeamLaserInstance(BeamLaserPrototype prototype) : base(prototype)
        {
        }

        public override void OnEquipped(EquipmentComponent component)
        {
            _playerController = Singletons.Get<PlayerController>();
        }

        public override void OnTick(EquipmentComponent component, float deltaTime)
        {
            Delay -= deltaTime;
            if(Target == null)
            {
                Multiplier = 1;
            }
            else
            {
                if(Delay <= 0)
                {
                    Delay = Prototype.interval;
                    var dmg = Prototype.damage * Multiplier;
                    _endpoint1 = component.transform;
                    if(Target is StructureComponent structure)
                    {
                        _endpoint2 = structure.transform;
                        structure.TakeDamage(component.Structure, dmg);
                    }
                    else if(Target is SegmentComponent segment)
                    {
                        _endpoint2 = segment.transform;
                        if(segment.Structure != null)
                        {
                            segment.Structure.TakeDamage(component.Structure, dmg);
                        }
                    }
                    else if(Target is EquipmentComponent equipment)
                    {
                        _endpoint2 = equipment.transform;
                        if(equipment.Structure != null)
                        {
                            equipment.Structure.TakeDamage(component.Structure, dmg);
                        }
                    }
                    Multiplier = Mathf.Clamp(Multiplier + Prototype.gain, 1, Prototype.multiplier);
                    var obj = Object.Instantiate(Prototype.visuals);
                    var visuals = obj.GetComponent<LaserVisuals>();
                    visuals.UseProviders(this);
                }
            }
        }

        public void Attack(ISelectable target)
        {
            if(target is StructureComponent or SegmentComponent or EquipmentComponent)
            {
                Target = target;
            }
        }
        
        private void OnClick(ClickEvent evt)
        {
            if(_playerController.Player != null && _playerController.Player.Adaptor!.selected != null)
            {
                Attack(_playerController.Player.Adaptor.selected);
            }
        }

        public override VisualElement CreateIndicator()
        {
            _indicator = base.CreateIndicator();
            _indicator.Q("rect").RegisterCallback<ClickEvent>(OnClick);
            _center = _indicator.Q("center-fill");
            _side = _indicator.Q("side-fill");
            _bottom = _indicator.Q("bottom-fill");
            return _indicator;
        }

        public override void UpdateIndicator(EquipmentComponent component)
        {
            _center.style.height = new Length((1 - Delay / Prototype.interval) * 100, LengthUnit.Percent);
            _side.style.height = new Length(Multiplier / Prototype.multiplier * 100, LengthUnit.Percent);
            _bottom.style.width = new Length(Hp / Prototype.hp * 100, LengthUnit.Percent);
        }

        public override void OnSerialize()
        {
            base.OnSerialize();
            TargetId = Target == null ? null : Target.Id;
        }

        public override void OnDeserialize()
        {
            base.OnDeserialize();
            if (TargetId != null)
            {
                Target = Singletons.Get<IdRegistry>().Get<ISelectable>(TargetId);
            }
        }
    }
}