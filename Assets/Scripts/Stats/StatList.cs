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
    public Dictionary<string, Stat> StatsDictionary {
        get => stats.ToDictionary (s => s.Name, s => s);
    }

    public StatList () { }
    public StatList (StatListSaveData saveData) => stats = saveData.Stats.ConvertAll (s => s.Load ());

    public Stat GetStat (string name, float baseValue) {
        Stat ret = StatsDictionary.TryGet (name, new Stat (name, baseValue));
        SetStat (ret);
        return ret;
    }

    public float GetBaseValue (string name, float baseValue) => GetStat (name, baseValue).BaseValue;
    public float GetAppliedValue (string name, float baseValue) => GetStat (name, baseValue).AppliedValue;
    public List<StatModifier> GetModifiers (string name) => GetStat (name, 0).Modifiers;
    public bool GetIsDirty (string name) => GetStat (name, 0).IsDirty;

    public bool AddStat (Stat stat) {
        if (!StatsDictionary.ContainsKey (stat.Name)) {
            stats.Add (stat);
            return true;
        }
        return false;
    }

    public bool SetStat (Stat stat) {
        int c = stats.RemoveAll (s => s.Name == stat.Name);
        stats.Add (stat);
        return c > 0;
    }

    public void RemoveStat (Stat stat) => stats.Remove (stat);
    public void RemoveStat (string name) => stats.RemoveAll (s => s.Name == name);

    public void Tick () => stats.ForEach (s => s.Modifiers.ForEach (m => m.Tick ()));

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
