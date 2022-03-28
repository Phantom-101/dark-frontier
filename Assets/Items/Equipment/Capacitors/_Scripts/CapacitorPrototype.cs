using DarkFrontier.Items._Scripts;
using UnityEngine;

namespace DarkFrontier.Items.Equipment.Capacitors
{
    [CreateAssetMenu(menuName = "Items/Prototypes/Equipment/Capacitors/Capacitor")]
    public class CapacitorPrototype : EquipmentPrototype
    {
        public float capacitance;
        
        public override ItemInstance NewInstance() => new CapacitorInstance(this);
    }
}
