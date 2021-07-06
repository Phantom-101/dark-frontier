using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class StatList : ISaveTo<StatListSaveData> {
    public List<Stat> Stats {
        get => stats;
    }
    [SerializeField] private List<Stat> stats = new List<Stat> ();
    public ILookup<string, Stat> StatsLookup {
        get => stats.ToLookup (s => s.Name, s => s);
    }

    // Default constructor
    public StatList () { }
    // From save data
    public StatList (StatListSaveData saveData) => stats = saveData.Stats.ConvertAll (s => s.Load ());

    public Stat GetStat (string name, float baseValue) {
        if (StatsLookup.Contains (name)) return StatsLookup[name].First ();
        return new Stat (name, baseValue);
    }
    public float GetBaseValue (string name, float baseValue) => GetStat (name, baseValue).BaseValue;
    public float GetAppliedValue (string name, float baseValue) => GetStat (name, baseValue).AppliedValue;
    public List<StatModifier> GetModifiers (string name) => GetStat (name, 0).Modifiers;
    public bool GetIsDirty (string name) => GetStat (name, 0).IsDirty;
    public void AddStat (Stat stat) {
        if (!StatsLookup.Contains (stat.Name)) {
            stats.Add (stat);
        }
    }
    public void RemoveStat (Stat stat) => stats.Remove (stat);
    public void RemoveStat (string name) => stats.RemoveAll (s => s.Name == name);

    public StatListSaveData Save () {
        return new StatListSaveData {
            Stats = Stats.ConvertAll (s => s.Save ()),
        };
    }
}

[Serializable]
public class StatListSaveData : ILoadTo<StatList> {
    public List<StatSaveData> Stats;

    public StatList Load () => new StatList (this);
}
