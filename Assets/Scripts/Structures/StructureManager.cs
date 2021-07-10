using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StructureManager : SingletonBase<StructureManager> {

    [SerializeField] private List<Structure> structures = new List<Structure> ();
    [SerializeField] private Queue<Structure> processQueue = new Queue<Structure> ();

    private void Update () {
        structures.RemoveAll (e => e == null);
        new List<Structure> (structures).ForEach (e => {
            e.Tick ();
            if (!processQueue.Contains (e)) processQueue.Enqueue (e);
        });
        Structure toProcess = null;
        while (toProcess == null && processQueue.Count > 0) toProcess = processQueue.Dequeue ();
        if (toProcess != null) toProcess.ExpensiveTick ();
    }

    private void FixedUpdate () {
        new List<Structure> (structures).ForEach (e => e.FixedTick ());
    }

    public List<Structure> GetStructures () => structures;

    public void AddStructure (Structure structure) => structures.Add (structure);

    public void RemoveStructure (Structure structure) => structures.Remove (structure);

    public Structure GetStructure (string id) => structures.Find (s => s.Id == id);

    public bool Detects (Structure a, Structure b) {
        if (a.Sector != b.Sector) return false;

        float sqrDis = (a.transform.localPosition - b.transform.localPosition).sqrMagnitude;
        float range = a.Stats.GetAppliedValue (StatNames.SensorStrength, 0) * b.Stats.GetAppliedValue (StatNames.Detectability, 0);
        float sqrRange = range * range;
        return sqrDis <= sqrRange;
    }

    public HashSet<Structure> GetDetected (Structure structure) {
        List<Structure> inSector = structure.Sector.InSector;
        HashSet<Structure> ret = new HashSet<Structure> ();
        foreach (Structure candidate in inSector)
            if (candidate != structure && Detects (structure, candidate))
                ret.Add (candidate);
        return ret;
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
        structures.Remove (structure);
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
        structures.Remove (structure);
        // Destroy structure game object
        Destroy (structure.gameObject);
    }

    public void SaveGame (DirectoryInfo directory) {
        List<StructureSaveData> saveData = new List<StructureSaveData> ();
        structures.ForEach (structure => { saveData.Add (structure.GetSaveData ()); });
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
        this.structures.ForEach (structure => { Destroy (structure.gameObject); });
        this.structures = new List<Structure> ();
        structures.ForEach (data => {
            StructureSO profile = ItemManager.Instance.GetItem (data.ProfileId) as StructureSO;
            GameObject structure = Instantiate (profile.Prefab, SectorManager.Instance.GetSector (data.SectorId).transform);
            structure.name = profile.Name;
            Structure comp = structure.GetComponent<Structure> ();
            comp.SetSaveData (data);
            this.structures.Add (comp);
            if (data.IsPlayer) PlayerController.Instance.Player = comp;
        });
    }
}
