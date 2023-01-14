#nullable enable
using UnityEngine;

namespace DarkFrontier.Data.Collections.Graphs
{
    public class OctreeComponent : MonoBehaviour
    {
        public Bounds bounds;
        public int maxDepth = 8;
        
        private Octree<bool>? _octree;
        private readonly IOctreeSplitter<bool> _splitter = new CollisionSplitter();
        private readonly IOctreeSetter<bool> _setter = new CollisionSetter();

        private void Start()
        {
            _octree = new Octree<bool>(bounds, _splitter, _setter, maxDepth);
        }

        private void Update()
        {
            _octree!.Regenerate(_splitter, _setter);
        }

        private void OnDrawGizmosSelected()
        {
            _octree?.Draw();
        }
    }
}