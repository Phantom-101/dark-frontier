public class EquipmentSlotSerializedParser {
    public static ISerializable<IEquipmentSlot> Parse (ISerialized<IEquipmentSlot> serialized) {
        if (serialized is EquipmentSlotSerialized) return new EquipmentSlotInstance (serialized as EquipmentSlotSerialized);
        return null;
    }
}

