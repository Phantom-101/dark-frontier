using System;
using UnityEngine;

namespace DarkFrontier.Positioning.Navigation
{
    [Serializable]
    public class Aabb
    {
        public Vector3 min;
        public Vector3 max;

        public static Aabb FromMinMax(Vector3 min, Vector3 max)
        {
            return new Aabb
            {
                min = min,
                max = max
            };
        }

        public static Aabb FromCenterExtents(Vector3 center, Vector3 extents)
        {
            return new Aabb
            {
                min = center - extents,
                max = center + extents
            };
        }

        public float RoughSize()
        {
            return Navigation.Chebyshev(max - min);
        }

        public bool Contains(Vector3 point)
        {
            return min.x <= point.x && max.x >= point.x &&
                   min.y <= point.y && max.y >= point.y &&
                   min.z <= point.z && max.z >= point.z;
        }
    }
}