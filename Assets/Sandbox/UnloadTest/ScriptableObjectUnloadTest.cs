using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectUnloadTest : MonoBehaviour {
    public int InstantiateCount;

    private List<TestScriptableObject> list = new List<TestScriptableObject> ();

    private void Update () {
        if (Input.GetKeyUp (KeyCode.W)) {
            for (int i = 0; i < InstantiateCount; i++) {
                list.Add (ScriptableObject.CreateInstance<TestScriptableObject> ());
            }
            list = new List<TestScriptableObject> ();
            System.GC.Collect ();
            Resources.UnloadUnusedAssets ();
        }
    }

    private class TestScriptableObject : ScriptableObject { }
}
