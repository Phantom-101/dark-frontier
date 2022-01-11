using System;
using System.Collections;
using System.IO;
using DarkFrontier.Channels;
using DarkFrontier.Factions;
using DarkFrontier.Files;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Items.Structures;
using DarkFrontier.Locations;
using DarkFrontier.SceneManagement;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.Serialization {
    public class SaveManager : MonoBehaviour {
        [SerializeField] private string _universe;

        [SerializeField] private VoidEventChannelSO _saveGameChannel;

        private static SaveManager _instance;

        private readonly Lazy<SectorManager> iSectorManager = new Lazy<SectorManager>(() => Singletons.Get<SectorManager>(), false);
        private readonly Lazy<FactionManager> iFactionManager = new Lazy<FactionManager>(() => Singletons.Get<FactionManager>(), false);
        private readonly Lazy<StructureManager> iStructureManager = new Lazy<StructureManager>(() => Singletons.Get<StructureManager>(), false);

        private void Awake () {
            if (_instance != null) {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
            _saveGameChannel.OnEventRaised += Save;
        }

        public string GetUniverse () { return _universe; }

        public void SetUniverse (string name) { _universe = name; }

        public static SaveManager GetInstance () { return _instance; }

        public void Save () {
            CreateSavesDirectoryIfNotExists ();
            string timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds ().ToString ();
            DirectoryInfo info = PathManager.GetSaveDirectory (_universe, timestamp);
            if (!info.Exists) info.Create ();
            iStructureManager.Value.SaveGame (info);
            iFactionManager.Value.SaveGame (info);
            iSectorManager.Value.SaveGame (info);
        }

        public void Load (string universeName) {
            CreateSavesDirectoryIfNotExists ();
            DirectoryInfo[] saves = PathManager.GetSaveDirectories (universeName);
            if (saves.Length == 0) return;
            DirectoryInfo latest = saves[0];
            for (int i = 1; i < saves.Length; i++)
                if (saves[i].Name.CompareTo (latest.Name) > 0)
                    latest = saves[i];
            StartCoroutine(Load(latest));
        }

        public void Load (string universeName, string saveName) {
            CreateSavesDirectoryIfNotExists ();
            DirectoryInfo info = PathManager.GetSaveDirectory (universeName, saveName);
            if (!info.Exists) return;
            StartCoroutine(Load(info));
        }

        private static void CreateSavesDirectoryIfNotExists () {
            DirectoryInfo info = PathManager.GetUniversesDirectory ();
            if (!info.Exists) info.Create ();
        }

        private IEnumerator Load(DirectoryInfo aDirectory) {
            yield return SceneUtils.Instance.LoadSceneAsync ("Game");
            iSectorManager.Value.LoadGame (aDirectory);
            iFactionManager.Value.LoadGame (aDirectory);
            iStructureManager.Value.LoadGame (aDirectory);
            _universe = aDirectory.Parent!.Name;
        }
    }
}