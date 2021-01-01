using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureManager : MonoBehaviour {

    [SerializeField] private List<Structure> _structures = new List<Structure> ();

    [SerializeField] private StructureCreatedEventChannelSO _invSpawnChannel;
    [SerializeField] private StructureCreatedEventChannelSO _siteSpawnChannel;
    [SerializeField] private StructureCreatedEventChannelSO _buySpawnChannel;

    [SerializeField] private StructureDestroyedEventChannelSO _shipDestroyedChannel;
    [SerializeField] private StructureDestroyedEventChannelSO _stationDestroyedChannel;
    [SerializeField] private StructureDestroyedEventChannelSO _cargoDestroyedChannel;

    private static StructureManager _instance;

    protected virtual void Awake () {

        _invSpawnChannel.OnStructureCreated += OnInvSpawn;
        _siteSpawnChannel.OnStructureCreated += OnSiteSpawn;
        _buySpawnChannel.OnStructureCreated += OnBuySpawn;

        _shipDestroyedChannel.OnStructureDestroyed += OnShipDestroyed;
        _stationDestroyedChannel.OnStructureDestroyed += OnStationDestroyed;
        _cargoDestroyedChannel.OnStructureDestroyed += OnCargoDestroyed;

        _instance = this;

    }

    private void Update () {

        foreach (Structure structure in _structures.ToArray ()) structure.Tick ();

    }

    private void FixedUpdate () {

        foreach (Structure structure in _structures.ToArray ()) structure.FixedTick ();

    }

    public virtual List<Structure> GetStructures () { return _structures; }

    private void OnInvSpawn (StructureSO profile, Faction owner, Sector sector, Location location) {

        GameObject structure = Instantiate (profile.Prefab, sector.transform);
        structure.name = profile.Name;
        structure.transform.localPosition = location.GetPosition ();
        structure.GetComponent<Structure> ().SetFaction (owner);
        structure.GetComponent<Structure> ().SetSector (sector);

    }

    private void OnSiteSpawn (StructureSO profile, Faction owner, Sector sector, Location location) {

        GameObject structure = Instantiate (profile.Prefab, sector.transform);
        structure.name = profile.Name;
        structure.transform.localPosition = location.GetPosition ();
        structure.GetComponent<Structure> ().SetFaction (owner);
        structure.GetComponent<Structure> ().SetSector (sector);
        // Spawn structure of profile at random location around location.GetPosition

    }

    private void OnBuySpawn (StructureSO profile, Faction owner, Sector sector, Location location) {

        GameObject structure = Instantiate (profile.Prefab, location.GetTransform ());
        structure.name = profile.Name;
        structure.transform.localPosition = Vector3.zero;
        structure.GetComponent<Structure> ().SetFaction (owner);
        structure.GetComponent<Structure> ().SetSector (sector);

    }

    private void OnShipDestroyed (Structure destroyedStructure) {

        _structures.Remove (destroyedStructure);

        // Drop stuff according to StructureSO.DropPercentage

        Destroy (destroyedStructure.gameObject);

    }

    private void OnStationDestroyed (Structure destroyedStructure) {

        _structures.Remove (destroyedStructure);

        // Drop stuff according to StructureSO.DropPercentage

        // Destroy docked ships

        Destroy (destroyedStructure.gameObject);

    }

    private void OnCargoDestroyed (Structure destroyedStructure) {

        _structures.Remove (destroyedStructure);

        Destroy (destroyedStructure.gameObject);

    }

    public void AddStructure (Structure structure) { if (!_structures.Contains (structure)) _structures.Add (structure); }

    public void RemoveStructure (Structure structure) { _structures.Remove (structure); }

    public Structure GetStructure (string id) {

        Structure found = null;
        _structures.ForEach (structure => {

            if (structure.GetId () == id) found = structure;

        });
        return found;

    }

    public void SaveGame (string timestamp) {

        string saveName = SaveManager.GetInstance ().GetUniverse ();

        List<StructureSaveData> saveData = new List<StructureSaveData> ();
        _structures.ForEach (structure => { saveData.Add (structure.GetSaveData ()); });

        SerializationManager.Save (Application.persistentDataPath + "/saves/" + saveName + "/" + timestamp, "structures.save", saveData);

    }

    public void LoadGame (object saveData) {

        List<StructureSaveData> structures = saveData as List<StructureSaveData>;

        _structures.ForEach (structure => { Destroy (structure.gameObject); });
        _structures = new List<Structure> ();

        structures.ForEach (data => {

            StructureSO profile = ItemManager.GetInstance ().GetItem (data.ProfileId) as StructureSO;
            GameObject structure = Instantiate (profile.Prefab, SectorManager.GetInstance ().GetSector (data.SectorId).transform);
            structure.name = profile.Name;
            Structure comp = structure.GetComponent<Structure> ();
            comp.SetSaveData (data);
            _structures.Add (comp);

        });

    }

    public static StructureManager GetInstance () { return _instance; }

}
