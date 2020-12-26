using UnityEngine;

public class WeaponSlot : EquipmentSlot {

    [SerializeField] protected Structure _target;

    public WeaponSO GetWeapon () { return _equipment as WeaponSO; }

    public Structure GetTarget () { return _target; }

    public void SetTarget (Structure target) { _target = target; }

    public override bool CanEquip (EquipmentSO equipment) {

        return base.CanEquip (equipment) || (equipment is WeaponSO && equipment.Meta <= _equipper.GetProfile ().MaxMeta);

    }

    protected override void Reset () {

        base.Reset ();

        _target = null;
        _active = false;

    }

}
