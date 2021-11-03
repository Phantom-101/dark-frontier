using System.Collections.Generic;
using DarkFrontier.Foundation.Behaviors;
using UnityEngine;

namespace DarkFrontier.Sandbox.UpdateTest {
    public class HybridUpdateTest : MonoBehaviour {
        public int InstantiateCount;

        private readonly List<IBehavior> behaviors = new List<IBehavior> ();

        private void Start () {
            for (int i = 0; i < InstantiateCount; i++) {
                GameObject gameObject = new GameObject ();
                BehaviorContainer component = gameObject.AddComponent<BehaviorContainer> ();
                component.uBehavior = new PureBehaviorTestUpdater ();
                behaviors.Add (component);
            }
        }
    }
}
