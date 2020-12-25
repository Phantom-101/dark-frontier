using UnityEngine;

public class WeaponSlot : EquipmentSlot {

    [SerializeField] protected WeaponSO _weapon;
    [SerializeField] protected Structure _target;
    [SerializeField] protected bool _active;

    public override bool CanEquip (EquipmentSO equipment) {

        return base.CanEquip (equipment) || (equipment is WeaponSO && equipment.Meta <= _equipper.GetProfile ().MaxMeta);

    }

    protected override void OnEquip (Structure structure, EquipmentSlot slot, EquipmentSO prevEquipment, EquipmentSO newEquipment) {

        if (slot != this) return;

        base.OnEquip (structure, slot, prevEquipment, newEquipment);

        _weapon = _equipment as WeaponSO;
        _target = null;
        _active = false;

    }

    protected override void OnUnequip (Structure structure, EquipmentSlot slot, EquipmentSO prevEquipment, EquipmentSO newEquipment) {

        if (slot != this) return;

        base.OnUnequip (structure, slot, prevEquipment, newEquipment);

        _weapon = null;
        _target = null;
        _active = false;

    }

    protected override void OnDestroyed (Structure structure, EquipmentSlot slot, EquipmentSO prevEquipment, EquipmentSO newEquipment) {

        if (slot != this) return;

        base.OnDestroyed (structure, slot, prevEquipment, newEquipment);

        _weapon = null;
        _target = null;
        _active = false;

    }

    public override bool CanActivate () {

        if (_equipper.GetTarget () == null) return false;
        if ((_equipper.transform.localPosition - _equipper.GetTarget ().transform.localPosition).sqrMagnitude > _weapon.Range * _weapon.Range) return false;

        return true;

    }

    public override bool CanSustain () {

        return CanActivate ();

    }

    public override bool Activate () {

        if (!CanActivate ()) return false;

        _target = _equipper.GetTarget ();
        _active = true;
        return true;

    }

    public override void Deactivate () {

        _active = false;

    }

    public override void Tick () {

        if (!CanSustain ()) Deactivate ();

        if (_active) {

            if (_storedEnergy == _equipment.EnergyStorage) {

                _storedEnergy = 0;
                _target.ChangeHull (-_weapon.Damage);

            } else {

                _storedEnergy += Time.deltaTime;

            }

        }

    }

}
