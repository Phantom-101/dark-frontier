using System;
using UnityEngine;

namespace DarkFrontier.Positioning.Navigation
{
    [Serializable]
    public record Aabb (Vector3 Center, Vector3 Extents, Vector3 Min, Vector3 Max)
    {
        public static Aabb FromCenterExtents(Vector3 center, Vector3 extents)
        {
            return new Aabb(center, extents, center - extents, center + extents);
        }

        public float RoughSize()
        {
            return Navigation.Chebyshev(Extents) * 2;
        }

        public bool Contains(Vector3 point)
        {
            return Min.x <= point.x && Max.x >= point.x &&
                   Min.y <= point.y && Max.y >= point.y &&
                   Min.z <= point.z && Max.z >= point.z;
        }
    }
}