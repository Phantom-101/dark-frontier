using UnityEngine;

public class EquipmentSlot : MonoBehaviour {

    [SerializeField] protected EquipmentSO _equipment;
    [SerializeField] protected float _storedEnergy;
    [SerializeField] protected float _currentDurability;
    [SerializeField] protected Structure _equipper;
    [SerializeField] protected bool _active;

    public EquipmentSO GetEquipment () { return _equipment; }

    public float GetStoredEnergy () { return _storedEnergy; }

    public void ChangeStoredEnergy (float amount) { _storedEnergy = Mathf.Clamp (_storedEnergy + amount, 0, _equipment.EnergyStorage); }

    public void SetStoredEnergy (float amount) { _storedEnergy = Mathf.Clamp (amount, 0, _equipment.EnergyStorage); }

    public float GetDurability () { return _currentDurability; }

    public void ChangeDurability (float amount) { _currentDurability = Mathf.Clamp (_currentDurability + amount, 0, _equipment.Durability); }

    public void SetDurability (float amount) { _currentDurability = Mathf.Clamp (amount, 0, _equipment.Durability); }

    public Structure GetEquipper () { return _equipper; }

    public bool IsActive () { return _active; }

    public bool CanActivate () { return _equipment.CanActivate (this); }

    public void SetIsActive (bool target) { _active = target; }

    public void Activate () { _equipment.Activate (this); }

    public void Deactivate () { _equipment.Deactivate (this); }

    public void Tick () { _equipment.Tick (this); }

    public void FixedTick () { _equipment.FixedTick (this); }

    public virtual bool CanEquip (EquipmentSO equipment) {

        if (equipment == null) return true;

        return false;

    }

    public virtual bool Equip (EquipmentSO equipment) {

        if (CanEquip (equipment)) {

            if (_equipment != null) {

                _equipment.OnUnequip (this);

            }

            Reset ();

            _equipment = equipment;

            if (_equipment != null) {

                _equipment.OnEquip (this);

            }

            return true;

        }

        return false;

    }

    protected virtual void Reset () {

        _storedEnergy = 0;
        _currentDurability = 0;

    }

}
