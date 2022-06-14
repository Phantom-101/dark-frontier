using UnityEngine;

namespace DarkFrontier.Positioning.Navigation
{
    public class NavigationCollider : MonoBehaviour
    {
        public Vector3 extents = Vector3.one;

        public Aabb Aabb { get; private set; } = new();
        
        public void Regenerate()
        {
            var position = transform.position;
            Aabb.min = position - extents;
            Aabb.max = position + extents;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, extents * 2);
        }
    }
}
