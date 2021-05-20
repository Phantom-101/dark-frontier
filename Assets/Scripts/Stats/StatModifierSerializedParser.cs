public class StatModifierSerializedParser {
    public static ISerializable<IStatModifier> Parse (ISerialized<IStatModifier> serialized) {
        if (serialized is StatModifierSerialized) return new StatModifierInstance (serialized as StatModifierSerialized);
        return null;
    }
}