using DarkFrontier.Items._Scripts;
using UnityEngine;

namespace DarkFrontier.Items.Equipment.Electronics.ShieldResists
{
    [CreateAssetMenu(menuName = "Items/Prototypes/Equipment/Electronics/Shield Resists/Shield Amplifier")]
    public class ShieldAmplifierPrototype : EquipmentPrototype
    {
        public float amplification;
        
        public override ItemInstance NewInstance() => new ShieldAmplifierInstance(this);
    }
}