using DarkFrontier.Attributes;
using DarkFrontier.Utils;
using UnityEngine;


namespace DarkFrontier.Positioning.Navigation
{
    public class NavigationIntersection : MonoBehaviour
    {
        public Transform? from;
        public Transform? to;
        public BoxCollider? box;

        [field: SerializeField, ReadOnly]
        public bool Intersects { get; private set; }

        private void Update()
        {
            if(from != null && to != null && box != null)
            {
                Intersects = GeometryUtils.Intersects(Aabb.FromCenterExtents(box.center, box.size), IntersectRay.FromEndpoints(from.position, to.position));
            }
        }
    }
}
