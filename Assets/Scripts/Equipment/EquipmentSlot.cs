﻿using System;
using UnityEditor;
//using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : MonoBehaviour {

    [SerializeField] protected EquipmentSO _equipment;
    [SerializeField] protected float _energy;
    [SerializeField] protected float _durability;
    [SerializeField] protected Structure _equipper;
    [SerializeField] protected bool _currentState;
    [SerializeField] protected bool _targetState;
    [SerializeField] protected ChargeSO _charge;
    //[SerializeField] protected List<NewEquipmentSO> _allowedEquipment = new List<NewEquipmentSO> ();

    public EquipmentSO Equipment { get => _equipment; set { _equipment = value; if (_equipment != null) _equipment.OnEquip (this); } }
    public float Energy { get => _energy; set { _energy = Mathf.Clamp (value, 0, _equipment == null ? 0 : _equipment.EnergyStorage); } }
    public float Durability { get => _durability; set { _durability = Mathf.Clamp (value, 0, _equipment == null ? 0 : _equipment.Durability); } }
    public Structure Equipper { get => _equipper; set => _equipper = value; }
    public bool CurrentState { get => _currentState; set => _currentState = value; }
    public bool TargetState { get => _targetState; set => _targetState = value; }
    public ChargeSO Charge { get => _charge; set => _charge = value; }
    //public List<NewEquipmentSO> AllowedEquipment { get => _allowedEquipment; }

    public virtual void Tick () { if (Equipment != null) Equipment.Tick (this); }
    public virtual void FixedTick () { if (Equipment != null) Equipment.FixedTick (this); }
    public virtual void ResetValues () {

        Energy = 0;
        Durability = 0;
        CurrentState = false;
        TargetState = false;
        Charge = null;

    }
    public virtual bool CanEquip (EquipmentSO equipment) { return equipment == null; }
    public virtual NewEquipmentSlotSaveData GetSaveData () {

        return new NewEquipmentSlotSaveData {

            EquipmentId = Equipment.Id,
            Energy = Energy,
            Durability = Durability,
            EquipperId = Equipper.GetId (),
            CurrentState = CurrentState,
            TargetState = TargetState,
            ChargeId = Charge.Id

        };

    }
    public virtual void LoadSaveData (NewEquipmentSlotSaveData data) {

        Equipment = ItemManager.GetInstance ().GetItem (data.EquipmentId) as EquipmentSO;
        Energy = data.Energy;
        Durability = data.Durability;
        CurrentState = data.CurrentState;
        TargetState = data.TargetState;
        Charge = ItemManager.GetInstance ().GetItem (data.ChargeId) as ChargeSO;

    }

}

[CustomEditor (typeof (EquipmentSlot))]
[CanEditMultipleObjects]
public class EquipmentSlotEditor : Editor {

    SerializedProperty equipment;
    SerializedProperty energy;
    SerializedProperty durability;

    void OnEnable () {

        equipment = serializedObject.FindProperty ("_equipment");
        energy = serializedObject.FindProperty ("_energy");
        durability = serializedObject.FindProperty ("_durability");

    }

    public override void OnInspectorGUI () {

        serializedObject.Update ();

        EditorGUILayout.PropertyField (equipment);
        EditorGUILayout.Slider (energy, 0, equipment.objectReferenceValue == null ? 0 : (equipment.objectReferenceValue as EquipmentSO).EnergyStorage);
        EditorGUILayout.Slider (durability, 0, equipment.objectReferenceValue == null ? 0 : (equipment.objectReferenceValue as EquipmentSO).Durability);

        serializedObject.ApplyModifiedProperties ();

    }

}

[Serializable]
public class NewEquipmentSlotSaveData {

    public string EquipmentId;
    public float Energy;
    public float Durability;
    public string EquipperId;
    public bool CurrentState;
    public bool TargetState;
    public string ChargeId;

}