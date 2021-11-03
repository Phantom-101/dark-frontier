using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Identification;
using DarkFrontier.Foundation.Serialization;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Structures;
using System;
using DarkFrontier.Foundation.Extensions;
using UnityEngine;

namespace DarkFrontier.Locations {
    public class Sector : ComponentBehavior, ISavableState<Sector.Serializable> {
        public Id UId => iId;
        [SerializeField] private Id iId = new Id ();
        public StructureRegistry UPopulation { get => iPopulation; }
        [SerializeField] private StructureRegistry iPopulation = new StructureRegistry ();

        private SectorManager iSectorManager;

        public override void Initialize () {
            iSectorManager = Singletons.Get<SectorManager> ();
        }

        public override void Enable () {
            iSectorManager.Registry.Set (this);
        }

        public Serializable ToSerializable () {
            return new Serializable {
                Name = gameObject.name,
                Position = transform.localPosition.ToArray (),
                Rotation = transform.localRotation.ToArray (),
                Id = iId,
            };
        }

        public void FromSerializable (Serializable aSerializable) {
            gameObject.name = aSerializable.Name;
            var lTransform = transform;
            lTransform.localPosition = new Vector3 (aSerializable.Position[0], aSerializable.Position[1], aSerializable.Position[2]);
            lTransform.localRotation = new Quaternion (aSerializable.Rotation[0], aSerializable.Rotation[1], aSerializable.Rotation[2], aSerializable.Rotation[3]);
            iId = new Id (aSerializable.Id);
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