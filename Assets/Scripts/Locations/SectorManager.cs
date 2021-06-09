using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SectorManager : SingletonBase<SectorManager> {

    private HashSet<Sector> _sectors = new HashSet<Sector> ();

    public void AddSector (Sector sector) { _sectors.Add (sector); }

    public void RemoveSector (Sector sector) { _sectors.Remove (sector); }

    public Sector GetSector (string id) {
        Sector found = null;
        _sectors.ToList ().ForEach (sector => {
            if (sector.GetId () == id) found = sector;
        });
        return found;
    }

    public void SaveGame (DirectoryInfo directory) {
        List<SectorSaveData> saveData = new List<SectorSaveData> ();
        _sectors.ToList ().ForEach (sector => { saveData.Add (sector.GetSaveData ()); });
        FileInfo file = PathManager.GetSectorFile (directory);
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
        FileInfo file = PathManager.GetSectorFile (directory);
        if (!file.Exists) return;
        List<SectorSaveData> sectors = JsonConvert.DeserializeObject (
            File.ReadAllText (file.FullName),
            new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.All,
            }
        ) as List<SectorSaveData>;
        _sectors.ToList ().ForEach (sector => { Destroy (sector.gameObject); });
        _sectors = new HashSet<Sector> ();
        sectors.ForEach (data => {
            GameObject sector = new GameObject ();
            Sector comp = sector.AddComponent<Sector> ();
            comp.SetSaveData (data);
            _sectors.Add (comp);
        });
    }

}
