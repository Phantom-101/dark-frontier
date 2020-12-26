using System.Collections.Generic;
using UnityEngine;

public class Sector : MonoBehaviour {

    [SerializeField] private List<Structure> _inSector = new List<Structure> ();

    [SerializeField] private StructureDestroyedEventChannelSO _shipDestroyedChannel;
    [SerializeField] private StructureDestroyedEventChannelSO _stationDestroyedChannel;
    [SerializeField] private StructureDestroyedEventChannelSO _cargoDestroyedChannel;

    private void Awake () {
        _shipDestroyedChannel.OnStructureDestroyed += Exited;
        _stationDestroyedChannel.OnStructureDestroyed += Exited;
        _cargoDestroyedChannel.OnStructureDestroyed += Exited;
    }

    public void Entered (Structure structure) {

        _inSector.Add (structure);

    }

    public void Exited (Structure structure) {

        if (structure.GetSector () != this) return;

        _inSector.Remove (structure);

    }

}
