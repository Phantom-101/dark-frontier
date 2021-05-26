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

public class StructureStatNames {
    public static string SensorStrength = "sensor-strength";
    public static string ScannerStrength = "scanner-strength";
    public static string MaxLocks = "max-locks";
    public static string Detectability = "detectability";
    public static string SignatureSize = "signature-size";
    public static string DockingBaySize = "docking-bay-size";
    public static string DamageMultiplier = "damage-multiplier";
    public static string RechargeMultiplier = "recharge-multiplier";
    public static string LinearSpeedMultiplier = "linear-speed-multiplier";
    public static string AngularSpeedMultiplier = "angular-speed-multiplier";
}
