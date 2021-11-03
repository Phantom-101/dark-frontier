using UnityEngine;

namespace DarkFrontier.Foundation.Extensions {
    public static class Vector3Extensions {
        public static float[] ToArray(this Vector3 aVector) {
            return new[] { aVector.x, aVector.y, aVector.z };
        }
    }
}
