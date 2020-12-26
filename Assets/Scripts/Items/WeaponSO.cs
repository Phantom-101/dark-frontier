using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Weapon")]
public class WeaponSO : EquipmentSO {

    public double Damage;
    public double Range;

    public override bool CanActivate (EquipmentSlot slot) {

        if (slot.GetEquipper ().GetTarget () == null) return false;
        if ((slot.GetEquipper ().transform.localPosition - slot.GetEquipper ().GetTarget ().transform.localPosition).sqrMagnitude > Range * Range) return false;

        return true;

    }

    public override bool CanSustain (EquipmentSlot slot) { return CanActivate (slot); }

    public override void Activate (EquipmentSlot slot) {

        if (!CanActivate (slot)) return;

        (slot as WeaponSlot).SetTarget (slot.GetEquipper ().GetTarget ());
        slot.SetIsActive (true);

    }

    public override void Deactivate (EquipmentSlot slot) {

        slot.SetIsActive (false);

    }

    public override void Tick (EquipmentSlot slot) {

        if (!CanSustain (slot)) Deactivate (slot);

        if (slot.IsActive ()) {

            if (slot.GetStoredEnergy () == EnergyStorage) {

                slot.SetStoredEnergy (0);
                (slot as WeaponSlot).GetTarget ().ChangeHull (-Damage);

            } else {

                slot.ChangeStoredEnergy (Time.deltaTime);

            }

        }

    }

}
