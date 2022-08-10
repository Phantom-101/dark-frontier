using DarkFrontier.Items._Scripts;
using UnityEngine;

namespace DarkFrontier.Items.Equipment.Electronics.Resistance
{
    [CreateAssetMenu(menuName = "Items/Prototypes/Equipment/Electronics/Resistance Boosters/Shield Amplifier")]
    public class ShieldSolidifierPrototype : EquipmentPrototype
    {
        public float amplification;
        
        public override ItemInstance NewInstance() => new ShieldSolidifierInstance(this);
    }
}