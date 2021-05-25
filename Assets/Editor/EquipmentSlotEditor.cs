using UnityEditor;
using UnityEngine;

/*
[CustomEditor (typeof (EquipmentSlot))]
[CanEditMultipleObjects]
public class EquipmentSlotEditor : Editor {

    SerializedProperty _equipment;
    SerializedProperty _energy;
    SerializedProperty _durability;
    SerializedProperty _equipper;
    SerializedProperty _currentState;
    SerializedProperty _targetState;
    SerializedProperty _charge;

    float _labelWidth;

    private void OnEnable () {

        _equipment = serializedObject.FindProperty ("_equipment");
        _energy = serializedObject.FindProperty ("_energy");
        _durability = serializedObject.FindProperty ("_durability");
        _equipper = serializedObject.FindProperty ("_equipper");
        _currentState = serializedObject.FindProperty ("_currentState");
        _targetState = serializedObject.FindProperty ("_targetState");
        _charge = serializedObject.FindProperty ("_charge");

        _labelWidth = EditorGUIUtility.labelWidth;

    }

    public override void OnInspectorGUI () {

        serializedObject.Update ();

        EditorGUILayout.PropertyField (_equipment);
        EditorGUILayout.PropertyField (_equipper);

        //EditorGUILayout.BeginHorizontal ();
        //EditorGUIUtility.labelWidth = 55;
        EditorGUILayout.Slider (_energy, 0, _equipment.objectReferenceValue == null ? 0 : (_equipment.objectReferenceValue as NewEquipmentSO).EnergyStorage);
        EditorGUILayout.Slider (_durability, 0, _equipment.objectReferenceValue == null ? 0 : (_equipment.objectReferenceValue as NewEquipmentSO).Durability);
        //EditorGUIUtility.labelWidth = _labelWidth;
        //EditorGUILayout.EndHorizontal ();

        EditorGUILayout.BeginHorizontal ();
        EditorGUIUtility.labelWidth = 10;
        _currentState.boolValue = EditorGUILayout.ToggleLeft ("Current", _currentState.boolValue);
        _targetState.boolValue = EditorGUILayout.ToggleLeft ("Target", _targetState.boolValue);
        GUILayout.FlexibleSpace ();
        EditorGUIUtility.labelWidth = _labelWidth;
        EditorGUILayout.EndHorizontal ();

        EditorGUILayout.PropertyField (_charge);

        serializedObject.ApplyModifiedProperties ();

    }

}
*/