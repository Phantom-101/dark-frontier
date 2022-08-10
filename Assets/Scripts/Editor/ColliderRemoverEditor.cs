using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ColliderRemover))]
public class ColliderRemoverEditor : Editor
{
    private ColliderRemover script;

    private void OnEnable()
    {
        script = (ColliderRemover)target;
    }
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Remove"))
        {
            script.Execute();
        }
        if (GUILayout.Button("Stop"))
        {
            script.Stop();
        }
    }
}