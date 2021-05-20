public class StructureSerializedParser {
    public static ISerializable<IStructure> Parse (ISerialized<IStructure> serialized) {
        if (serialized is StructureSerialized) return new StructureInstance (serialized as StructureSerialized);
        return null;
    }
}