﻿using DarkFrontier.Foundation.Extensions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SectorManager : SingletonBase<SectorManager> {

    [SerializeField] private List<Sector> sectors = new List<Sector> ();

    public void AddSector (Sector sector) { sectors.AddUnique (sector); }

    public void RemoveSector (Sector sector) { sectors.RemoveAll (sector); }

    public Sector GetSector (string id) {
        Sector found = null;
        sectors.ToList ().ForEach (sector => {
            if (sector.Id == id) found = sector;
        });
        return found;
    }

    public void SaveGame (DirectoryInfo directory) {
        List<SectorSaveData> saveData = new List<SectorSaveData> ();
        sectors.ToList ().ForEach (sector => { saveData.Add (sector.GetSaveData ()); });
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
        this.sectors.ToList ().ForEach (sector => { Destroy (sector.gameObject); });
        this.sectors = new List<Sector> ();
        sectors.ForEach (data => {
            GameObject sector = new GameObject ();
            Sector comp = sector.AddComponent<Sector> ();
            comp.SetSaveData (data);
            this.sectors.Add (comp);
        });
    }

}
