#if UNITY_EDITOR
using DarkFrontier.Items.Equipment;
using DarkFrontier.Items.Segments;
using UnityEditor;
using UnityEngine;

namespace DarkFrontier.Items.Structures
{
    [CustomEditor(typeof(StructureComponent))]
    public class StructureEditor : Editor
    {
        private StructureComponent _component;

        private void OnEnable()
        {
            _component = (StructureComponent)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            if(GUILayout.Button("Create Instance"))
            {
                if(_component.Instance == null)
                {
                    _component.SetInstance(new StructureInstance());
                }
            }

            if(_component.Instance != null)
            {
                if(GUILayout.Button("Fix Nulls"))
                {
                    for(int i = 0, li = _component.Instance.Segments.Length; i < li; i++)
                    {
                        _component.Instance.Segments[i] ??= new SegmentRecord();

                        for(int j = 0, lj = _component.Instance.Segments[i].Instance.Equipment.Length; j < lj; j++)
                        {
                            _component.Instance.Segments[i].Instance.Equipment[j] ??= new EquipmentRecord();
                        }
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif