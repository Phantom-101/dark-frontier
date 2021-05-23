using System;

[Serializable]
public class EquipmentDataSerialized : ISerialized<IEquipmentData> {
    public string EquipmentId;

    public EquipmentDataSerialized (EquipmentDataInstance serializable) {
        EquipmentId = serializable.Equipment == null ? "" : serializable.Equipment.Id;
    }

    public ISerializable<IEquipmentData> GetSerializable () { return new EquipmentDataInstance (this); }
}
