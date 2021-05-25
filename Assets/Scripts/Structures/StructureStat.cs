using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StructureStat {

    [SerializeField] private string _name;
    [SerializeField] private float _baseValue;
    [SerializeField] private List<StructureStatModifier> _modifiers;
    [SerializeField] private float _appliedValue;
    [SerializeField] private bool _dirty;

    public StructureStat (string name, float baseValue) : this (name, baseValue, new List<StructureStatModifier> ()) { }

    public StructureStat (string name, float baseValue, List<StructureStatModifier> modifiers) {

        _name = name;
        _baseValue = baseValue;
        _modifiers = modifiers;
        _appliedValue = 0;
        _dirty = true;

    }

    public string GetName () { return _name; }

    public float GetBaseValue () { return _baseValue; }

    public List<StructureStatModifier> GetModifiers () { return _modifiers; }

    public void AddModifier (StructureStatModifier modifier) {

        if (!_modifiers.Contains (modifier)) {

            _modifiers.Add (modifier);
            _dirty = true;

        }

    }

    public void RemoveModifier (StructureStatModifier modifier) {

        _modifiers.Remove (modifier);
        _dirty = true;

    }

    public float GetAppliedValue () {

        if (!_dirty) return _appliedValue;

        float add = 0, multiply = 1, percentAdd = 0;
        foreach (StructureStatModifier modifier in _modifiers) {

            switch (modifier.GetModifierType ()) {

                case StructureStatModifierType.Additive:
                    add += modifier.GetValue ();
                    break;
                case StructureStatModifierType.Multiplicative:
                    multiply *= modifier.GetValue ();
                    break;
                case StructureStatModifierType.PercentAdditive:
                    percentAdd += modifier.GetValue ();
                    break;
                default:
                    Debug.Log ("Unknown modifier type applied.");
                    break;

            }

        }

        _appliedValue = _baseValue + add + _baseValue * (multiply - 1) + _baseValue * percentAdd;
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
    public static string LinearSpeedMultiplier = "linear-speed_multiplier";
    public static string AngularSpeedMultiplier = "angular-speed_multiplier";
}
