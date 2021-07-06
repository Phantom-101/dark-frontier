using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Stat : ISaveTo<StatSaveData> {
    // The name of this stat
    public string Name {
        get => name;
    }
    [SerializeField] private string name;

    // The value upon which modifiers make their modifications
    public float BaseValue {
        get => baseValue;
        set {
            baseValue = value;
            isDirty = true;
        }
    }
    [SerializeField] private float baseValue;

    // The effective value of this stat after all modifiers have been taken into account
    public float AppliedValue {
        get => GetAppliedValue ();
    }
    [SerializeField] private float appliedValue;

    // Collection of modifiers which modify the base value
    public List<StatModifier> Modifiers {
        get => modifiers.Values.ToList ();
    }
    [SerializeField] private StringToStatModifierDictionary modifiers = new StringToStatModifierDictionary ();

    // Denotes whether or not applied value needs to be recalculated
    public bool IsDirty {
        get => isDirty;
    }
    [SerializeField] private bool isDirty = true;

    // Constructors
    public Stat (string name) : this (name, 0) { }
    public Stat (string name, float baseValue) : this (name, baseValue, new List<StatModifier> ()) { }
    public Stat (string name, float baseValue, List<StatModifier> modifiers) {
        this.name = name;
        this.baseValue = baseValue;
        this.modifiers = modifiers.ToDictionary (m => m.Id, m => m).ToSerializable<string, StatModifier, StringToStatModifierDictionary> ();
    }
    // From save data
    public Stat (StatSaveData saveData) {
        name = saveData.Name;
        baseValue = saveData.BaseValue;
        modifiers = saveData.Modifiers.ToDictionary (m => m.Id, m => m).ToSerializable<string, StatModifier, StringToStatModifierDictionary> ();
    }

    public void AddModifier (StatModifier modifier) {
        modifiers[modifier.Id] = modifier;
        isDirty = true;
    }

    public void RemoveModifier (StatModifier modifier) {
        modifiers.Remove (modifier.Id);
        isDirty = true;
    }

    public void RemoveModifier (string id) {
        modifiers.Remove (id);
        isDirty = true;
    }

    private float GetAppliedValue () {
        // If not dirty, then return cached value
        if (!isDirty) return appliedValue;
        // Prepare variables
        float add = 0, multiply = 1, percentAdd = 0;
        // Iterate through modifiers and add to respective variables
        foreach (StatModifier modifier in modifiers.Values) {
            if (modifier.Type == StatModifierType.Additive) add += modifier.Value;
            else if (modifier.Type == StatModifierType.Multiplicative) multiply *= modifier.Value;
            else if (modifier.Type == StatModifierType.PercentAdditive) percentAdd += modifier.Value;
        }
        // Calculate applied value and set dirty to false
        appliedValue = BaseValue + add + BaseValue * (multiply - 1) + BaseValue * percentAdd;
        isDirty = false;
        // Return result
        return appliedValue;
    }

    public Stat Copy () {
        return new Stat (name, baseValue, Modifiers.ConvertAll (m => m.Copy ()));
    }

    public StatSaveData Save () {
        return new StatSaveData {
            Name = name,
            BaseValue = baseValue,
            Modifiers = Modifiers,
        };
    }
}

[Serializable]
public class StatSaveData : ILoadTo<Stat> {
    public string Name;
    public float BaseValue;
    public List<StatModifier> Modifiers;

    public Stat Load () => new Stat (this);
}

public class StatNames {
    public static string MaxHull = "Max Hull";
    public static string LinearAccelerationMultiplier = "Linear Acceleration Multiplier";
    public static string AngularAccelerationMultiplier = "Angular Acceleration Multiplier";
    public static string SensorStrength = "Sensor Strength";
    public static string Detectability = "Detectability";
    public static string ScannerStrength = "Scanner Strength";
    public static string SignatureSize = "Signature Size";
    public static string MaxTargetLocks = "Max Target Locks";
    public static string InventoryVolume = "Inventory Volume";
    public static string DockingBayVolume = "Docking Bay Volume";
    public static string CargoDropPercentage = "Cargo Drop Percentage";
}
