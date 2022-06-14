using System;
using UnityEngine;

namespace DarkFrontier.Positioning.Navigation
{
    [Serializable]
    public class IntersectRay
    {
        public Vector3 origin;
        public Vector3 dir;
        public Vector3 invDir;
        public float length;
        
        public static IntersectRay FromEndpoints(Vector3 from, Vector3 to)
        {
            var delta = to - from;
            var mag = delta.magnitude;
            var norm = delta / mag;
            return new IntersectRay
            {
                origin = from,
                dir = norm,
                invDir = new Vector3(1/norm.x, 1/norm.y, 1/norm.z),
                length = mag
            };
        }

        public Vector3 PointAt(float distance)
        {
            return origin + dir * distance;
        }
    }
}