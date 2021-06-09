using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class FactionManager : SingletonBase<FactionManager> {
    private HashSet<Faction> _factions = new HashSet<Faction> ();
    [SerializeField] private List<FactionSO> _initialization = new List<FactionSO> ();

    private void Awake () {
        _initialization.ForEach (faction => {
            _factions.Add (faction.GetFaction ());
        });
        _initialization = new List<FactionSO> ();
    }

    public HashSet<Faction> GetFactions () { return _factions; }

    public void AddFaction (Faction faction) { _factions.Add (faction); }

    public void RemoveFaction (Faction faction) { _factions.Remove (faction); }

    public Faction GetFaction (string id) { return _factions.Where (e => e.GetId () == id).FirstOrDefault (); }

    public void SaveGame (DirectoryInfo directory) {
        List<FactionSaveData> saveData = new List<FactionSaveData> ();
        _factions.ToList ().ForEach (faction => { saveData.Add (faction.GetSaveData ()); });
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
        _factions = new HashSet<Faction> ();
        factions.ForEach (data => {
            Faction faction = new Faction ();
            faction.LoadSaveData (data);
            _factions.Add (faction);
        });
    }
}
