using DarkFrontier.Items._Scripts;
using UnityEngine;

namespace DarkFrontier.Items.Equipment.Engines
{
    [CreateAssetMenu(menuName = "Items/Prototypes/Equipment/Engines/Engine")]
    public class EnginePrototype : EquipmentPrototype
    {
        public float linearSpeed;
        
        public float angularSpeed;
        
        public float linearAcceleration;
        
        public float angularAcceleration;
        
        public override ItemInstance NewInstance() => new EngineInstance(this);
    }
}