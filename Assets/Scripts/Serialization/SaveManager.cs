using System;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour {

    [SerializeField] private string _universe;

    [SerializeField] private VoidEventChannelSO _saveGameChannel;
    [SerializeField] private VoidEventChannelSO _loadGameChannel;

    private static SaveManager _instance;

    private void Awake () {

        _instance = this;

        _saveGameChannel.OnEventRaised += Save;
        _loadGameChannel.OnEventRaised += Load;

    }

    public string GetUniverse () { return _universe; }

    public void SetUniverse (string name) { _universe = name; }

    public static SaveManager GetInstance () { return _instance; }

    public void Save () {

        string timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds ().ToString ();

        StructureManager.GetInstance ().SaveGame (timestamp);
        SectorManager.GetInstance ().SaveGame (timestamp);
        FactionManager.GetInstance ().SaveGame (timestamp);

    }

    public void Load () {

        string[] saves = Directory.GetDirectories (Application.persistentDataPath + "/saves/" + _universe);

        if (saves.Length == 0) return;

        string latest = saves[0];
        for (int i = 1; i < saves.Length; i++)
            if (saves[i].CompareTo (latest) > 0)
                latest = saves[i];

        SectorManager.GetInstance ().LoadGame (SerializationManager.Load (latest + "/sectors.save"));
        FactionManager.GetInstance ().LoadGame (SerializationManager.Load (latest + "/factions.save"));
        StructureManager.GetInstance ().LoadGame (SerializationManager.Load (latest + "/structures.save"));

    }

}
