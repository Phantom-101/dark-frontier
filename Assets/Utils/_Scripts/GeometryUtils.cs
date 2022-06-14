using DarkFrontier.Foundation.Mathematics;
using DarkFrontier.Positioning.Navigation;

namespace DarkFrontier.Utils
{
    public static class GeometryUtils
    {
        public static bool Intersects(Aabb box, IntersectRay ray)
        {
            var x1 = (box.min.x - ray.origin.x) * ray.invDir.x;
            var x2 = (box.max.x - ray.origin.x) * ray.invDir.x;
            var y1 = (box.min.y - ray.origin.y) * ray.invDir.y;
            var y2 = (box.max.y - ray.origin.y) * ray.invDir.y;
            var z1 = (box.min.z - ray.origin.z) * ray.invDir.z;
            var z2 = (box.max.z - ray.origin.z) * ray.invDir.z;

            var a = Math.Max(Math.Min(x1, x2), Math.Min(y1, y2), Math.Min(z1, z2));
            var b = Math.Min(Math.Max(x1, x2), Math.Max(y1, y2), Math.Max(z1, z2));
            
            return b >= Math.Max(a, 0) && a < ray.length;
        }
        
        public static bool Intersects(Aabb box, IntersectRay ray, out float distance)
        {
            var x1 = (box.min.x - ray.origin.x) * ray.invDir.x;
            var x2 = (box.max.x - ray.origin.x) * ray.invDir.x;
            var y1 = (box.min.y - ray.origin.y) * ray.invDir.y;
            var y2 = (box.max.y - ray.origin.y) * ray.invDir.y;
            var z1 = (box.min.z - ray.origin.z) * ray.invDir.z;
            var z2 = (box.max.z - ray.origin.z) * ray.invDir.z;

            distance = Math.Max(Math.Min(x1, x2), Math.Min(y1, y2), Math.Min(z1, z2));
            var b = Math.Min(Math.Max(x1, x2), Math.Max(y1, y2), Math.Max(z1, z2));

            return b >= Math.Max(distance, 0) && distance < ray.length;
        }
    }
}