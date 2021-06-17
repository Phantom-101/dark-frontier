using System;
using UnityEngine;

[Serializable]
public class StructureStatModifier {
    public string Name;
    public string Id;
    public float Value;
    public StructureStatModifierType Type;
    public float Duration;

    public bool Expired { get => Duration <= 0; }

    public StructureStatModifier () { }

    public void Tick () { Duration -= Time.deltaTime; }

    public StructureStatModifier Copy () {
        return new StructureStatModifier {
            Name = Name,
            Id = Id,
            Value = Value,
            Type = Type,
            Duration = Duration,
        };
    }
}

public enum StructureStatModifierType {
    Additive,
    Multiplicative,
    PercentAdditive,
}
