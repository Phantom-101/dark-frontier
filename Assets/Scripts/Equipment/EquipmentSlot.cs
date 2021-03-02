using System;
//using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : MonoBehaviour {

    [SerializeField] protected EquipmentSize _slotSize;
    [SerializeField] protected EquipmentSO _equipment;
    [SerializeField] protected float _energy;
    [SerializeField] protected float _durability;
    [SerializeField] protected Structure _equipper;
    [SerializeField] protected bool _currentState;
    [SerializeField] protected bool _targetState;
    [SerializeField] protected ChargeSO _charge;
    //[SerializeField] protected List<NewEquipmentSO> _allowedEquipment = new List<NewEquipmentSO> ();

    public EquipmentSize SlotSize { get => _slotSize; }
    public EquipmentSO Equipment { get => _equipment; set { _equipment = value; if (_equipment != null) _equipment.OnEquip (this); } }
    public float Energy { get => _energy; set { _energy = Mathf.Clamp (value, 0, _equipment == null ? 0 : _equipment.EnergyStorage); } }
    public float Durability { get => _durability; set { _durability = Mathf.Clamp (value, 0, _equipment == null ? 0 : _equipment.Durability); } }
    public Structure Equipper { get => _equipper; set => _equipper = value; }
    public bool CurrentState { get => _currentState; set => _currentState = value; }
    public bool TargetState { get => _targetState; set => _targetState = value; }
    public ChargeSO Charge { get => _charge; set => _charge = value; }
    //public List<NewEquipmentSO> AllowedEquipment { get => _allowedEquipment; }
    public Vector3 Offset { get => Equipper == null ? Vector3.zero : Quaternion.Inverse (Equipper.transform.rotation) * (transform.position - Equipper.transform.position); }

    public virtual void Tick () { if (Equipment != null) Equipment.Tick (this); }
    public virtual void FixedTick () { if (Equipment != null) Equipment.FixedTick (this); }
    public virtual void ResetValues () {

        Energy = 0;
        Durability = 0;
        CurrentState = false;
        TargetState = false;
        Charge = null;

    }
    public virtual bool CanEquip (EquipmentSO equipment) { return equipment == null || equipment.Size == SlotSize; }
    public virtual EquipmentSlotSaveData GetSaveData () {

        EquipmentSlotSaveData data = new EquipmentSlotSaveData {

            Energy = Energy,
            Durability = Durability,
            CurrentState = CurrentState,
            TargetState = TargetState

        };
        data.EquipmentId = Equipment == null ? "" : Equipment.Id;
        data.EquipperId = Equipper == null ? "" : Equipper.Id;
        data.ChargeId = Charge == null ? "" : Charge.Id;
        return data;

    }
    public virtual void LoadSaveData (EquipmentSlotSaveData data) {

        Equipment = ItemManager.GetInstance ().GetItem (data.EquipmentId) as EquipmentSO;
        Energy = data.Energy;
        Durability = data.Durability;
        CurrentState = data.CurrentState;
        TargetState = data.TargetState;
        Charge = ItemManager.GetInstance ().GetItem (data.ChargeId) as ChargeSO;

    }

}

[Serializable]
public class EquipmentSlotSaveData {

    public string EquipmentId;
    public float Energy;
    public float Durability;
    public string EquipperId;
    public bool CurrentState;
    public bool TargetState;
    public string ChargeId;

}