using System;
using System.Collections.Generic;
using UnityEngine;

public class Sector : MonoBehaviour {
    public string Id { get => id; }
    [SerializeField] private string id;
    [SerializeField] private List<Structure> population = new List<Structure> ();

    public List<Structure> Population {
        get {
            population.RemoveAll (s => s == null);
            return population;
        }
    }

    private void Start () {
        SectorManager.Instance.AddSector (this);
    }

    public void Entered (Structure structure) {
        population.Add (structure);
    }

    public void Exited (Structure structure) {
        population.Remove (structure);
    }

    public SectorSaveData GetSaveData () {

        SectorSaveData data = new SectorSaveData {

            Name = gameObject.name,
            Position = new float[] { transform.localPosition.x, transform.localPosition.y, transform.localPosition.z },
            Rotation = new float[] { transform.localRotation.x, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w },
            Id = id

        };
        return data;

    }

    public void SetSaveData (SectorSaveData saveData) {

        gameObject.name = saveData.Name;
        transform.localPosition = new Vector3 (saveData.Position[0], saveData.Position[1], saveData.Position[2]);
        transform.localRotation = new Quaternion (saveData.Rotation[0], saveData.Rotation[1], saveData.Rotation[2], saveData.Rotation[3]);
        id = saveData.Id;

    }

}


[Serializable]
public class SectorSaveData {
    public string Name;
    public float[] Position;
    public float[] Rotation;
    public string Id;
}