using System.Collections.Generic;
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

    private void Awake () {

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

    public List<Structure> GetStructures () { return _structures; }

    public void AddStructure (Structure structure) { if (!_structures.Contains (structure)) _structures.Add (structure); }

    public void RemoveStructure (Structure structure) { _structures.Remove (structure); }

    public Structure GetStructure (string id) {

        Structure found = null;
        _structures.ForEach (structure => {

            if (structure.Id == id) found = structure;

        });
        return found;

    }

    private void OnInvSpawn (StructureSO profile, Faction owner, Sector sector, Location location) {

        GameObject structure = Instantiate (profile.Prefab, sector.transform);
        structure.name = profile.Name;
        structure.transform.localPosition = location.GetPosition ();
        structure.GetComponent<Structure> ().Faction = owner;
        structure.GetComponent<Structure> ().Sector = sector;

    }

    private void OnSiteSpawn (StructureSO profile, Faction owner, Sector sector, Location location) {

        GameObject structure = Instantiate (profile.Prefab, sector.transform);
        structure.name = profile.Name;
        structure.transform.localPosition = location.GetPosition ();
        structure.GetComponent<Structure> ().Faction = owner;
        structure.GetComponent<Structure> ().Sector = sector;
        // Spawn structure of profile at random location around location.GetPosition

    }

    private void OnBuySpawn (StructureSO profile, Faction owner, Sector sector, Location location) {

        GameObject structure = Instantiate (profile.Prefab, location.GetTransform ());
        structure.name = profile.Name;
        structure.transform.localPosition = Vector3.zero;
        structure.GetComponent<Structure> ().Faction = owner;
        structure.GetComponent<Structure> ().Sector = sector;

    }

    private void OnShipDestroyed (Structure destroyedStructure) {

        _structures.Remove (destroyedStructure);

        if (destroyedStructure.Profile.DestructionEffect != null) {

            GameObject effect = Instantiate (destroyedStructure.Profile.DestructionEffect, destroyedStructure.transform.parent);
            effect.transform.localPosition = destroyedStructure.transform.localPosition;
            effect.transform.localScale = Vector3.one * destroyedStructure.Profile.ApparentSize;
            Destroy (effect, 3);

        }

        // Drop stuff according to StructureSO.DropPercentage

        LeanTween.value (destroyedStructure.gameObject, 0, 1, 5).setOnUpdateParam (destroyedStructure.gameObject).setOnUpdateObject ((float value, object obj) => {

            GameObject go = obj as GameObject;
            go.GetComponentInChildren<MeshRenderer> ().material.SetFloat ("_DissolveAmount", value);

        });

        Destroy (destroyedStructure.GetComponent<ConstantForce> ());

        Destroy (destroyedStructure);
        Destroy (destroyedStructure.gameObject, 6);

    }

    private void OnStationDestroyed (Structure destroyedStructure) {

        _structures.Remove (destroyedStructure);

        if (destroyedStructure.Profile.DestructionEffect != null) {

            GameObject effect = Instantiate (destroyedStructure.Profile.DestructionEffect, destroyedStructure.transform.parent);
            effect.transform.localPosition = destroyedStructure.transform.localPosition;
            effect.transform.localScale = Vector3.one * destroyedStructure.Profile.ApparentSize;
            Destroy (effect, 3);

        }

        // Drop stuff according to StructureSO.DropPercentage

        // Destroy docked ships

        LeanTween.value (destroyedStructure.gameObject, 0, 1, 10).setOnUpdateParam (destroyedStructure.gameObject).setOnUpdateObject ((float value, object obj) => {

            GameObject go = obj as GameObject;
            go.GetComponentInChildren<MeshRenderer> ().material.SetFloat ("_DissolveAmount", value);

        });

        Destroy (destroyedStructure.GetComponent<ConstantForce> ());

        Destroy (destroyedStructure);
        Destroy (destroyedStructure.gameObject, 11);

    }

    private void OnCargoDestroyed (Structure destroyedStructure) {

        _structures.Remove (destroyedStructure);

        if (destroyedStructure.Profile.DestructionEffect != null) {

            GameObject effect = Instantiate (destroyedStructure.Profile.DestructionEffect, destroyedStructure.transform.parent);
            effect.transform.localPosition = destroyedStructure.transform.localPosition;
            effect.transform.localScale = Vector3.one * destroyedStructure.Profile.ApparentSize;
            Destroy (effect, 3);

        }

        Destroy (destroyedStructure.GetComponent<ConstantForce> ());
        Destroy (destroyedStructure.GetComponent<Rigidbody> ());

        Destroy (destroyedStructure);
        Destroy (destroyedStructure.gameObject);

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
            if (data.IsPlayer) PlayerController.GetInstance ().SetPlayer (comp);

        });

    }

    public static StructureManager GetInstance () { return _instance; }

}
