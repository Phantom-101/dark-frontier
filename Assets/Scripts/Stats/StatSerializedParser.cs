public class StatSerializedParser {
    public static ISerializable<IStat> Parse (ISerialized<IStat> serialized) {
        if (serialized is StatSerialized) return new StatInstance (serialized as StatSerialized);
        return null;
    }
}