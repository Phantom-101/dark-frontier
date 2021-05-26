using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StructureManager : MonoBehaviour {

    [SerializeField] private List<Structure> _structures = new List<Structure> ();
    [SerializeField] private Queue<Structure> _processQueue = new Queue<Structure> ();

    [SerializeField] private StructureCreatedEventChannelSO _invSpawnChannel;
    [SerializeField] private StructureCreatedEventChannelSO _siteSpawnChannel;
    [SerializeField] private StructureCreatedEventChannelSO _buySpawnChannel;

    [SerializeField] private StructureDestroyedEventChannelSO _shipDestroyedChannel;
    [SerializeField] private StructureDestroyedEventChannelSO _stationDestroyedChannel;
    [SerializeField] private StructureDestroyedEventChannelSO _cargoDestroyedChannel;

    private static StructureManager _instance;

    private void Awake () {

        _instance = this;
        Debug.Log ("StructureManager instance set");

        _invSpawnChannel.OnStructureCreated += OnInvSpawn;
        _siteSpawnChannel.OnStructureCreated += OnSiteSpawn;
        _buySpawnChannel.OnStructureCreated += OnBuySpawn;

        _shipDestroyedChannel.OnStructureDestroyed += OnShipDestroyed;
        _stationDestroyedChannel.OnStructureDestroyed += OnStationDestroyed;
        _cargoDestroyedChannel.OnStructureDestroyed += OnCargoDestroyed;

    }

    private void Update () {
        _structures.RemoveAll (e => e == null);
        _structures.ConvertAll (e => e).ForEach (e => {
            e.Tick ();
            if (!_processQueue.Contains (e))
                _processQueue.Enqueue (e);
        });
        Structure toProcess = null;
        while (toProcess == null && _processQueue.Count > 0) toProcess = _processQueue.Dequeue ();
        if (toProcess != null) {
            toProcess.Detected = null;
        }
    }

    private void FixedUpdate () {
        _structures.ConvertAll (e => e).ForEach (e => e.FixedTick ());
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

    public bool Detects (Structure a, Structure b) {

        if (a.Sector != b.Sector) return false;

        float sqrDis = (a.transform.localPosition - b.transform.localPosition).sqrMagnitude;
        float range = a.GetStatAppliedValue (StructureStatNames.SensorStrength) * b.GetStatAppliedValue (StructureStatNames.Detectability);
        float sqrRange = range * range;
        return sqrDis <= sqrRange;

    }

    public List<Structure> GetDetected (Structure structure) {

        List<Structure> inSector = structure.Sector.InSector;
        List<Structure> res = new List<Structure> ();
        foreach (Structure candidate in inSector)
            if (candidate != structure && Detects (structure, candidate))
                res.Add (candidate);
        return res;

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

    public void SaveGame (DirectoryInfo directory) {
        List<StructureSaveData> saveData = new List<StructureSaveData> ();
        _structures.ForEach (structure => { saveData.Add (structure.GetSaveData ()); });
        FileInfo file = PathManager.GetStructureFile (directory);
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
        FileInfo file = PathManager.GetStructureFile (directory);
        if (!file.Exists) return;
        List<StructureSaveData> structures = JsonConvert.DeserializeObject (
            File.ReadAllText (file.FullName),
            new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.All,
            }
        ) as List<StructureSaveData>;
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
