using System;
using UnityEngine;

namespace DarkFrontier.Locations {
    [Serializable]
    public class Location {
        public Transform UTransform => iTransform;
        public Vector3 UPosition => iHaveTransform ? iTransform.position : iPosition;

        [SerializeField] private Transform iTransform;
        [SerializeField] private Vector3 iPosition;

        private readonly bool iHaveTransform;

        public Location(Transform aTransform) {
            iTransform = aTransform;
            iHaveTransform = true;
        }

        public Location (Vector3 aPosition) => iPosition = aPosition;
    }
}