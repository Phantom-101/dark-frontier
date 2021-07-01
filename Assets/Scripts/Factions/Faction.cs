using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Initialization/Faction")]
public class Faction : ScriptableObject {
    public string Name;
    public string Id;
    public long Wealth;
    public StringToFloatDictionary Relations = new StringToFloatDictionary ();
    public HashSet<Structure> Property = new HashSet<Structure> ();

    public void ChangeWealth (long delta) { Wealth += delta; }
    public bool HasWealth (long condition) { return Wealth >= condition ? true : false; }
    public HashSet<Structure> GetProperty () { return Property; }
    public void AddProperty (Structure structure) { Property.Add (structure); }
    public void RemoveProperty (Structure structure) { Property.Remove (structure); }
    public void SetRelations (StringToFloatDictionary map) { Relations = map; }
    public float GetRelation (Faction other) { return Relations.ContainsKey (other.Id) ? Relations[other.Id] : 0; }
    public void SetRelation (Faction other, float target) { Relations[other.Id] = Mathf.Clamp (target, -1, 1); other.SetRelationBack (this, target); }
    private void SetRelationBack (Faction back, float target) { Relations[back.Id] = Mathf.Clamp (target, -1, 1); }
    public void ChangeRelation (Faction other, float delta) { SetRelation (other, GetRelation (other) + delta); }
    public bool IsAlly (Faction other) { return GetRelation (other) > 0.75f; }
    public bool IsEnemy (Faction other) { return GetRelation (other) < -0.75f; }
    public bool IsNeutral (Faction other) { return !IsAlly (other) && !IsEnemy (other); }

    public FactionSaveData GetSaveData () {
        FactionSaveData data = new FactionSaveData {
            Name = Name,
            Id = Id,
            Wealth = Wealth,
            Relations = Relations,
        };
        return data;
    }

    public void LoadSaveData (FactionSaveData saveData) {
        Name = saveData.Name;
        Id = saveData.Id;
        Wealth = saveData.Wealth;
        Relations = saveData.Relations;
    }
}

[Serializable]
public class FactionSaveData {
    public string Name;
    public string Id;
    public long Wealth;
    public StringToFloatDictionary Relations;
}