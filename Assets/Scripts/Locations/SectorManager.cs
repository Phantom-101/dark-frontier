using System.Collections.Generic;
using UnityEngine;

public class SectorManager : MonoBehaviour {

    [SerializeField] private List<Sector> _sectors;

    [SerializeField] private StructureDestroyedEventChannelSO _shipDestroyedChannel;
    [SerializeField] private StructureDestroyedEventChannelSO _stationDestroyedChannel;
    [SerializeField] private StructureDestroyedEventChannelSO _cargoDestroyedChannel;

    private static SectorManager _instance;

    private void Awake () {

        _instance = this;

    }

    public void AddSector (Sector sector) { if (!_sectors.Contains (sector)) _sectors.Add (sector); }

    public void RemoveSector (Sector sector) { _sectors.Remove (sector); }

    public Sector GetSector (string id) {

        Sector found = null;
        _sectors.ForEach (sector => {

            if (sector.GetId () == id) found = sector;

        });
        return found;

    }

    public void SaveGame (string timestamp) {

        string saveName = SaveManager.GetInstance ().GetUniverse ();

        List<SectorSaveData> saveData = new List<SectorSaveData> ();
        _sectors.ForEach (sector => { saveData.Add (sector.GetSaveData ()); });

        SerializationManager.Save (Application.persistentDataPath + "/saves/" + saveName + "/" + timestamp, "sectors.save", JsonHelper.ToJson (saveData));

    }

    public void LoadGame (string saveData) {

        List<SectorSaveData> sectors = JsonHelper.ListFromJson<SectorSaveData> (saveData);

        _sectors.ForEach (sector => { Destroy (sector.gameObject); });
        _sectors = new List<Sector> ();

        sectors.ForEach (data => {

            GameObject sector = new GameObject ();
            Sector comp = sector.AddComponent<Sector> ();
            comp.SetSaveData (data);
            comp.SetupChannels (_shipDestroyedChannel, _stationDestroyedChannel, _cargoDestroyedChannel);
            _sectors.Add (comp);

        });

    }

    public static SectorManager GetInstance () { return _instance; }

}
