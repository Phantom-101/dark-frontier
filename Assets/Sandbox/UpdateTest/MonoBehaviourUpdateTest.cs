using UnityEngine;

public class MonoBehaviourUpdateTest : MonoBehaviour {
    public int InstantiateCount;

    private void Start () {
        for (int i = 0; i < InstantiateCount; i++) {
            GameObject gameObject = new GameObject ();
            gameObject.AddComponent<MonoBehaviourTestUpdater> ();
        }
    }
}
