using UnityEngine;

namespace DarkFrontier.Foundation.Extensions {
    public static class Vector3Extensions {
        public static float[] ToArray(this Vector3 aVector) {
            return new[] {aVector.x, aVector.y, aVector.z};
        }

        public static Vector3? ToVector3(this float[] aArray) {
            if (aArray.Length != 3) {
                return null;
            }
            
            return new Vector3(aArray[0], aArray[1], aArray[2]);
        }
    }
}