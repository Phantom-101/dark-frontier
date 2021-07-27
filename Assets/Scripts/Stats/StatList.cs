using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class StatList : ISaveTo<StatListSaveData> {
    public List<Stat> Stats { get => stats; }
    [SerializeField] private List<Stat> stats = new List<Stat> ();
    public Dictionary<string, Stat> StatsDictionary { get => statsDictionary ?? (statsDictionary = stats.ToDictionary (s => s.Name, s => s)); }
    private Dictionary<string, Stat> statsDictionary;

    public StatList () { }
    public StatList (StatListSaveData saveData) => stats = saveData.Stats.ConvertAll (s => s.Load ());

    public Stat GetStat (string name, float baseValue) {
        if (StatsDictionary.ContainsKey (name)) return StatsDictionary[name];
        Stat ret = new Stat (name, baseValue);
        AddStat (ret);
        return ret;
    }

    public float GetBaseValue (string name, float baseValue) => GetStat (name, baseValue).BaseValue;
    public float GetAppliedValue (string name, float baseValue) => GetStat (name, baseValue).AppliedValue;
    public List<StatModifier> GetModifiers (string name) => GetStat (name, 0).Modifiers;

    public bool AddStat (Stat stat) {
        if (!StatsDictionary.ContainsKey (stat.Name)) {
            stats.Add (stat);
            statsDictionary = null;
            return true;
        }
        return false;
    }

    public bool SetStat (Stat stat) {
        if (StatsDictionary.TryGet (stat.Name, null) != stat) {
            int c = stats.RemoveAll (s => s.Name == stat.Name);
            stats.Add (stat);
            statsDictionary = null;
            return c > 0;
        }
        return false;
    }

    public bool RemoveStat (Stat stat) {
        if (stats.Remove (stat)) {
            statsDictionary = null;
            return true;
        }
        return false;
    }

    public bool RemoveStat (string name) {
        int c = stats.RemoveAll (s => s.Name == name);
        if (c > 0) {
            statsDictionary = null;
            return true;
        }
        return false;
    }

    public void Tick (float dt) => stats.ForEach (s => s.Modifiers.ForEach (m => m.Tick (dt)));

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
