using DarkFrontier.Controllers.Intents;
using DarkFrontier.Items.Segments;
using DarkFrontier.Items.Structures;
using UnityEngine.UIElements;

namespace DarkFrontier.Items.Equipment.Weapons.New
{
    public class BeamLaserInstance : EquipmentInstance, IAttackIntent
    {
        public new BeamLaserPrototype Prototype => (BeamLaserPrototype)base.Prototype;
        
        public BeamLaserInstance()
        {
        }
        
        public BeamLaserInstance(BeamLaserPrototype prototype) : base(prototype)
        {
        }

        public void Attack(ISelectable target)
        {
            switch(target)
            {
                case StructureComponent:
                    // random equipment on random segment take damage
                    // said random segment take damage
                    // structure take damage
                    break;
                case SegmentComponent:
                    // random equipment take damage
                    // segment take damage
                    // structure take damage
                    break;
                case EquipmentComponent:
                    // equipment take damage
                    // segment take damage
                    // structure take damage
                    break;
            }
        }

        public void TryAttack(ISelectable target)
        {
            Attack(target);
        }
        
        public override VisualElement CreateIndicator()
        {
            return base.CreateIndicator();
        }

        public override void UpdateIndicator(EquipmentComponent component)
        {
            base.UpdateIndicator(component);
        }
    }
}