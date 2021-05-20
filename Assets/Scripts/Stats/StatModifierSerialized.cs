using System;

[Serializable]
public class StatModifierSerialized : ISerialized<IStatModifier> {
    public float Value;
    public short ModifierType;
    public float Duration;
    public string Id;
    public string Name;
    public string Description;

    public StatModifierSerialized (StatModifierInstance serializable) {
        Value = serializable.Value;
        ModifierType = (short) serializable.ModifierType;
        Duration = serializable.Duration;
        Id = serializable.Id;
        Name = serializable.Name;
        Description = serializable.Description;
    }

    public ISerializable<IStatModifier> GetSerializable () { return new StatModifierInstance (this); }
}