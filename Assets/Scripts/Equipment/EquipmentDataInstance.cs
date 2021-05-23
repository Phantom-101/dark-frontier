using System;

[Serializable]
public class EquipmentDataInstance : IEquipmentData, ISerializable<IEquipmentData> {
    public IEquipment Equipment {
        get;
        private set;
    }

    public EquipmentDataInstance (EquipmentDataSerialized serialized) {
        // TODO find equipment by id
    }

    public ISerialized<IEquipmentData> GetSerialized () { return new EquipmentDataSerialized (this); }

    public void Initialize () { }
}

