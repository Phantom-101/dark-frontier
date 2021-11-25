using System;
using UnityEngine;

namespace DarkFrontier.Locations {
    [Serializable]
    public class Location {
        public Transform UTransform => iTransform;
        public Vector3 UPosition {
            get {
                if (iTransform == null) {
                    return iPosition;
                }
                return iPosition = iTransform.position;
            }
        }

        [SerializeField] private Transform iTransform;
        [SerializeField] private Vector3 iPosition;

        public Location(Transform aTransform) => iTransform = aTransform;
        public Location (Vector3 aPosition) => iPosition = aPosition;
    }
}