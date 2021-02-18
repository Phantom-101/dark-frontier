using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (Structure))]
[CanEditMultipleObjects]
public class StructureEditor : Editor {

    SerializedProperty _equipmentSlots;

    private void OnEnable () {

        _equipmentSlots = serializedObject.FindProperty ("_equipmentSlots");

    }

    public override void OnInspectorGUI () {

        base.OnInspectorGUI ();

        serializedObject.Update ();

        if (GUILayout.Button ("Find Equipment Slots")) {

            EquipmentSlot[] slots = (target as Structure).gameObject.GetComponentsInChildren<EquipmentSlot> ();
            _equipmentSlots.ClearArray ();
            foreach (EquipmentSlot slot in slots) {

                slot.Equipper = target as Structure;
                _equipmentSlots.InsertArrayElementAtIndex (0);
                _equipmentSlots.GetArrayElementAtIndex (0).objectReferenceValue = slot;

            }

        }

        serializedObject.ApplyModifiedProperties ();

    }

}