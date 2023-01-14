using UnityEngine;

namespace DarkFrontier.Data.Collections.Graphs
{
    public class CollisionSetter : IOctreeSetter<bool>
    {
        private readonly Collider[] _results = new Collider[1];

        public bool GetValue(OctreeNode<bool> node)
        {
            return Physics.OverlapBoxNonAlloc(node.bounds.center, node.bounds.extents, _results) > 0;
        }
    }
}