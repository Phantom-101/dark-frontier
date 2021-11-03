using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.Items.Prototypes {
    [CreateAssetMenu (menuName = "Items/Charges/Missile")]
    public class MissileSO : ItemPrototype {
        public StructurePrototype MissileStructure;
        public float HeadingAllowance;
        public float DetonationRange;
        public Damage Damage;
        public float Range;
    }
}
