using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Faction {

    [SerializeField] private string _name;
    [SerializeField] private string _id;
    [SerializeField] private long _wealth;
    private Dictionary<Faction, float> _relations = new Dictionary<Faction, float> ();

    public string GetName () { return _name; }

    public void SetName (string name) { _name = name; }

    public string GetId () { return _id; }

    public void SetId (string id) { _id = id; }

    public long GetWealth () { return _wealth; }

    public void SetWealth (long target) { _wealth = target; }

    public void ChangeWealth (long delta) { _wealth += delta; }

    public bool HasWealth (long condition) { return _wealth >= condition ? true : false; }

    public float GetRelation (Faction other) { return _relations.ContainsKey (other) ? _relations[other] : 0; }

    public void SetRelation (Faction other, float target) { _relations[other] = target; }

    public void ChangeRelation (Faction other, float delta) { _relations[other] = GetRelation (other) + delta; }

    public FactionSaveData GetSaveData () {

        FactionSaveData data = new FactionSaveData {

            Name = _name,
            Id = _id,
            Wealth = _wealth,
            RelationValues = _relations.Values.ToList ()

        };
        _relations.Keys.ToList ().ForEach (faction => {

            data.RelationIds.Add (faction.GetId ());

        });
        return data;

    }

    public void LoadSaveData (FactionSaveData saveData) {

        _name = saveData.Name;
        _id = saveData.Id;
        _wealth = saveData.Wealth;

    }

    public void LoadRelations (FactionSaveData saveData) {

        for (int i = 0; i < saveData.RelationIds.Count; i++) {

            _relations[FactionManager.GetInstance ().GetFaction (saveData.RelationIds[i])] = saveData.RelationValues[i];

        }

    }

}

[Serializable]
public class FactionSaveData {

    public string Name;
    public string Id;
    public long Wealth;
    public List<string> RelationIds = new List<string> ();
    public List<float> RelationValues = new List<float> ();

}