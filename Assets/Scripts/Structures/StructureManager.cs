using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureManager : MonoBehaviour {

    [SerializeField] private List<Structure> _structures = new List<Structure> ();

    [SerializeField] private SpawnStructureEventChannelSO _invSpawnChannel;
    [SerializeField] private SpawnStructureEventChannelSO _siteSpawnChannel;
    [SerializeField] private SpawnStructureEventChannelSO _buySpawnChannel;

    [SerializeField] private StructureDestroyedEventChannelSO _shipDestroyedChannel;
    [SerializeField] private StructureDestroyedEventChannelSO _stationDestroyedChannel;
    [SerializeField] private StructureDestroyedEventChannelSO _cargoDestroyedChannel;

    protected virtual void Awake () {

        _structures = FindObjectsOfType<Structure> ().ToList ();

        _invSpawnChannel.OnSpawnStructure += OnInvSpawn;
        _siteSpawnChannel.OnSpawnStructure += OnSiteSpawn;
        _buySpawnChannel.OnSpawnStructure += OnBuySpawn;

        _shipDestroyedChannel.OnStructureDestroyed += OnShipDestroyed;
        _stationDestroyedChannel.OnStructureDestroyed += OnStationDestroyed;
        _cargoDestroyedChannel.OnStructureDestroyed += OnCargoDestroyed;

    }

    private void Update () {

        foreach (Structure structure in _structures) structure.Tick ();

    }

    private void FixedUpdate () {

        foreach (Structure structure in _structures) structure.FixedTick ();

    }

    public virtual List<Structure> GetStructures () { return _structures; }

    private void OnInvSpawn (StructureSO profile, Structure spawner, Location location) {

        // Spawn structure of profile at location.GetPosition
        // Set faction to spawner's faction

    }

    private void OnSiteSpawn (StructureSO profile, Structure spawner, Location location) {

        // Spawn structure of profile at random location around location.GetPosition
        // Set faction to spawner's faction

    }

    private void OnBuySpawn (StructureSO profile, Structure spawner, Location location) {

        // Spawn structure of profile at location.GetPosition
        // Set structure parent to location.GetTransform
        // Set faction to spawner's faction

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

}
