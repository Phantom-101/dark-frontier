using System;
using UnityEngine;

[Serializable]
public class StatModifier {
    public string Name;
    public string Id;
    public float Value;
    public StatModifierType Type;
    public float Duration;
    public bool Expired { get => Duration <= 0; }
    
    public void Tick () { Duration -= Time.deltaTime; }

    public StatModifier Copy () {
        return new StatModifier {
            Name = Name,
            Id = Id,
            Value = Value,
            Type = Type,
            Duration = Duration,
        };
    }
}

public enum StatModifierType {
    Additive,
    Multiplicative,
    PercentAdditive,
}
