﻿public class WeaponSO : EquipmentSO {

    public override void Tick (EquipmentSlot slot) {

        if (!Safe (slot)) return;

        // Check if there is a mismatch between current and target states
        if (slot.CurrentState != slot.TargetState) {

            if (slot.TargetState) {

                slot.Target = slot.Equipper.Selected;
                if (CanCycleStart (slot)) slot.CurrentState = true;
                else slot.TargetState = false;

            } else slot.CurrentState = false;

        }

        if (slot.CurrentState) {

            if (CanCycleStart (slot)) {

                if (slot.Energy == EnergyStorage) {

                    if (RequireCharge) {

                        if (slot.Equipper.GetInventoryCount (slot.Charge) > 0) {

                            slot.Equipper.ChangeInventoryCount (slot.Charge, -1);
                            slot.Energy = 0;
                            OnCycleStart (slot);
                            slot.Durability -= Wear;

                            if (!slot.Equipment.AutomaticallyRepeat) slot.TargetState = false;

                        } else slot.TargetState = false;

                    } else {

                        slot.Energy = 0;
                        OnCycleStart (slot);
                        slot.Durability -= Wear;

                        if (!slot.Equipment.AutomaticallyRepeat) slot.TargetState = false;

                    }

                }

                OnCycleTick (slot);

            } else slot.TargetState = false;

        }

        SafeTick (slot);

    }

}
