using UnityEditor;
using UnityEngine;

namespace DarkFrontier.UI.Indicators.Selectors
{
#if UNITY_EDITOR
    [CustomEditor(typeof(SelectorSpawner))]
    public class SelectorSpawnerEditor : Editor
    {
        private SelectorSpawner _component;

        private void OnEnable()
        {
            _component = (SelectorSpawner)target;
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            if (GUILayout.Button("Update"))
            {
                _component.Spawn();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}