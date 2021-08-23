using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Zenject;

namespace DarkFrontier.Locations {
    public class SectorManager : MonoBehaviour {
        public SectorRegistry Registry { get => registry; }
        [SerializeReference] private SectorRegistry registry;

        [Inject]
        public void Construct (SectorRegistry registry) {
            this.registry = registry;
        }

        public void SaveGame (DirectoryInfo directory) {
            List<SectorSaveData> saveData = new List<SectorSaveData> ();
            registry.Sectors.ToList ().ForEach (sector => { saveData.Add (sector.GetSaveData ()); });
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
            registry.Sectors.ForEach (sector => { Destroy (sector.gameObject); });
            registry.Clear ();
            sectors.ForEach (data => {
                GameObject sector = new GameObject ();
                Sector comp = sector.AddComponent<Sector> ();
                comp.SetSaveData (data);
                registry.Set (comp);
            });
        }

    }
}