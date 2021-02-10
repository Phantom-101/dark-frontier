using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Weapons/Laser")]
public class LaserSO : ChargeMinorWeaponSO {

    public GameObject LaserVFX;
    public float LaserWidth;

    public override bool Activate (EquipmentSlot slot) {

        if (base.Activate (slot)) {

            WeaponSlot weapon = slot as WeaponSlot;

            GameObject vfx = Instantiate (LaserVFX, slot.transform);
            vfx.transform.localPosition = Vector3.zero;
            float dis = Vector3.Distance (slot.GetLocalPosition (), weapon.GetTarget ().transform.localPosition);
            vfx.transform.LookAt (weapon.GetTarget ().transform);
            vfx.transform.localScale = new Vector3 (LaserWidth, LaserWidth, dis);
            weapon.SetProjectile (vfx);
            return true;

        }

        return false;

    }

    public override void Deactivate (EquipmentSlot slot) {

        base.Deactivate (slot);

        WeaponSlot weapon = slot as WeaponSlot;

        if (weapon.GetProjectile () != null) Destroy (weapon.GetProjectile ());

    }

    public override void Tick (EquipmentSlot slot) {

        base.Tick (slot);

        WeaponSlot weapon = slot as WeaponSlot;

        GameObject proj = weapon.GetProjectile ();
        if (proj != null && weapon.GetTarget () != null) {

            float dis = Vector3.Distance (slot.GetLocalPosition (), weapon.GetTarget ().transform.localPosition);
            proj.transform.LookAt (weapon.GetTarget ().transform.position);
            proj.transform.localScale = new Vector3 (LaserWidth, LaserWidth, dis);

        }

    }

}
