using UnityEngine;

public class WeaponSlot : EquipmentSlot {

    [SerializeField] protected float _arc;
    [SerializeField] protected Structure _target;
    [SerializeField] protected GameObject _projectile;

    public WeaponSO Weapon { get { return _equipment as WeaponSO; } }
    public float Arc { get => _arc; }
    public Structure Target { get => _target; set => _target = value; }
    public GameObject Projectile { get => _projectile; set => _projectile = value; }

    public override void ResetValues () {

        base.ResetValues ();

        Target = null;
        Projectile = null;

    }
    public override bool CanEquip (EquipmentSO equipment) {

        return equipment == null || (base.CanEquip (equipment) && equipment is WeaponSO);

    }
    public float GetAngleToTarget () { return Vector3.Angle (transform.forward, _target.transform.position - transform.position); }
    public float GetAngleTo (Structure target) { return Vector3.Angle (transform.forward, target.transform.position - transform.position); }
    public bool CanFireAtTarget () { return GetAngleToTarget () <= Arc / 2; }
    public bool CanFireAt (Structure target) { return GetAngleTo (target) <= Arc / 2; }

}
