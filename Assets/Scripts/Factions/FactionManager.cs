using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Extensions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Zenject;

namespace DarkFrontier.Factions {
    public class FactionManager : ComponentBehavior {
        public FactionRegistry Registry { get => registry; }
        [SerializeReference] private FactionRegistry registry;

        [Inject]
        public void Construct (FactionRegistry registry) {
            this.registry = registry;
        }

        public void SaveGame (DirectoryInfo directory) {
            List<FactionSaveData> saveData = new List<FactionSaveData> ();
            registry.Factions.ToList ().ForEach (faction => { saveData.Add (faction.Save ()); });
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
            registry.Clear ();
            factions.ForEach (data => registry.Factions.Add (data.Load ()));
        }
    }
}