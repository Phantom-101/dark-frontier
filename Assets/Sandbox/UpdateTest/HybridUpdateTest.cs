using DarkFrontier.Foundation.Behaviors;
using System.Collections.Generic;
using UnityEngine;

public class HybridUpdateTest : MonoBehaviour {
    public int InstantiateCount;

    private readonly List<IBehavior> behaviors = new List<IBehavior> ();

    private void Start () {
        for (int i = 0; i < InstantiateCount; i++) {
            GameObject gameObject = new GameObject ();
            BehaviorContainer component = gameObject.AddComponent<BehaviorContainer> ();
            component.Behavior = new PureBehaviorTestUpdater ();
            behaviors.Add (component);
        }
    }

    private void Update () {
        behaviors.ForEach (e => e.Tick (Time.deltaTime));
    }
}
