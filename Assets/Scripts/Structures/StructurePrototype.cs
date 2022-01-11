using DarkFrontier.Items;
using UnityEngine;

namespace DarkFrontier.Structures {
    [CreateAssetMenu (menuName = "Items/Structure")]
    public class StructurePrototype : ItemPrototype {
        [Header ("Graphics")]
        public GameObject Prefab;
        public float ApparentSize;
        public Sprite HullWireframe;
        public bool ShowBlip;
        public GameObject DestructionEffect;
        public bool SnapToPlane;

        [Header ("Stats")]
        public Structure.Stats Stats = new Structure.Stats();
    }
}
