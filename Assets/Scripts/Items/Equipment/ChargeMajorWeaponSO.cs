public class ChargeMajorWeaponSO : EquipmentSO {

    public DamageProfile DamageMultiplier;
    public float RangeMultiplier;

    public override bool CanActivate (EquipmentSlot slot) {

        if (slot.GetChargeQuantity () == 0) return false;
        if (slot.GetEquipper ().GetTarget () == null) return false;
        if (!(slot as WeaponSlot).CanFireAt (slot.GetEquipper ().GetTarget ())) return false;
        if (!WithinRange (slot)) return false;

        return true;

    }

    public virtual bool WithinRange (EquipmentSlot slot) {

        return false;

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
                SpawnCharge (slot);

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

    public virtual void SpawnCharge (EquipmentSlot slot) { }

}
