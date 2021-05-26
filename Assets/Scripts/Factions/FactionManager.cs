using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FactionManager : MonoBehaviour {

    [SerializeField] private List<Faction> _factions = new List<Faction> ();
    [SerializeField] private List<FactionSO> _initialization = new List<FactionSO> ();

    private static FactionManager _instance;

    private void Awake () {

        _instance = this;
        Debug.Log ("FactionManager instance set");

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

    public void SaveGame (DirectoryInfo directory) {
        List<FactionSaveData> saveData = new List<FactionSaveData> ();
        _factions.ForEach (faction => { saveData.Add (faction.GetSaveData ()); });
        FileInfo file = PathManager.GetFactionFile (directory);
        if (!file.Exists) file.Create ().Close ();
        File.WriteAllText (
            file.FullName,
            JsonConvert.SerializeObject (
                saveData,
                Formatting.Indented,
                new JsonSerializerSettings {
                    TypeNameHandling = TypeNameHandling.All,
                }
            )
        );
    }

    public void LoadGame (DirectoryInfo directory) {
        FileInfo file = PathManager.GetFactionFile (directory);
        if (!file.Exists) return;
        List<FactionSaveData> factions = JsonConvert.DeserializeObject (
            File.ReadAllText (file.FullName),
            new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.All,
            }
        ) as List<FactionSaveData>;
        _factions = new List<Faction> ();
        factions.ForEach (data => {
            Faction faction = new Faction ();
            faction.LoadSaveData (data);
            _factions.Add (faction);
        });
    }

    public static FactionManager GetInstance () { return _instance; }

}
