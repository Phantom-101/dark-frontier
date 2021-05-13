public interface IStatModifier : IIdentifiable, IInfo {
    float Value {
        get;
    }
    StatModifierType ModifierType {
        get;
    }
    float Duration {
        get;
    }
}