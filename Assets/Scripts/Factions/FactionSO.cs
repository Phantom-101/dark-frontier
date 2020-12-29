using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Initialization/Faction")]
public class FactionSO : ScriptableObject {

    public string Name;
    public string Id;
    public long Wealth;
    public List<string> RelationIds = new List<string> ();
    public List<float> RelationValues = new List<float> ();

    public Faction GetFaction () {

        Faction f = FactionManager.GetInstance ().GetFaction (Id);
        if (f != null) return f;

        f = new Faction ();
        f.SetId (Id);
        f.SetName (Name);
        f.SetWealth (Wealth);
        return f;

    }

    public void SetRelations () {

        FactionManager manager = FactionManager.GetInstance ();
        Faction f = manager.GetFaction (Id);

        for (int i = 0; i < RelationIds.Count; i++) {

            f.SetRelation (manager.GetFaction (RelationIds[i]), RelationValues[i]);

        }

    }

}
