using System;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Shield")]
public class ShieldSO : EquipmentSO {
    public float MaxStrength;
    public float RechargeConsumption;
    public float RechargeEfficiency;

    public override void OnAwake (EquipmentSlot slot) {
        EnsureDataType (slot);
    }

    public override void OnEquip (EquipmentSlot slot) {
        slot.Data = new ShieldSlotData {
            Slot = slot,
            Equipment = this,
            Durability = Durability,
            Strength = 0,
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

        ShieldSlotData data = slot.Data as ShieldSlotData;

        float consumption = RechargeConsumption * Time.deltaTime;
        float lack = (MaxStrength - data.Strength) / RechargeEfficiency;
        float request = Mathf.Min (consumption, lack);
        float given = 0;
        slot.Equipper.GetEquipmentData<CapacitorSlotData> ().ForEach (capacitor => {
            float chargeLeft = capacitor.Charge;
            float dischargeLeft = capacitor.DischargeLeft;
            float allocated = Mathf.Min (chargeLeft, dischargeLeft, request - given);
            given += allocated;
            capacitor.Charge -= allocated;
            capacitor.DischargeLeft -= allocated;
        });

        data.Strength += given * RechargeEfficiency;
    }

    public override void FixedTick (EquipmentSlot slot) { }

    public override bool CanClick (EquipmentSlot slot) { return false; }

    public override void OnClicked (EquipmentSlot slot) { }

    public override void EnsureDataType (EquipmentSlot slot) {
        if (!(slot.Data is ShieldSlotData)) slot.Data = new ShieldSlotData {
            Slot = slot,
            Equipment = this,
            Durability = Durability,
            Strength = 0,
        };
    }
}

[Serializable]
public class ShieldSlotData : EquipmentSlotData {
    public float Strength;

    public override EquipmentSlotSaveData Save () {
        return new ShieldSlotSaveData {
            EquipmentId = Equipment == null ? "" : Equipment.Id,
            Durability = Durability,
            Strength = Strength,
        };
    }
}

[Serializable]
public class ShieldSlotSaveData : EquipmentSlotSaveData {
    public float Strength;

    public override EquipmentSlotData Load () {
        return new ShieldSlotData {
            Equipment = ItemManager.Instance.GetItem (EquipmentId) as EquipmentSO,
            Durability = Durability,
            Strength = Strength,
        };
    }
}
