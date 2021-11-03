using System.Collections.Generic;
using UnityEngine;

namespace DarkFrontier.Sandbox.UnloadTest {
    public class ClassUnloadTest : MonoBehaviour {
        public int InstantiateCount;

        private List<TestClass> list = new List<TestClass> ();

        private void Update () {
            if (UnityEngine.Input.GetKeyUp (KeyCode.Q)) {
                for (int i = 0; i < InstantiateCount; i++) {
                    list.Add (new TestClass ());
                }
                list = new List<TestClass> ();
                System.GC.Collect ();
                Resources.UnloadUnusedAssets ();
            }
        }

        private class TestClass { }
    }
}
