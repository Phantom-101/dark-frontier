#nullable enable
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
    public class BeamLaserInstance : EquipmentInstance, IAttackIntent
    {
        public new BeamLaserPrototype Prototype => (BeamLaserPrototype)base.Prototype;

        [field: SerializeReference]
        public ISelectable? Target { get; private set; }
        
        [field: SerializeField] [JsonProperty("target-id")]
        public string TargetId { get; private set; } = string.Empty;

        [field: SerializeField] [JsonProperty("multiplier")]
        public float Multiplier { get; private set; } = 1;
        
        [field: SerializeField] [JsonProperty("delay")]
        public float Delay { get; private set; }

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
                    if(Target is StructureComponent structure)
                    {
                        structure.TakeDamage(dmg);
                    }
                    else if(Target is SegmentComponent segment)
                    {
                        if(segment.Structure != null)
                        {
                            segment.Structure.TakeDamage(dmg);
                        }
                    }
                    else if(Target is EquipmentComponent equipment)
                    {
                        if(equipment.Structure != null)
                        {
                            equipment.Structure.TakeDamage(dmg);
                        }
                    }
                    Multiplier = Mathf.Clamp(Multiplier + Prototype.gain, 1, Prototype.multiplier);
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
            if(_playerController.Player != null && _playerController.Player.Instance?.Selected != null)
            {
                Attack(_playerController.Player.Instance.Selected);
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

        public override void ToSerialized()
        {
            base.ToSerialized();
            TargetId = Target == null ? string.Empty : Target.Id;
        }

        public override void FromSerialized()
        {
            base.FromSerialized();
            Target = string.IsNullOrEmpty(TargetId) ? Target : Singletons.Get<IdRegistry>().Get<ISelectable>(TargetId);
        }
    }
}