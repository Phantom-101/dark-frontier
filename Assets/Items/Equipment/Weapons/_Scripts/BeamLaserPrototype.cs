using DarkFrontier.Items._Scripts;
using UnityEngine;

namespace DarkFrontier.Items.Equipment.Weapons.New
{
    [CreateAssetMenu(menuName = "Items/Prototypes/Equipment/Weapons/Beam Laser")]
    public class BeamLaserPrototype : EquipmentPrototype
    {
        public AnimationCurve damage;

        public float heatGain;

        public float heatLoss;
        
        public override ItemInstance NewInstance() => new BeamLaserInstance(this);
    }
}
