using System.Collections.Generic;
using UnityEngine;

public class StructureManager : MonoBehaviour {

    [SerializeField] private List<Structure> _structures = new List<Structure> ();

    [SerializeField] private StructureDestroyedEventChannelSO _shipDestroyedChannel;
    [SerializeField] private StructureDestroyedEventChannelSO _stationDestroyedChannel;
    [SerializeField] private StructureDestroyedEventChannelSO _cargoDestroyedChannel;

    protected virtual void Awake () {

        _shipDestroyedChannel.OnStructureDestroyed += OnShipDestroyed;
        _stationDestroyedChannel.OnStructureDestroyed += OnStationDestroyed;
        _cargoDestroyedChannel.OnStructureDestroyed += OnCargoDestroyed;

    }

    public virtual List<Structure> GetStructures () { return _structures; }

    protected virtual void OnShipDestroyed (Structure destroyedStructure, StructureSO destroyedType) {

        _structures.Remove (destroyedStructure);

        // Drop stuff according to StructureSO.DropPercentage

        Destroy (destroyedStructure.gameObject);

    }

    protected virtual void OnStationDestroyed (Structure destroyedStructure, StructureSO destroyedType) {

        _structures.Remove (destroyedStructure);

        // Drop stuff according to StructureSO.DropPercentage

        // Destroy docked ships

        Destroy (destroyedStructure.gameObject);

    }

    protected virtual void OnCargoDestroyed (Structure destroyedStructure, StructureSO destroyedType) {

        _structures.Remove (destroyedStructure);

        Destroy (destroyedStructure.gameObject);

    }

}
