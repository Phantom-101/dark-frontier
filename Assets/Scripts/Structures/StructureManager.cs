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
        float range = a.Stats.GetAppliedValue (StatNames.SensorStrength, 0) * b.Stats.GetAppliedValue (StatNames.Detectability, 0);
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

    public Structure SpawnStructure (StructureSO profile, Faction owner, Sector sector, Location location) {
        GameObject go = Instantiate (profile.Prefab, sector.transform);
        go.name = profile.Name;
        go.transform.localPosition = location.GetPosition ();
        Structure structure = go.GetComponent<Structure> ();
        structure.Faction = owner;
        structure.Sector = sector;
        return structure;
    }

    public void DestroyStructure (Structure structure) {
        // Destroy docked structures
        structure.GetDocked ().ForEach (e => DestroyStructure (e));
        // Remove structure from list
        _structures.Remove (structure);
        // Spawn destruction effect
        if (structure.Profile.DestructionEffect != null) {
            GameObject effect = Instantiate (structure.Profile.DestructionEffect, structure.transform.parent);
            effect.transform.localPosition = structure.transform.localPosition;
            effect.transform.localScale = Vector3.one * structure.Profile.ApparentSize;
        }
        // TODO Drop stuff according to StructureSO.DropPercentage
        // Destroy structure game object
        Destroy (structure.gameObject);
    }

    public void DisposeStructure (Structure structure) {
        // Destroy docked structures
        structure.GetDocked ().ForEach (e => DisposeStructure (e));
        // Remove structure from list
        _structures.Remove (structure);
        // Destroy structure game object
        Destroy (structure.gameObject);
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
