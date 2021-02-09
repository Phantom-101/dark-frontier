using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Faction {

    [SerializeField] private string _name;
    [SerializeField] private string _id;
    [SerializeField] private long _wealth;
    [SerializeField] private List<Structure> _property = new List<Structure> ();
    [SerializeField] private StringToFloatDictionary _relations = new StringToFloatDictionary ();

    public string GetName () { return _name; }

    public void SetName (string name) { _name = name; }

    public string GetId () { return _id; }

    public void SetId (string id) { _id = id; }

    public long GetWealth () { return _wealth; }

    public void SetWealth (long target) { _wealth = target; }

    public void ChangeWealth (long delta) { _wealth += delta; }

    public bool HasWealth (long condition) { return _wealth >= condition ? true : false; }

    public List<Structure> GetProperty () { return _property; }

    public void AddProperty (Structure structure) { _property.Add (structure); }

    public void RemoveProperty (Structure structure) { _property.Remove (structure); }

    public void SetRelations (StringToFloatDictionary map) { _relations = map; }

    public float GetRelation (Faction other) { return _relations.ContainsKey (other._id) ? _relations[other._id] : 0; }

    public void SetRelation (Faction other, float target) { _relations[other._id] = Mathf.Clamp (target, -1, 1); other.SetRelationBack (this, target); }

    private void SetRelationBack (Faction back, float target) { _relations[back._id] = Mathf.Clamp (target, -1, 1); }

    public void ChangeRelation (Faction other, float delta) { SetRelation (other, GetRelation (other) + delta); }

    public bool IsAlly (Faction other) { return GetRelation (other) > 0.75f ? true : false; }

    public bool IsEnemy (Faction other) { return GetRelation (other) < -0.75f ? true : false; }

    public bool IsNeutral (Faction other) { return !IsAlly (other) && !IsEnemy (other); }

    public FactionSaveData GetSaveData () {

        FactionSaveData data = new FactionSaveData {

            Name = _name,
            Id = _id,
            Wealth = _wealth,
            RelationIds = _relations.Keys.ToList (),
            RelationValues = _relations.Values.ToList ()

        };
        return data;

    }

    public void LoadSaveData (FactionSaveData saveData) {

        _name = saveData.Name;
        _id = saveData.Id;
        _wealth = saveData.Wealth;
        for (int i = 0; i < saveData.RelationIds.Count; i++) {

            _relations[saveData.RelationIds[i]] = saveData.RelationValues[i];

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