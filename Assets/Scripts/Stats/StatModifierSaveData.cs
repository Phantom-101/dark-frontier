using System;

[Serializable]
public class StatModifierSaveData : SerializedBase<IStatModifier> {
    public float Value;
    public short ModifierType;
    public float Duration;
    public string Id;
    public string Name;
    public string Description;

    public StatModifierSaveData (StatModifierInstance serializable) {
        DataType = StatModifierSaveDataType.Default;
        Value = serializable.Value;
        ModifierType = (short) serializable.ModifierType;
        Duration = serializable.Duration;
        Id = serializable.Id;
        Name = serializable.Name;
        Description = serializable.Description;
    }

    public override ISerializable<IStatModifier> GetSerializable () { return new StatModifierInstance (this); }
}