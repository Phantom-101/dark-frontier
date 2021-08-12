using System;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Generator")]
public class GeneratorSO : EquipmentSO {
    public float Generation;

    public override void OnAwake (EquipmentSlot slot) {
        EnsureDataType (slot);
    }

    public override void OnEquip (EquipmentSlot slot) {
        slot.Data = new GeneratorSlotData {
            Slot = slot,
            Equipment = this,
            Durability = Durability,
        };
    }

    public override void OnUnequip (EquipmentSlot slot) {
        slot.Data = new EquipmentSlotData {
            Slot = slot,
            Equipment = null,
            Durability = 0,
        };
    }

    public override void Tick (EquipmentSlot slot, float dt) {
        EnsureDataType (slot);

        float pool = Generation * dt;
        slot.Equipper.GetEquipmentData<CapacitorSlotData> ().ForEach (capacitor => {
            float lack = (capacitor.Equipment as CapacitorSO).Capacitance - capacitor.Charge;
            float allocated = Mathf.Min (pool, lack, capacitor.ChargeLeft);
            capacitor.Charge += allocated;
            capacitor.ChargeLeft -= allocated;
            pool -= allocated;
        });
    }

    public override void FixedTick (EquipmentSlot slot, float dt) { }

    public override bool CanClick (EquipmentSlot slot) { return false; }

    public override void OnClicked (EquipmentSlot slot) { }

    public override void EnsureDataType (EquipmentSlot slot) {
        if (!(slot.Data is GeneratorSlotData)) slot.Data = new GeneratorSlotData {
            Slot = slot,
            Equipment = this,
            Durability = Durability,
        };
    }
}

[Serializable]
public class GeneratorSlotData : EquipmentSlotData {
    public override EquipmentSlotSaveData Save () {
        return new GeneratorSlotSaveData {
            EquipmentId = Equipment == null ? "" : Equipment.Id,
            Durability = Durability,
        };
    }
}

[Serializable]
public class GeneratorSlotSaveData : EquipmentSlotSaveData {
    public override EquipmentSlotData Load () {
        return new GeneratorSlotData {
            Equipment = ItemManager.Instance.GetItem (EquipmentId) as EquipmentSO,
            Durability = Durability,
        };
    }
}
