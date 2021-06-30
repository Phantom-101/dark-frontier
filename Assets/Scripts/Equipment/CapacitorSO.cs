using System;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Capacitor")]
public class CapacitorSO : EquipmentSO {
    public float Capacitance;
    public float MaxChargeRate;
    public float MaxDischargeRate;

    public override void OnAwake (EquipmentSlot slot) {
        EnsureDataType (slot);
    }

    public override void OnEquip (EquipmentSlot slot) {
        slot.Data = new CapacitorSlotData {
            Slot = slot,
            Equipment = this,
            Durability = Durability,
            Charge = 0,
            ChargeLeft = 0,
            DischargeLeft = 0,
        };
    }

    public override void OnUnequip (EquipmentSlot slot) {
        slot.Data = new EquipmentSlotData {
            Slot = slot,
            Equipment = null,
            Durability = 0,
        };
    }

    public override void Tick (EquipmentSlot slot) {
        EnsureDataType (slot);

        CapacitorSlotData data = slot.Data as CapacitorSlotData;

        data.ChargeLeft = MaxChargeRate * Time.deltaTime;
        data.DischargeLeft = MaxDischargeRate * Time.deltaTime;
    }

    public override void FixedTick (EquipmentSlot slot) { }

    public override bool CanClick (EquipmentSlot slot) { return false; }

    public override void OnClicked (EquipmentSlot slot) { }

    public override void EnsureDataType (EquipmentSlot slot) {
        if (!(slot.Data is CapacitorSlotData)) slot.Data = new CapacitorSlotData {
            Slot = slot,
            Equipment = this,
            Durability = Durability,
            Charge = 0,
            ChargeLeft = 0,
            DischargeLeft = 0,
        };
    }
}

[Serializable]
public class CapacitorSlotData : EquipmentSlotData {
    public float Charge;
    public float ChargeLeft;
    public float DischargeLeft;

    public override EquipmentSlotSaveData Save () {
        return new CapacitorSlotSaveData {
            EquipmentId = Equipment == null ? "" : Equipment.Id,
            Durability = Durability,
            Charge = Charge,
            ChargeLeft = ChargeLeft,
            DischargeLeft = DischargeLeft,
        };
    }
}

[Serializable]
public class CapacitorSlotSaveData : EquipmentSlotSaveData {
    public float Charge;
    public float ChargeLeft;
    public float DischargeLeft;

    public override EquipmentSlotData Load () {
        return new CapacitorSlotData {
            Equipment = ItemManager.Instance.GetItem (EquipmentId) as EquipmentSO,
            Durability = Durability,
            Charge = Charge,
            ChargeLeft = ChargeLeft,
            DischargeLeft = DischargeLeft,
        };
    }
}
