using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat {
    public string Name;
    public float BaseValue;
    public Dictionary<string, StatModifier> Modifiers = new Dictionary<string, StatModifier> ();
    [SerializeField]
    private float _appliedValue;
    public float AppliedValue {
        get {
            if (!_dirty) return _appliedValue;
            float add = 0, multiply = 1, percentAdd = 0;
            foreach (StatModifier modifier in Modifiers.Values) {
                switch (modifier.Type) {
                    case StatModifierType.Additive:
                        add += modifier.Value;
                        break;
                    case StatModifierType.Multiplicative:
                        multiply *= modifier.Value;
                        break;
                    case StatModifierType.PercentAdditive:
                        percentAdd += modifier.Value;
                        break;
                    default:
                        Debug.Log ("Unknown modifier type applied");
                        break;
                }
            }
            _appliedValue = BaseValue + add + BaseValue * (multiply - 1) + BaseValue * percentAdd;
            _dirty = false;
            return _appliedValue;
        }
    }
    [SerializeField]
    private bool _dirty = true;
    
    public void AddModifier (StatModifier modifier) {
        Modifiers[modifier.Id] = modifier;
        _dirty = true;
    }

    public void RemoveModifier (StatModifier modifier) {
        Modifiers.Remove (modifier.Id);
        _dirty = true;
    }

    public void RemoveModifier (string id) {
        Modifiers.Remove (id);
        _dirty = true;
    }
}

public enum StatType {
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
