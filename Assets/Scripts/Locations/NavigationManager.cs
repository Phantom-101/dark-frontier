using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.Locations {
    public class NavigationManager {
        public float Distance (Location aLocationA, Location aLocationB, DistanceType aType) {
            Vector3 lDif = aLocationA.UPosition - aLocationB.UPosition;
            switch (aType) {
                case DistanceType.Euclidean:
                    return lDif.magnitude;
                case DistanceType.SquaredEuclidean:
                    return lDif.sqrMagnitude;
                case DistanceType.Manhattan:
                    return Mathf.Abs (lDif.x) + Mathf.Abs (lDif.y) + Mathf.Abs (lDif.z);
                case DistanceType.Chebyshev:
                    return Mathf.Max (Mathf.Abs (lDif.x), Mathf.Abs (lDif.y), Mathf.Abs (lDif.z));
                default:
                    return -1;
            }
        }
    }

    public enum DistanceType {
        Euclidean,
        SquaredEuclidean,
        Manhattan,
        Chebyshev,
    }
}