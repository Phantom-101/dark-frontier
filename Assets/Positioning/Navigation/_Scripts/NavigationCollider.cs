using UnityEngine;


namespace DarkFrontier.Positioning.Navigation
{
    public class NavigationCollider : MonoBehaviour
    {
        public Vector3 extents = Vector3.one;

        public Aabb Aabb { get; private set; } = null!;
        
        public void Regenerate()
        {
            Aabb = Aabb.FromCenterExtents(transform.position, extents);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, extents * 2);
        }
    }
}
