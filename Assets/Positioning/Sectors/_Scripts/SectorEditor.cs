using UnityEditor;
using UnityEngine;

namespace DarkFrontier.Positioning.Sectors
{
#if UNITY_EDITOR
    [CustomEditor(typeof(SectorComponent))]
    public class SectorEditor : Editor
    {
        private SectorComponent _component;

        private void OnEnable()
        {
            _component = (SectorComponent)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            if(GUILayout.Button("Create Instance"))
            {
                _component.instance = new SectorInstance();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
