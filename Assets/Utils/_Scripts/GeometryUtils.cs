using DarkFrontier.Foundation.Mathematics;
using DarkFrontier.Positioning.Navigation;

namespace DarkFrontier.Utils
{
    public static class GeometryUtils
    {
        public static bool Intersects(Aabb box, IntersectRay ray)
        {
            var x1 = (box.Min.x - ray.Origin.x) * ray.InvDir.x;
            var x2 = (box.Max.x - ray.Origin.x) * ray.InvDir.x;
            var y1 = (box.Min.y - ray.Origin.y) * ray.InvDir.y;
            var y2 = (box.Max.y - ray.Origin.y) * ray.InvDir.y;
            var z1 = (box.Min.z - ray.Origin.z) * ray.InvDir.z;
            var z2 = (box.Max.z - ray.Origin.z) * ray.InvDir.z;

            var a = Math.Max(Math.Min(x1, x2), Math.Min(y1, y2), Math.Min(z1, z2));
            var b = Math.Min(Math.Max(x1, x2), Math.Max(y1, y2), Math.Max(z1, z2));
            
            return b >= Math.Max(a, 0) && a < ray.Length;
        }
        
        public static bool Intersects(Aabb box, IntersectRay ray, out float distance)
        {
            var x1 = (box.Min.x - ray.Origin.x) * ray.InvDir.x;
            var x2 = (box.Max.x - ray.Origin.x) * ray.InvDir.x;
            var y1 = (box.Min.y - ray.Origin.y) * ray.InvDir.y;
            var y2 = (box.Max.y - ray.Origin.y) * ray.InvDir.y;
            var z1 = (box.Min.z - ray.Origin.z) * ray.InvDir.z;
            var z2 = (box.Max.z - ray.Origin.z) * ray.InvDir.z;

            distance = Math.Max(Math.Min(x1, x2), Math.Min(y1, y2), Math.Min(z1, z2));
            var b = Math.Min(Math.Max(x1, x2), Math.Max(y1, y2), Math.Max(z1, z2));

            return b >= Math.Max(distance, 0) && distance < ray.Length;
        }
    }
}