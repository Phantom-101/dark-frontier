using UnityEditor;
using UnityEngine;

public class WeaponSlot : EquipmentSlot {

    [SerializeField] protected float _arc;
    [SerializeField] protected Structure _target;
    [SerializeField] protected GameObject _projectile;

    public WeaponSO GetWeapon () { return _equipment as WeaponSO; }

    public float GetArc () { return _arc; }

    public float GetAngleToTarget () { return Vector3.Angle (transform.forward, _target.transform.position - transform.position); }

    public float GetAngleTo (Structure target) { return Vector3.Angle (transform.forward, target.transform.position - transform.position); }

    public bool CanFireAtTarget () { return GetAngleToTarget () <= _arc / 2; }

    public bool CanFireAt (Structure target) { return GetAngleTo (target) <= _arc / 2; }

    public Structure GetTarget () { return _target; }

    public void SetTarget (Structure target) { _target = target; }

    public GameObject GetProjectile () { return _projectile; }

    public void SetProjectile (GameObject projectile) { _projectile = projectile; }

    public override bool CanEquip (EquipmentSO equipment) {

        return base.CanEquip (equipment) || (equipment is WeaponSO && equipment.Tier <= _equipper.GetProfile ().MaxEquipmentTier);

    }

    protected override void ResetValues () {

        base.ResetValues ();

        _target = null;
        _active = false;

    }

}

[CustomEditor (typeof (WeaponSlot))]
[InitializeOnLoad]
public class WeaponSlotEditor : Editor {

    [DrawGizmo (GizmoType.InSelectionHierarchy)]
    private static void DrawArc (WeaponSlot ws, GizmoType gizmoType) {

        Handles.color = new Color (1, 0, 0, 0.1f);
        Handles.DrawSolidArc (ws.transform.position, ws.transform.up, Quaternion.Euler (0, -ws.GetArc () / 2, 0) * ws.transform.forward, ws.GetArc (), ws.GetWeapon () == null ? 10 : ws.GetWeapon ().Range);

    }

}