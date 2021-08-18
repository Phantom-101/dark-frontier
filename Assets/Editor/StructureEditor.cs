using DarkFrontier.Structures;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (Structure))]
[CanEditMultipleObjects]
public class StructureEditor : Editor {
    SerializedProperty equipmentSlots;

    private void OnEnable () {
        equipmentSlots = serializedObject.FindProperty ("equipmentSlots");
    }

    public override void OnInspectorGUI () {
        base.OnInspectorGUI ();

        serializedObject.Update ();

        if (GUILayout.Button ("Find Equipment Slots")) {
            EquipmentSlot[] slots = (target as Structure).gameObject.GetComponentsInChildren<EquipmentSlot> ();
            equipmentSlots.ClearArray ();
            foreach (EquipmentSlot slot in slots) {
                slot.Equipper = target as Structure;
                equipmentSlots.InsertArrayElementAtIndex (equipmentSlots.arraySize);
                equipmentSlots.GetArrayElementAtIndex (equipmentSlots.arraySize - 1).objectReferenceValue = slot;
            }
        }

        serializedObject.ApplyModifiedProperties ();
    }
}