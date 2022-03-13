#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DarkFrontier.Positioning.Sectors
{
    [CustomEditor(typeof(SectorAuthoring))]
    public class SectorEditor : Editor
    {
        private SectorAuthoring _component;

        private void OnEnable()
        {
            _component = (SectorAuthoring)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            if(GUILayout.Button("Create Instance"))
            {
                _component.instance ??= new SectorInstance();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif