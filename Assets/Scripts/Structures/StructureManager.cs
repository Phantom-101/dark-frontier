using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class StructureManager : SingletonBase<StructureManager> {

    [SerializeField] private List<Structure> _structures = new List<Structure> ();
    [SerializeField] private Queue<Structure> _processQueue = new Queue<Structure> ();

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
        float range = a.GetStatAppliedValue (StatType.SensorStrength, 0) * b.GetStatAppliedValue (StatType.Detectability, 0);
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

    public void OnInvSpawn (StructureSO profile, Faction owner, Sector sector, Location location) {

        GameObject structure = Instantiate (profile.Prefab, sector.transform);
        structure.name = profile.Name;
        structure.transform.localPosition = location.GetPosition ();
        structure.GetComponent<Structure> ().Faction = owner;
        structure.GetComponent<Structure> ().Sector = sector;

    }

    public void OnSiteSpawn (StructureSO profile, Faction owner, Sector sector, Location location) {

        GameObject structure = Instantiate (profile.Prefab, sector.transform);
        structure.name = profile.Name;
        structure.transform.localPosition = location.GetPosition ();
        structure.GetComponent<Structure> ().Faction = owner;
        structure.GetComponent<Structure> ().Sector = sector;
        // Spawn structure of profile at random location around location.GetPosition

    }

    public void OnBuySpawn (StructureSO profile, Faction owner, Sector sector, Location location) {

        GameObject structure = Instantiate (profile.Prefab, location.GetTransform ());
        structure.name = profile.Name;
        structure.transform.localPosition = Vector3.zero;
        structure.GetComponent<Structure> ().Faction = owner;
        structure.GetComponent<Structure> ().Sector = sector;

    }

    public void OnStructureDestroyed (Structure destroyedStructure) {
        // Destroy docked structures
        destroyedStructure.GetDocked ().ForEach (e => OnStructureDestroyed (e));
        // Remove structure from list
        _structures.Remove (destroyedStructure);
        // Spawn destruction effect
        if (destroyedStructure.Profile.DestructionEffect != null) {
            GameObject effect = Instantiate (destroyedStructure.Profile.DestructionEffect, destroyedStructure.transform.parent);
            effect.transform.localPosition = destroyedStructure.transform.localPosition;
            effect.transform.localScale = Vector3.one * destroyedStructure.Profile.ApparentSize;
        }
        // TODO Drop stuff according to StructureSO.DropPercentage
        // Destroy structure game object
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
            StructureSO profile = ItemManager.Instance.GetItem (data.ProfileId) as StructureSO;
            GameObject structure = Instantiate (profile.Prefab, SectorManager.Instance.GetSector (data.SectorId).transform);
            structure.name = profile.Name;
            Structure comp = structure.GetComponent<Structure> ();
            comp.SetSaveData (data);
            _structures.Add (comp);
            if (data.IsPlayer) PlayerController.Instance.Player = comp;
        });
    }
}
