using System;
using UnityEngine;

public abstract class EquipmentSO : ItemSO {
    public float Durability;
    public GameObject ButtonPrefab;

    public abstract void OnAwake (EquipmentSlot slot);

    public abstract void OnEquip (EquipmentSlot slot);

    public abstract void OnUnequip (EquipmentSlot slot);

    public abstract void Tick (EquipmentSlot slot);

    public abstract void FixedTick (EquipmentSlot slot);

    public abstract bool CanClick (EquipmentSlot slot);

    public abstract void OnClicked (EquipmentSlot slot);

    public abstract void EnsureDataType (EquipmentSlot slot);
}

[Serializable]
public class EquipmentSlotData {
    public EquipmentSlot Slot;
    public EquipmentSO Equipment;
    public float Durability;

    public virtual EquipmentSlotSaveData Save () {
        return new EquipmentSlotSaveData {
            EquipmentId = Equipment == null ? "" : Equipment.Id,
            Durability = Durability,
        };
    }
}

[Serializable]
public class EquipmentSlotSaveData {
    public string EquipmentId;
    public float Durability;

    public virtual EquipmentSlotData Load () {
        return new EquipmentSlotData {
            Equipment = ItemManager.GetInstance ().GetItem (EquipmentId) as EquipmentSO,
            Durability = Durability,
        };
    }
}
