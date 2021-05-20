public class StructureSegmentSerializedParser {
    public static ISerializable<IStructureSegment> Parse (ISerialized<IStructureSegment> serialized) {
        if (serialized is StructureSegmentSerialized) return new StructureSegmentInstance (serialized as StructureSegmentSerialized);
        return null;
    }
}