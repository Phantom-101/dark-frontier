using DarkFrontier.Items._Scripts;
using UnityEngine;

namespace DarkFrontier.Items.Equipment.Electronics.Resistance
{
    [CreateAssetMenu(menuName = "Items/Prototypes/Equipment/Electronics/Resistance Boosters/Adaptive Plating")]
    public class AdaptivePlatingPrototype : EquipmentPrototype
    {
        public float adaptation;
        
        public override ItemInstance NewInstance() => new AdaptivePlatingInstance(this);
    }
}