using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.Items.Prototypes {
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
