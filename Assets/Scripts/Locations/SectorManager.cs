using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace DarkFrontier.Locations {
    public class SectorManager : MonoBehaviour {
        public SectorRegistry Registry { get => registry; }
        [SerializeReference] private SectorRegistry registry = new SectorRegistry ();

        public void SaveGame (DirectoryInfo directory) {
            List<Sector.Serializable> saveData = new List<Sector.Serializable> ();
            registry.Sectors.ToList ().ForEach (sector => saveData.Add (sector.ToSerializable ()));
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
            List<Sector.Serializable> sectors = JsonConvert.DeserializeObject<List<Sector.Serializable>> (
                File.ReadAllText (file.FullName),
                new JsonSerializerSettings {
                    TypeNameHandling = TypeNameHandling.All,
                }
            );
            registry.Sectors.ForEach (sector => { Destroy (sector.gameObject); });
            registry.Clear ();
            sectors.ForEach (data => {
                GameObject sector = new GameObject ();
                Sector comp = sector.AddComponent<Sector> ();
                comp.FromSerializable (data);
                registry.Set (comp);
            });
        }

    }
}