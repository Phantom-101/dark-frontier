using DarkFrontier.Items._Scripts;
using UnityEngine;

namespace DarkFrontier.Items.Equipment.Generators
{
    [CreateAssetMenu(menuName = "Items/Prototypes/Equipment/Generators/Generator")]
    public class GeneratorPrototype : EquipmentPrototype
    {
        public float generation;
        
        public override ItemInstance NewInstance() => new GeneratorInstance(this);
    }
}