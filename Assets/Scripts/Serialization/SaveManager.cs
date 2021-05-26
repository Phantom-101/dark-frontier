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
            if (SectorManager.GetInstance () == null) return;
            if (FactionManager.GetInstance () == null) return;
            if (StructureManager.GetInstance () == null) return;
            SectorManager.GetInstance ().LoadGame (_job.Directory);
            FactionManager.GetInstance ().LoadGame (_job.Directory);
            StructureManager.GetInstance ().LoadGame (_job.Directory);
            _universe = _job.Directory.Parent.Name;
            _job = null;
        }
    }

    public string GetUniverse () { return _universe; }

    public void SetUniverse (string name) { _universe = name; }

    public static SaveManager GetInstance () { return _instance; }

    public void Save () {
        Debug.Log ("saving");

        CreateSavesDirectoryIfNotExists ();
        string timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds ().ToString ();
        DirectoryInfo info = PathManager.GetSaveDirectory (_universe, timestamp);
        if (!info.Exists) info.Create ();
        StructureManager.GetInstance ().SaveGame (info);
        SectorManager.GetInstance ().SaveGame (info);
        FactionManager.GetInstance ().SaveGame (info);
    }

    public void Load (string universeName) {
        CreateSavesDirectoryIfNotExists ();
        DirectoryInfo[] saves = PathManager.GetSaveDirectories (universeName);
        if (saves.Length == 0) return;
        DirectoryInfo latest = saves[0];
        for (int i = 1; i < saves.Length; i++)
            if (saves[i].Name.CompareTo (latest.Name) > 0)
                latest = saves[i];
        _job = new SaveLoadJob {
            Directory = latest,
        };
    }

    public void Load (string universeName, string saveName) {
        CreateSavesDirectoryIfNotExists ();
        DirectoryInfo info = PathManager.GetSaveDirectory (universeName, saveName);
        if (!info.Exists) return;
        _job = new SaveLoadJob {
            Directory = info,
        };
    }

    void CreateSavesDirectoryIfNotExists () {
        DirectoryInfo info = PathManager.GetUniversesDirectory ();
        if (!info.Exists) info.Create ();
    }
}

[Serializable]
public class SaveLoadJob {
    public DirectoryInfo Directory;
}