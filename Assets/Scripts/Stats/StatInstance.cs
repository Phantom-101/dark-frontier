using System;
using System.Collections.Generic;

public class StatInstance : IStat, ISerializable<IStat> {
    public float BaseValue {
        get;
        private set;
    }
    public Dictionary<string, IStatModifier> Modifiers {
        get;
        private set;
    }
    public float AppliedValue {
        get {
            float a = 0, m = 1, p = 0;
            foreach (IStatModifier modifier in Modifiers.Values) {
                if (modifier.ModifierType == StatModifierType.Add) a += modifier.Value;
                if (modifier.ModifierType == StatModifierType.Mult) m *= modifier.Value;
                if (modifier.ModifierType == StatModifierType.Percent) p += modifier.Value;
            }
            return a + BaseValue * (m + p);
        }
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

    public StatInstance (float baseValue, string name = "Unnamed", string description = "None") {
        BaseValue = baseValue;
        Modifiers = new Dictionary<string, IStatModifier> ();
        Id = Guid.NewGuid ().ToString ();
        Name = name;
        Description = description;
    }

    public StatInstance (StatSerialized serialized) {
        BaseValue = serialized.BaseValue;
        Modifiers = new Dictionary<string, IStatModifier> ();
        serialized.Modifiers.ConvertAll (e => StatModifierSerializedParser.Parse (e) as IStatModifier).ForEach (e => {
            Modifiers[e.Id] = e;
        });
        Id = serialized.Id;
        Name = serialized.Name;
        Description = serialized.Description;
    }

    public ISerialized<IStat> GetSerialized () { return new StatSerialized (this); }
}
