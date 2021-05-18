using System;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour {

    [SerializeField] private string _universe;
    private static SaveLoadJob _job;

    [SerializeField] private VoidEventChannelSO _saveGameChannel;

    private static SaveManager _instance;

    private void Awake () {

        _instance = this;

        _saveGameChannel.OnEventRaised += Save;

    }

    private void Update () {

        if (_job != null) {

            Debug.Log ("doing job");

            if (SectorManager.GetInstance () == null) return;
            if (FactionManager.GetInstance () == null) return;
            if (StructureManager.GetInstance () == null) return;

            Debug.Log ("job started");

            string path = Application.persistentDataPath + "/saves/" + _job.Universe + "/" + _job.Save;

            Debug.Log ($"path: {path}");

            SectorManager.GetInstance ().LoadGame (SerializationManager.Load (path + "/sectors.save"));
            FactionManager.GetInstance ().LoadGame (SerializationManager.Load (path + "/factions.save"));
            StructureManager.GetInstance ().LoadGame (SerializationManager.Load (path + "/structures.save"));

            _universe = _job.Universe;

            _job = null;

        }

    }

    public string GetUniverse () { return _universe; }

    public void SetUniverse (string name) { _universe = name; }

    public static SaveManager GetInstance () { return _instance; }

    public void Save () {

        CreateSavesDirectoryIfNotExists ();

        string timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds ().ToString ();

        StructureManager.GetInstance ().SaveGame (timestamp);
        SectorManager.GetInstance ().SaveGame (timestamp);
        FactionManager.GetInstance ().SaveGame (timestamp);

    }

    public void Load (string universeName) {

        CreateSavesDirectoryIfNotExists ();

        string[] saves = Directory.GetDirectories (Application.persistentDataPath + "/saves/" + universeName);

        if (saves.Length == 0) return;

        string latest = saves[0];
        for (int i = 1; i < saves.Length; i++)
            if (saves[i].CompareTo (latest) > 0)
                latest = saves[i];

        _job = new SaveLoadJob {

            Universe = universeName,
            Save = latest.Remove (0, Application.persistentDataPath.Length + universeName.Length + 2)

        };

        SectorManager.GetInstance ().LoadGame (SerializationManager.Load (latest + "/sectors.save"));
        FactionManager.GetInstance ().LoadGame (SerializationManager.Load (latest + "/factions.save"));
        StructureManager.GetInstance ().LoadGame (SerializationManager.Load (latest + "/structures.save"));

        _universe = universeName;

    }

    public void Load (string universeName, string saveName) {

        CreateSavesDirectoryIfNotExists ();

        if (!Directory.Exists (Application.persistentDataPath + "/saves/" + universeName + "/" + saveName)) return;

        _job = new SaveLoadJob {

            Universe = universeName,
            Save = saveName

        };

    }

    public string[] GetAllUniverses () {

        CreateSavesDirectoryIfNotExists ();

        DirectoryInfo[] infos = new DirectoryInfo (Application.persistentDataPath + "/saves/").GetDirectories ();
        string[] names = new string[infos.Length];
        for (int i = 0; i < infos.Length; i++) names[i] = infos[i].Name;
        return names;

    }

    public string[] GetAllSaves (string universeName) {

        CreateSavesDirectoryIfNotExists ();

        if (!Directory.Exists (Application.persistentDataPath + "/saves/" + universeName)) return new string[0];

        DirectoryInfo[] infos = new DirectoryInfo (Application.persistentDataPath + "/saves/" + universeName).GetDirectories ();
        string[] names = new string[infos.Length];
        for (int i = 0; i < infos.Length; i++) names[i] = infos[i].Name;
        return names;

    }

    void CreateSavesDirectoryIfNotExists () {

        if (!Directory.Exists (Application.persistentDataPath + "/saves/")) Directory.CreateDirectory (Application.persistentDataPath + "/saves/");

    }

}

[Serializable]
public class SaveLoadJob {

    public string Universe;
    public string Save;

}