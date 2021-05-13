public class StatModifierInstance : IStatModifier, IInstance<IStatModifier>, ISerializable<IStatModifier> {
    public float Value {
        get;
        private set;
    }
    public StatModifierType ModifierType {
        get;
        private set;
    }
    public float Duration {
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
    public IPrototype<IStatModifier> Prototype {
        get;
        private set;
    }

    public StatModifierInstance (StatModifierSaveData serialized) {
        Value = serialized.Value;
        ModifierType = (StatModifierType) serialized.ModifierType;
        Duration = serialized.Duration;
        Id = serialized.Id;
        Name = serialized.Name;
        Description = serialized.Description;
    }

    public ISerialized<IStatModifier> GetSerialized () { return new StatModifierSaveData (this); }
}
