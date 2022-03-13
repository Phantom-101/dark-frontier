#if UNITY_EDITOR
using DarkFrontier.Items.Equipment;
using DarkFrontier.Items.Segments;
using UnityEditor;
using UnityEngine;

namespace DarkFrontier.Items.Structures
{
    [CustomEditor(typeof(StructureAuthoring))]
    public class StructureEditor : Editor
    {
        private StructureAuthoring _component;

        private void OnEnable()
        {
            _component = (StructureAuthoring)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            if(GUILayout.Button("Create Instance"))
            {
                _component.instance ??= new StructureInstance();
            }

            if(_component.instance != null)
            {
                if(GUILayout.Button("Fix Nulls"))
                {
                    for(int i = 0, li = _component.instance.SegmentRecords.Length; i < li; i++)
                    {
                        _component.instance.SegmentRecords[i] ??= new SegmentRecord();

                        for(int j = 0, lj = _component.instance.SegmentRecords[i].Instance.EquipmentRecords.Length; j < lj; j++)
                        {
                            _component.instance.SegmentRecords[i].Instance.EquipmentRecords[j] ??= new EquipmentRecord();
                        }
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif