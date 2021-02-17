using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Weapons/Laser")]
public class LaserSO : WeaponSO {

    public DamageProfile Damage;
    public float Range;
    public GameObject LaserVFX;
    public float LaserWidth;

    public override bool CanCycleStart (EquipmentSlot slot) {

        if (!base.CanCycleStart (slot)) return false;
        if ((slot as WeaponSlot).Target == null) return false;
        if (!(slot as WeaponSlot).CanFireAt ((slot as WeaponSlot).Target)) return false;
        if ((slot.Equipper.transform.localPosition - (slot as WeaponSlot).Target.transform.localPosition).sqrMagnitude > Range * Range) return false;
        return true;

    }

    public override void OnCycleStart (EquipmentSlot slot) {

        WeaponSlot weapon = slot as WeaponSlot;
        weapon.Target.TakeDamage (Damage, slot.transform.position);

    }

    public override void SafeTick (EquipmentSlot slot) {

        WeaponSlot weapon = slot as WeaponSlot;

        if (slot.CurrentState) {

            if (weapon.Target != null) {

                if (weapon.Projectile == null) {

                    GameObject vfx = Instantiate (LaserVFX, slot.transform);
                    vfx.transform.localPosition = Vector3.zero;
                    weapon.Projectile = vfx;

                }

                float dis = Vector3.Distance (slot.transform.position, weapon.Target.transform.position);
                weapon.Projectile.transform.LookAt (weapon.Target.transform);
                weapon.Projectile.transform.localScale = new Vector3 (LaserWidth, LaserWidth, dis);

            }

        } else {

            if (weapon.Projectile != null) Destroy (weapon.Projectile);

        }

    }

}
