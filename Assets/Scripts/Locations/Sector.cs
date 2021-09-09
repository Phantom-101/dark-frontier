using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Identification;
using DarkFrontier.Foundation.Serialization;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Structures;
using System;
using UnityEngine;

namespace DarkFrontier.Locations {
    public class Sector : ComponentBehavior, ISavableState<Sector.Serializable> {
        public Id Id { get => id; }
        [SerializeField] private Id id = new Id ();
        public StructureRegistry Population { get => population; }
        [SerializeField] private StructureRegistry population = new StructureRegistry ();

        private SectorManager sectorManager;

        protected override void MultiInitialize () {
            sectorManager.Registry.Set (this);
        }

        public override void GetServices () {
            sectorManager = Singletons.Get<SectorManager> ();
        }

        public Serializable ToSerializable () {
            return new Serializable {
                Name = gameObject.name,
                Position = new float[] { transform.localPosition.x, transform.localPosition.y, transform.localPosition.z },
                Rotation = new float[] { transform.localRotation.x, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w },
                Id = id,
            };
        }

        public void FromSerializable (Serializable serializable) {
            gameObject.name = serializable.Name;
            transform.localPosition = new Vector3 (serializable.Position[0], serializable.Position[1], serializable.Position[2]);
            transform.localRotation = new Quaternion (serializable.Rotation[0], serializable.Rotation[1], serializable.Rotation[2], serializable.Rotation[3]);
            id = new Id (serializable.Id);
        }

        [Serializable]
        public class Serializable {
            public string Name;
            public float[] Position;
            public float[] Rotation;
            public string Id;
        }
    }
}