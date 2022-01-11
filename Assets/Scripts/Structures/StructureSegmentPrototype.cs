using DarkFrontier.Items;
using UnityEngine;

namespace DarkFrontier.Structures {
    [CreateAssetMenu (menuName = "Items/Structure Segment")]
    public class StructureSegmentPrototype : ItemPrototype {
        [Header ("Graphics")]
        public GameObject Prefab;

        [Header ("Stats")]
        public Structure.Stats Stats = new Structure.Stats();

        public override ItemPrototype.State NewState() => new State(this);

        public new class State : ItemPrototype.State {
            
            
            public State(ItemPrototype aPrototype) : base(aPrototype) { }
            
        }
    }
}
