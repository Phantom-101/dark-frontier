using DarkFrontier.Items._Scripts;
using UnityEngine;

namespace DarkFrontier.Items.Equipment.Electronics.HullResists
{
    [CreateAssetMenu(menuName = "Items/Prototypes/Equipment/Electronics/Hull Resists/Adaptive Plating")]
    public class AdaptivePlatingPrototype : EquipmentPrototype
    {
        public float adaptation;
        
        public override ItemInstance NewInstance() => new AdaptivePlatingInstance(this);
    }
}