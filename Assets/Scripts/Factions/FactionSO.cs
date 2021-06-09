using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Initialization/Faction")]
public class FactionSO : ScriptableObject {

    public string Name;
    public string Id;
    public long Wealth;
    public StringToFloatDictionary Relations = new StringToFloatDictionary ();

    public Faction GetFaction () {

        Faction f = FactionManager.Instance.GetFaction (Id);
        if (f != null) return f;

        f = new Faction ();
        f.SetId (Id);
        f.SetName (Name);
        f.SetWealth (Wealth);
        StringToFloatDictionary copy = new StringToFloatDictionary ();
        Relations.CopyTo (copy);
        f.SetRelations (copy);
        return f;

    }

}
