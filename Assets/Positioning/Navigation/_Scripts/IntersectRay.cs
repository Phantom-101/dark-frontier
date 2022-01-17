using System;
using UnityEngine;

namespace DarkFrontier.Positioning.Navigation
{
    [Serializable]
    public record IntersectRay (Vector3 Origin, Vector3 Destination, Vector3 Dir, Vector3 InvDir, float Length)
    {
        public static IntersectRay FromEndpoints(Vector3 from, Vector3 to)
        {
            var delta = to - from;
            var mag = delta.magnitude;
            var norm = delta / mag;
            return new IntersectRay(from, to, norm, new Vector3(1 / norm.x, 1 / norm.y, 1 / norm.z), mag);
        }

        public Vector3 PointAt(float distance)
        {
            return Origin + Dir * distance;
        }
    }
}