using System.Collections.Generic;

public interface IStat : IIdentifiable, IInfo {
    float BaseValue {
        get;
    }
    Dictionary<string, IStatModifier> Modifiers {
        get;
    }
    float AppliedValue {
        get;
    }
}

