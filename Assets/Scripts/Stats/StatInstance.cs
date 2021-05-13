using System.Collections.Generic;

public class StatInstance : IStat, ISerializable<IStat> {
    public float BaseValue {
        get;
        private set;
    }
    public List<IStatModifier> Modifiers {
        get;
        private set;
    }
    public string Id {
        get;
        private set;
    }
    public string Name {
        get;
        private set;
    }
    public string Description {
        get;
        private set;
    }

    public StatInstance (StatSaveData serialized) {
        BaseValue = serialized.BaseValue;
        Modifiers = serialized.Modifiers.ConvertAll ((e) => StatModifierSaveDataType.Parse (e) as IStatModifier);
        Id = serialized.Id;
        Name = serialized.Name;
        Description = serialized.Description;
    }

    public ISerialized<IStat> GetSerialized () { return new StatSaveData (this); }
}
