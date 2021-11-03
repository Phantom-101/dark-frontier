using UnityEngine;

namespace DarkFrontier.Sandbox.EventFunctionsTest {
    public class EventFunctions : MonoBehaviour {
        private void Awake () {
            Debug.Log ("Awake");
        }

        private void Start () {
            Debug.Log ("Start");
        }

        private void OnEnable () {
            Debug.Log ("On Enable");
        }
    }
}
