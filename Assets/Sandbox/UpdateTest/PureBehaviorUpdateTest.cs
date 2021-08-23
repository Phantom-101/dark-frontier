using DarkFrontier.Foundation.Behaviors;
using System.Collections.Generic;
using UnityEngine;

public class PureBehaviorUpdateTest : MonoBehaviour {
    public int InstantiateCount;

    private readonly List<IBehavior> behaviors = new List<IBehavior> ();

    private void Start () {
        for (int i = 0; i < InstantiateCount; i++) {
            behaviors.Add (new PureBehaviorTestUpdater ());
        }
    }

    private void Update () {
        behaviors.ForEach (e => e.Tick (Time.deltaTime));
    }
}
