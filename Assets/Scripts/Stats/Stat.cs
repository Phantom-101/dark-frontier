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
        get => modifiers;
    }
    [SerializeField] private List<StatModifier> modifiers = new List<StatModifier> ();
    public ILookup<string, StatModifier> ModifiersLookup {
        get => modifiers.ToLookup (m => m.Id, m => m);
    }

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
        modifiers.ForEach (m => AddModifier (m));
    }
    // From save data
    public Stat (StatSaveData saveData) {
        name = saveData.Name;
        baseValue = saveData.BaseValue;
        saveData.Modifiers.ForEach (m => AddModifier (m));
    }

    public void AddModifier (StatModifier modifier) {
        if (!ModifiersLookup.Contains (modifier.Id)) {
            modifiers.Add (modifier);
            isDirty = true;
        }
    }

    public void RemoveModifier (StatModifier modifier) {
        if (modifiers.Remove (modifier)) {
            isDirty = true;
        }
    }

    public void RemoveModifier (string id) {
        if (modifiers.RemoveAll (m => m.Id == id) > 0) {
            isDirty = true;
        }
    }

    private float GetAppliedValue () {
        // If not dirty, then return cached value
        if (!isDirty) return appliedValue;
        // Prepare variables
        float add = 0, multiply = 1, percentAdd = 0;
        // Iterate through modifiers and add to respective variables
        foreach (StatModifier modifier in modifiers) {
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
