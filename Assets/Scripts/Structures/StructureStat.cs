using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StructureStat {
    public string Name;
    public float BaseValue;
    public Dictionary<string, StructureStatModifier> Modifiers = new Dictionary<string, StructureStatModifier> ();
    [SerializeField] private float _appliedValue;
    [SerializeField] private bool _dirty = true;

    public StructureStat () { }

    public void AddModifier (StructureStatModifier modifier) {
        Modifiers[modifier.Id] = modifier;
        _dirty = true;
    }

    public void RemoveModifier (StructureStatModifier modifier) {
        Modifiers.Remove (modifier.Id);
        _dirty = true;
    }

    public void RemoveModifier (string id) {
        Modifiers.Remove (id);
        _dirty = true;
    }

    public float GetAppliedValue () {
        if (!_dirty) return _appliedValue;

        float add = 0, multiply = 1, percentAdd = 0;
        foreach (StructureStatModifier modifier in Modifiers.Values) {
            switch (modifier.Type) {
                case StructureStatModifierType.Additive:
                    add += modifier.Value;
                    break;
                case StructureStatModifierType.Multiplicative:
                    multiply *= modifier.Value;
                    break;
                case StructureStatModifierType.PercentAdditive:
                    percentAdd += modifier.Value;
                    break;
                default:
                    Debug.Log ("Unknown modifier type applied.");
                    break;
            }
        }

        _appliedValue = BaseValue + add + BaseValue * (multiply - 1) + BaseValue * percentAdd;
        _dirty = false;

        return _appliedValue;
    }
}

public enum StructureStatType {
    MaxHull,
    MaxShieldStrengthMultiplier,
    SensorStrength,
    ScannerStrength,
    Detectability,
    SignatureSize,
    MaxLocks,
    DockingBaySize,
    DamageMultiplier,
    RechargeMultiplier,
    LinearSpeedMultiplier,
    AngularSpeedMultiplier,
    DropPercentage,
}
