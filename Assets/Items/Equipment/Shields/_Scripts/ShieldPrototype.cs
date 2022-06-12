using DarkFrontier.Items._Scripts;
using UnityEngine;

namespace DarkFrontier.Items.Equipment.Shields
{
    [CreateAssetMenu(menuName = "Items/Prototypes/Equipment/Shields/Shield")]
    public class ShieldPrototype : EquipmentPrototype
    {
        public float shielding;
        
        public float reinforcement;
        
        public override ItemInstance NewInstance() => new ShieldInstance(this);
    }
}