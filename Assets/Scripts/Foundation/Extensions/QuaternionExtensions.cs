using UnityEngine;

namespace DarkFrontier.Foundation.Extensions {
    public static class QuaternionExtensions {
        public static float[] ToArray(this Quaternion aQuaternion) {
            return new[] { aQuaternion.x, aQuaternion.y, aQuaternion.z, aQuaternion.w };
        }
    }
}
