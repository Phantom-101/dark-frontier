using System;
using System.Collections.Generic;
using UnityEngine;

public class Sector : MonoBehaviour {

    [SerializeField] private string _id;
    [SerializeField] private List<Structure> _inSector = new List<Structure> ();

    public List<Structure> InSector { get => _inSector; }

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

        if (structure.Sector != this) return;

        _inSector.Remove (structure);

    }

    public SectorSaveData GetSaveData () {

        SectorSaveData data = new SectorSaveData {

            Name = gameObject.name,
            Position = new float[] { transform.localPosition.x, transform.localPosition.y, transform.localPosition.z },
            Rotation = new float[] { transform.localRotation.x, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w },
            Id = _id

        };
        return data;

    }

    public void SetSaveData (SectorSaveData saveData) {

        gameObject.name = saveData.Name;
        transform.localPosition = new Vector3 (saveData.Position[0], saveData.Position[1], saveData.Position[2]);
        transform.localRotation = new Quaternion (saveData.Rotation[0], saveData.Rotation[1], saveData.Rotation[2], saveData.Rotation[3]);
        _id = saveData.Id;

    }

}


[Serializable]
public class SectorSaveData {

    public string Name;
    public float[] Position;
    public float[] Rotation;
    public string Id;

}