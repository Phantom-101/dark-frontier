using System;

[Serializable]
public class EquipmentSlotInstance : IEquipmentSlot, ISerializable<IEquipmentSlot> {
    public EquipmentSO Equipment {
        get;
        private set;
    }

    public EquipmentSlotInstance (EquipmentSlotSerialized serialized) {
        // TODO find equipment by id
    }

    public ISerialized<IEquipmentSlot> GetSerialized () { return new EquipmentSlotSerialized (this); }
}

