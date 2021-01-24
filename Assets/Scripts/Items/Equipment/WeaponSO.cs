public class WeaponSO : EquipmentSO {

    public float Damage;
    public float Range;

    public override bool CanActivate (EquipmentSlot slot) {

        if (slot.GetEquipment ().RequireCharge && slot.GetChargeQuantity () == 0) return false;
        if (slot.GetEquipper ().GetTarget () == null) return false;
        if ((slot.GetEquipper ().transform.localPosition - slot.GetEquipper ().GetTarget ().transform.localPosition).sqrMagnitude > Range * Range) return false;

        return true;

    }

    public override bool CanSustain (EquipmentSlot slot) { return CanActivate (slot); }

    public override bool Activate (EquipmentSlot slot) {

        if (!CanActivate (slot)) return false;

        (slot as WeaponSlot).SetTarget (slot.GetEquipper ().GetTarget ());
        slot.SetIsActive (true);
        return true;

    }

    public override void Deactivate (EquipmentSlot slot) {

        slot.SetIsActive (false);

    }

    public override void Tick (EquipmentSlot slot) {

        if (!CanSustain (slot)) Deactivate (slot);

        if (slot.IsActive ()) {

            if (slot.GetStoredEnergy () == EnergyStorage) {

                slot.SetStoredEnergy (0);
                if (slot.GetEquipment ().RequireCharge) slot.DepleteCharge (1);
                (slot as WeaponSlot).GetTarget ().TakeDamage (Damage, slot.GetSlotPosition ());

                slot.TakeDamage (Wear);

            }

        } else {

            if (slot.GetEquipment ().RequireCharge) {

                ChargeSO charge = slot.GetCharge ();

                if (charge == null) {

                    foreach (ChargeSO usable in slot.GetCharges ()) {

                        if (slot.GetEquipper ().HasInventoryCount (usable, 1) && slot.CanLoadCharge (usable, 1)) {

                            slot.GetEquipper ().ChangeInventoryCount (usable, -1);
                            slot.LoadCharge (usable, 1);
                            break;

                        }

                    }

                } else {

                    if (slot.GetEquipper ().HasInventoryCount (charge, 1) && slot.CanLoadCharge (charge, 1)) {

                        slot.GetEquipper ().ChangeInventoryCount (charge, -1);
                        slot.LoadCharge (charge, 1);

                    }


                }

            }

        }

    }

}
