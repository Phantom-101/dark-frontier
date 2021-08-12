using DarkFrontier.Foundation.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Faction : ISaveTo<FactionSaveData> {
    public string Name;
    public string Id;
    public long Wealth;
    public StringToFloatDictionary Relations = new StringToFloatDictionary ();
    public List<Structure> Property = new List<Structure> ();

    public Faction () { }
    public Faction (FactionSaveData saveData) {
        Name = saveData.Name;
        Id = saveData.Id;
        Wealth = saveData.Wealth;
        Relations = saveData.Relations;
    }

    public void ChangeWealth (long delta) { Wealth += delta; }
    public bool HasWealth (long condition) { return Wealth >= condition; }
    public List<Structure> GetProperty () { return Property; }
    public void AddProperty (Structure structure) { Property.AddUnique (structure); }
    public void RemoveProperty (Structure structure) { Property.RemoveAll (structure); }
    public void SetRelations (StringToFloatDictionary map) { Relations = map; }
    public float GetRelation (string other) { return other == null ? 0 : Relations.TryGet (other, 0); }
    public void SetRelation (string other, float target) { if (other != null) { Relations[other] = Mathf.Clamp (target, -1, 1); FactionManager.Instance.GetFaction (other).SetRelationBack (Id, target); } }
    private void SetRelationBack (string back, float target) { if (back != null) { Relations[back] = Mathf.Clamp (target, -1, 1); } }
    public void ChangeRelation (string other, float delta) { SetRelation (other, GetRelation (other) + delta); }
    public bool IsAlly (string other) { return GetRelation (other) > 0.75f; }
    public bool IsEnemy (string other) { return GetRelation (other) < -0.75f; }
    public bool IsNeutral (string other) { return !IsAlly (other) && !IsEnemy (other); }

    public FactionSaveData GetSaveData () {
        FactionSaveData data = new FactionSaveData {
            Name = Name,
            Id = Id,
            Wealth = Wealth,
            Relations = Relations,
        };
        return data;
    }

    public FactionSaveData Save () {
        return new FactionSaveData {
            Name = Name,
            Id = Id,
            Wealth = Wealth,
            Relations = Relations,
        };
    }
}

[Serializable]
public class FactionSaveData : ILoadTo<Faction> {
    public string Name;
    public string Id;
    public long Wealth;
    public StringToFloatDictionary Relations;

    public Faction Load () {
        return new Faction (this);
    }
}