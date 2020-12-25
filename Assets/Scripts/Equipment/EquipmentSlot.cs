using UnityEngine;

public class EquipmentSlot : MonoBehaviour {

    [SerializeField] protected EquipmentSO _equipment;
    [SerializeField] protected double _storedEnergy;
    [SerializeField] protected double _currentDurability;
    [SerializeField] protected Structure _equipper;

    public virtual bool CanEquip (EquipmentSO equipment) {

        if (equipment == null) return true;

        return false;

    }

    public virtual bool Equip (EquipmentSO equipment) {

        if (CanEquip (equipment)) {

            // Call OnUnequipChannel.RaiseEvent and unsubscribe to old channels

            if (_equipment != null) {

                if (_equipment.OnEquipChannel != null) _equipment.OnEquipChannel.OnChanged -= OnEquip;

                if (_equipment.OnUnequipChannel != null) {

                    _equipment.OnUnequipChannel.RaiseEvent (_equipper, this, _equipment, equipment);
                    _equipment.OnUnequipChannel.OnChanged -= OnUnequip;

                }

                if (_equipment.OnDestroyChannel != null) _equipment.OnDestroyChannel.OnChanged -= OnDestroyed;

            }

            // Call OnEquipChannel.RaiseEvent and subscribe to new channels

            if (_equipment != null) {

                if (_equipment.OnEquipChannel != null) {

                    _equipment.OnEquipChannel.OnChanged += OnEquip;
                    _equipment.OnEquipChannel.RaiseEvent (_equipper, this, _equipment, equipment);

                }

                if (_equipment.OnUnequipChannel != null) _equipment.OnUnequipChannel.OnChanged += OnUnequip;

                if (_equipment.OnDestroyChannel != null) _equipment.OnDestroyChannel.OnChanged += OnDestroyed;

            }

            // Change equipment
            _equipment = equipment;

            return true;

        }

        return false;

    }

    protected virtual void OnEquip (Structure structure, EquipmentSlot slot, EquipmentSO prevEquipment, EquipmentSO newEquipment) {

        if (slot != this) return;

        _storedEnergy = 0;
        _currentDurability = _equipment.Durability;

    }

    protected virtual void OnUnequip (Structure structure, EquipmentSlot slot, EquipmentSO prevEquipment, EquipmentSO newEquipment) {

        if (slot != this) return;

        _storedEnergy = 0;
        _currentDurability = 0;

    }

    protected virtual void OnDestroyed (Structure structure, EquipmentSlot slot, EquipmentSO prevEquipment, EquipmentSO newEquipment) {

        if (slot != this) return;

        _storedEnergy = 0;
        _currentDurability = 0;

    }

    public virtual void Damage (double amount) {

        _currentDurability -= amount;

        if (_currentDurability <= 0) {

            if (_equipment != null) {

                if (_equipment.OnEquipChannel != null) _equipment.OnEquipChannel.OnChanged -= OnEquip;

                if (_equipment.OnUnequipChannel != null) _equipment.OnUnequipChannel.OnChanged -= OnUnequip;

                if (_equipment.OnDestroyChannel != null) {

                    _equipment.OnDestroyChannel.RaiseEvent (_equipper, this, _equipment, null);
                    _equipment.OnDestroyChannel.OnChanged -= OnDestroyed;

                }

                _equipment = null;

            }

        }

    }

    public virtual bool CanActivate () { return false; }

    public virtual bool CanSustain () { return false; }

    public virtual bool Activate () { return false; }

    public virtual void Deactivate () { }

    public virtual void Tick () { }

}
