using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Initialization/Faction")]
public class FactionSO : ScriptableObject {

    public string Name;
    public string Id;
    public long Wealth;
    public StringToFloatMap Relations = new StringToFloatMap ();

    public Faction GetFaction () {

        Faction f = FactionManager.GetInstance ().GetFaction (Id);
        if (f != null) return f;

        f = new Faction ();
        f.SetId (Id);
        f.SetName (Name);
        f.SetWealth (Wealth);
        StringToFloatMap copy = new StringToFloatMap ();
        Relations.CopyTo (copy);
        f.SetRelations (copy);
        return f;

    }

}
