using System;
using System.Linq;
using UnityEngine;

public class EquipmentSlot : MonoBehaviour {

    [SerializeField] protected EquipmentSO _equipment;
    [SerializeField] protected float _storedEnergy;
    [SerializeField] protected float _currentDurability;
    [SerializeField] protected Structure _equipper;
    [SerializeField] protected bool _active;
    [SerializeField] protected ItemSO _charge;
    [SerializeField] protected int _chargeQuantity;

    public EquipmentSO GetEquipment () { return _equipment; }

    public float GetStoredEnergy () { return _storedEnergy; }

    public float GetRequiredEnergy () {

        if (_equipment == null) return 0;

        return _equipment.EnergyStorage - _storedEnergy;

    }

    public void ChangeStoredEnergy (float amount) {

        if (_equipment == null) return;

        _storedEnergy = Mathf.Clamp (_storedEnergy + amount, 0, _equipment.EnergyStorage);

    }

    public void SetStoredEnergy (float amount) {

        if (_equipment == null) return;

        _storedEnergy = Mathf.Clamp (amount, 0, _equipment.EnergyStorage);

    }

    public float GetDurability () { return _currentDurability; }

    public void ChangeDurability (float amount) {

        if (_equipment == null) return;

        _currentDurability = Mathf.Clamp (_currentDurability + amount, 0, _equipment.Durability);

        if (_currentDurability <= 0) Equip (null);

    }

    public void SetDurability (float amount) {

        if (_equipment == null) return;

        _currentDurability = Mathf.Clamp (amount, 0, _equipment.Durability);

        if (_currentDurability <= 0) Equip (null);

    }

    public virtual void TakeDamage (float amount) {

        ChangeDurability (-amount);

        if (GetDurability () <= 0) Equip (null);

    }

    public Structure GetEquipper () { return _equipper; }

    public bool IsActive () { return _active; }

    public bool CanActivate () {

        if (_equipment == null) return false;

        return _equipment.CanActivate (this);

    }

    public void SetIsActive (bool target) { _active = target; }

    public void Activate () { _equipment.Activate (this); }

    public void Deactivate () { _equipment.Deactivate (this); }

    public bool CanLoadCharge (ItemSO charge) {

        if (_equipment == null) return false;
        if (!_equipment.Activatable) return false;
        if (!_equipment.Charges.Contains (charge)) return false;
        if (_charge == charge) {

            if (GetFreeInventorySize () < charge.Size) return false;

        } else {

            if (_equipment.InventorySize < charge.Size) return false;

        }

        return true;

    }

    public bool LoadCharge (ItemSO charge) {

        if (!CanLoadCharge (charge)) return false;

        if (_charge == charge) {

            _chargeQuantity++;

        } else {

            _charge = charge;
            _chargeQuantity = 1;

        }

        return true;

    }

    public float GetUsedInventorySize () {

        if (_charge == null) return 0;

        return _charge.Size * _chargeQuantity;

    }

    public float GetFreeInventorySize () {

        return GetTotalInventorySize () - GetUsedInventorySize ();

    }

    public float GetTotalInventorySize () {

        return _equipment.InventorySize;

    }

    public void Tick () {

        if (_equipment == null) return;

        _equipment.Tick (this);

    }

    public void FixedTick () {

        if (_equipment == null) return;

        _equipment.FixedTick (this);

    }

    public virtual bool CanEquip (EquipmentSO equipment) {

        if (equipment == null) return true;

        return false;

    }

    public virtual bool Equip (EquipmentSO equipment) {

        if (CanEquip (equipment)) {

            if (_equipment != null) {

                _equipment.OnUnequip (this);

            }

            ResetValues ();

            _equipment = equipment;

            if (_equipment != null) {

                _equipment.OnEquip (this);

            }

            return true;

        }

        return false;

    }

    protected virtual void ResetValues () {

        _storedEnergy = 0;
        _currentDurability = 0;

    }

    public virtual EquipmentSlotSaveData GetSaveData () {

        EquipmentSlotSaveData data = new EquipmentSlotSaveData {

            StoredEnergy = _storedEnergy,
            CurrentDurability = _currentDurability,
            Active = _active

        };
        if (_equipment != null) data.EquipmentId = _equipment.Id;
        if (_equipper != null) data.EquipperId = _equipper.GetId ();
        return data;

    }

    public virtual void LoadSaveData (EquipmentSlotSaveData saveData) {

        _equipment = ItemManager.GetInstance ().GetItem (saveData.EquipmentId) as EquipmentSO;
        _storedEnergy = saveData.StoredEnergy;
        _currentDurability = saveData.CurrentDurability;
        _active = saveData.Active;

    }

}

[Serializable]
public class EquipmentSlotSaveData {

    public string EquipmentId;
    public float StoredEnergy;
    public float CurrentDurability;
    public string EquipperId;
    public bool Active;

}