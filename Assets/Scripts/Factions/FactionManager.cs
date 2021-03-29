using System.Collections.Generic;
using UnityEngine;

public class FactionManager : MonoBehaviour {

    [SerializeField] private List<Faction> _factions = new List<Faction> ();
    [SerializeField] private List<FactionSO> _initialization = new List<FactionSO> ();

    private static FactionManager _instance;

    private void Awake () {

        _instance = this;

        _initialization.ForEach (faction => {

            _factions.Add (faction.GetFaction ());

        });

        _initialization = new List<FactionSO> ();

    }

    public List<Faction> GetFactions () { return _factions; }

    public void AddFaction (Faction faction) { if (!_factions.Contains (faction)) _factions.Add (faction); }

    public void RemoveFaction (Faction faction) { _factions.Remove (faction); }

    public Faction GetFaction (string id) {

        Faction found = null;
        _factions.ForEach (faction => {

            if (faction.GetId () == id) found = faction;

        });
        return found;

    }

    public void SaveGame (string timestamp) {

        string saveName = SaveManager.GetInstance ().GetUniverse ();

        List<FactionSaveData> saveData = new List<FactionSaveData> ();
        _factions.ForEach (faction => { saveData.Add (faction.GetSaveData ()); });

        SerializationManager.Save (Application.persistentDataPath + "/saves/" + saveName + "/" + timestamp, "factions.save", JsonHelper.ToJson (saveData));

    }

    public void LoadGame (string saveData) {

        List<FactionSaveData> factions = JsonHelper.ListFromJson<FactionSaveData> (saveData);

        _factions = new List<Faction> ();

        factions.ForEach (data => {

            Faction faction = new Faction ();
            faction.LoadSaveData (data);
            _factions.Add (faction);

        });

    }

    public static FactionManager GetInstance () { return _instance; }

}
