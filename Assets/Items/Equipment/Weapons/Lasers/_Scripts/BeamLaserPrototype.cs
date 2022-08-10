using DarkFrontier.Items._Scripts;
using DarkFrontier.Items.Structures.New;
using UnityEngine;

namespace DarkFrontier.Items.Equipment.Weapons.Lasers
{
    [CreateAssetMenu(menuName = "Items/Prototypes/Equipment/Weapons/Lasers/Beam Laser")]
    public class BeamLaserPrototype : EquipmentPrototype
    {
        public Damage damage;
        public float multiplier;
        public float gain;
        public float interval;
        
        public override ItemInstance NewInstance() => new BeamLaserInstance(this);
    }
}
