using DarkFrontier.Foundation;
using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Structures;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DarkFrontier.Locations {
    public class Sector : ComponentBehavior {
        public Id Id { get => id; }
        [SerializeField] private Id id = new Id ();
        public List<Structure> Population { get => population; }
        [SerializeField] private List<Structure> population = new List<Structure> ();

        private SectorManager sectorManager;

        [Inject]
        public void Construct (SectorManager sectorManager) {
            this.sectorManager = sectorManager;
        }

        protected override void MultiInitialize () {
            sectorManager.Registry.Add (this);
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
            id = new Id (saveData.Id);

        }

    }


    [Serializable]
    public class SectorSaveData {
        public string Name;
        public float[] Position;
        public float[] Rotation;
        public string Id;
    }
}