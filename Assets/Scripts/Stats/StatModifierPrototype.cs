using UnityEngine;

public class StatModifierPrototype : ScriptableObject, IStatModifier, IPrototype<IStatModifier> {
    public float Value {
        get;
        set;
    }
    public StatModifierType ModifierType {
        get;
        set;
    }
    public float Duration {
        get;
        set;
    }
    public string Id {
        get;
        set;
    }
    public string Name {
        get;
        set;
    }
    public string Description {
        get;
        set;
    }
    public IInstance<IStatModifier> Instance {
        get;
        set;
    }
}

