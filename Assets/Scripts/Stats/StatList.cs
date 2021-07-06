using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class StatList : ISaveTo<StatListSaveData> {
    public List<Stat> Stats {
        get => stats.Values.ToList ();
    }
    [SerializeField] private StringToStatDictionary stats = new StringToStatDictionary ();

    // Default constructor
    public StatList () { }
    // From save data
    public StatList (StatListSaveData saveData) {
        stats = saveData.Stats.ConvertAll (s => s.Load ()).ToDictionary (s => s.Name, s => s).ToSerializable<string, Stat, StringToStatDictionary> ();
    }

    public Stat GetStat (string name, float baseValue) => stats.TryGet (name, new Stat (name, baseValue));
    public float GetBaseValue (string name, float baseValue) => GetStat (name, baseValue).BaseValue;
    public float GetAppliedValue (string name, float baseValue) => GetStat (name, baseValue).AppliedValue;
    public List<StatModifier> GetModifiers (string name) => GetStat (name, 0).Modifiers;
    public bool GetIsDirty (string name) => GetStat (name, 0).IsDirty;
    public void AddStat (Stat stat) => stats[stat.Name] = stat;
    public void TryAddStat (Stat stat) => stats.TryAdd (stat.Name, stat);
    public void RemoveStat (Stat stat) => stats.Remove (stat.Name);
    public void RemoveStat (string name) => stats.Remove (name);

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
