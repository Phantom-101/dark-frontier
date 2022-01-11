using DarkFrontier.Items.Equipment;
using DarkFrontier.Items.Segments;
using UnityEditor;
using UnityEngine;

namespace DarkFrontier.Items.Structures
{
#if UNITY_EDITOR
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
                _component.instance = new StructureInstance();
            }

            if(_component.instance != null)
            {
                if(GUILayout.Button("Fix Nulls"))
                {
                    for(int i = 0, li = _component.instance.Segments.Length; i < li; i++)
                    {
                        if (_component.instance.Segments[i] == null)
                        {
                            _component.instance.Segments[i] = new SegmentRecord();
                        }

                        for(int j = 0, lj = _component.instance.Segments[i].Instance.Equipment.Length; j < lj; j++)
                        {
                            if (_component.instance.Segments[i].Instance.Equipment[j] == null)
                            {
                                _component.instance.Segments[i].Instance.Equipment[j] = new EquipmentRecord();
                            }
                        }
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}