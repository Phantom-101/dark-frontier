using System;
using System.Collections.Generic;
using UnityEngine;

public class Sector : MonoBehaviour {

    [SerializeField] private string _id;

    [SerializeField] private List<Structure> _inSector = new List<Structure> ();

    private void Start () {

        SectorManager.GetInstance ().AddSector (this);

    }

    public void SetupChannels (StructureDestroyedEventChannelSO ship, StructureDestroyedEventChannelSO station, StructureDestroyedEventChannelSO cargo) {

        ship.OnStructureDestroyed += Exited;
        station.OnStructureDestroyed += Exited;
        cargo.OnStructureDestroyed += Exited;

    }

    public string GetId () { return _id; }

    public void SetId (string id) { _id = id; }

    public void Entered (Structure structure) {

        _inSector.Add (structure);

    }

    public void Exited (Structure structure) {

        if (structure.GetSector () != this) return;

        _inSector.Remove (structure);

    }

    public SectorSaveData GetSaveData () {

        SectorSaveData data = new SectorSaveData {

            Name = gameObject.name,
            Position = transform.localPosition,
            Rotation = transform.localRotation,
            Id = _id

        };
        return data;

    }

    public void SetSaveData (SectorSaveData saveData) {

        gameObject.name = saveData.Name;
        transform.localPosition = saveData.Position;
        transform.localRotation = saveData.Rotation;
        _id = saveData.Id;

    }

}


[Serializable]
public class SectorSaveData {

    public string Name;
    public Vector3 Position;
    public Quaternion Rotation;
    public string Id;

}