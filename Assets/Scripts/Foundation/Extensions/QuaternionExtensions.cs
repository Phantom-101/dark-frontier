using UnityEngine;

namespace DarkFrontier.Foundation.Extensions {
    public static class QuaternionExtensions {
        public static float[] ToArray(this Quaternion aQuaternion) {
            return new[] { aQuaternion.x, aQuaternion.y, aQuaternion.z, aQuaternion.w };
        }

        public static Quaternion? ToQuaternion(this float[] aArray) {
            if (aArray.Length != 4) {
                return null;
            }

            return new Quaternion(aArray[0], aArray[1], aArray[2], aArray[3]);
        }
    }
}
