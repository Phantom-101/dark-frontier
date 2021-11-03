using System.Collections.Generic;
using DarkFrontier.Foundation.Behaviors;
using UnityEngine;

namespace DarkFrontier.Sandbox.UpdateTest {
    public class PureBehaviorUpdateTest : MonoBehaviour {
        public int InstantiateCount;

        private readonly List<IBehavior> behaviors = new List<IBehavior> ();

        private void Start () {
            for (int i = 0; i < InstantiateCount; i++) {
                behaviors.Add (new PureBehaviorTestUpdater ());
            }
        }
    }
}
