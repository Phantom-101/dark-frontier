using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectArrayLayout))]
public class ObjectArrayLayoutEditor : Editor
{
    private ObjectArrayLayout script;

    private void OnEnable()
    {
        script = (ObjectArrayLayout)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Layout"))
        {
            script.Layout();
        }
    }
}
