using System;

[Serializable]
public class EquipmentSlotSerialized : ISerialized<IEquipmentSlot> {
    public string EquipmentId;

    public EquipmentSlotSerialized (EquipmentSlotInstance serializable) {
        EquipmentId = serializable.Equipment.Id;
    }

    public ISerializable<IEquipmentSlot> GetSerializable () { return new EquipmentSlotInstance (this); }
}
