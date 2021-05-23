public class EquipmentDataSerializedParser {
    public static ISerializable<IEquipmentData> Parse (ISerialized<IEquipmentData> serialized) {
        if (serialized is EquipmentDataSerialized) return new EquipmentDataInstance (serialized as EquipmentDataSerialized);
        return null;
    }
}
