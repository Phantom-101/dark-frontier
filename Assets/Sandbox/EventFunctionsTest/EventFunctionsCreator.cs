using UnityEngine;

namespace DarkFrontier.Sandbox.EventFunctionsTest {
    public class EventFunctionsCreator : MonoBehaviour {
        private void Start () {
            GameObject gameObject = new GameObject ();
            EventFunctions component = gameObject.AddComponent<EventFunctions> ();
            Debug.Log ("Immediately After Instantiation");
        }

        private void Update () {
            Debug.Log ("Update");
        }
    }
}
