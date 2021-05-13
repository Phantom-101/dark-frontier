using System.Collections.Generic;

public interface IStat : IIdentifiable, IInfo {
    float BaseValue {
        get;
    }
    List<IStatModifier> Modifiers {
        get;
    }
}

