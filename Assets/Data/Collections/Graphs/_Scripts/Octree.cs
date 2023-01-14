using UnityEngine;

namespace DarkFrontier.Data.Collections.Graphs
{
    public class Octree<T>
    {
        public OctreeNode<T> root;

        public Octree(Bounds bounds, IOctreeSplitter<T> splitter, IOctreeSetter<T> setter, int maxDepth)
        {
            root = new OctreeNode<T>(bounds, splitter, setter, 0, maxDepth);
        }

        public void Regenerate(IOctreeSplitter<T> splitter, IOctreeSetter<T> setter)
        {
            root.Regenerate(splitter, setter);
        }

        public void Draw()
        {
            root.Draw();
        }
    }
}
