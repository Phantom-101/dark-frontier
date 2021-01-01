using UnityEngine;

public class WeaponSlot : EquipmentSlot {

    [SerializeField] protected Structure _target;
    [SerializeField] protected GameObject _projectile;

    public WeaponSO GetWeapon () { return _equipment as WeaponSO; }

    public Structure GetTarget () { return _target; }

    public void SetTarget (Structure target) { _target = target; }

    public GameObject GetProjectile () { return _projectile; }

    public void SetProjectile (GameObject projectile) { _projectile = projectile; }

    public override bool CanEquip (EquipmentSO equipment) {

        return base.CanEquip (equipment) || (equipment is WeaponSO && equipment.Meta <= _equipper.GetProfile ().MaxMeta);

    }

    protected override void ResetValues () {

        base.ResetValues ();

        _target = null;
        _active = false;

    }

}