using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class FactionManager : SingletonBase<FactionManager> {
    private HashSet<Faction> _factions = new HashSet<Faction> ();
    public HashSet<Faction> Factions { get => _factions; private set => _factions = value; }

    public void AddFaction (Faction faction) { Factions.Add (faction); }
    public void RemoveFaction (Faction faction) { Factions.Remove (faction); }
    public Faction GetFaction (string id) { return Factions.Where (e => e.Id == id).FirstOrDefault (); }

    public void SaveGame (DirectoryInfo directory) {
        List<FactionSaveData> saveData = new List<FactionSaveData> ();
        Factions.ToList ().ForEach (faction => { saveData.Add (faction.GetSaveData ()); });
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
        Factions = new HashSet<Faction> ();
        factions.ForEach (data => {
            Faction faction = ScriptableObject.CreateInstance<Faction> ();
            faction.LoadSaveData (data);
            Factions.Add (faction);
        });
    }
}
