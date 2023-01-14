#nullable enable
using UnityEngine;

namespace DarkFrontier.Data.Collections.Graphs
{
    public class OctreeNode<T>
    {
        public readonly Bounds bounds;
        public OctreeNode<T>[]? children;
        public T? value;
        public readonly int depth;
        public readonly int maxDepth;

        public OctreeNode(Bounds bounds, IOctreeSplitter<T> splitter, IOctreeSetter<T> setter, int depth, int maxDepth)
        {
            this.bounds = bounds;
            this.depth = depth;
            this.maxDepth = maxDepth;
            Regenerate(splitter, setter);
        }

        public void Regenerate(IOctreeSplitter<T> splitter, IOctreeSetter<T> setter)
        {
            if (depth + 1 < maxDepth && splitter.ShouldSplit(this))
            {
                if (children == null)
                {
                    children = new OctreeNode<T>[8];
                    for (var z = 0; z < 2; z++)
                    {
                        for (var y = 0; y < 2; y++)
                        {
                            for (var x = 0; x < 2; x++)
                            {
                                children[x + (y << 1) + (z << 2)] = new OctreeNode<T>(new Bounds(bounds.center + Vector3.Scale(bounds.extents, new Vector3(x - 0.5f, y - 0.5f, z - 0.5f)), bounds.size / 2), splitter, setter, depth + 1, maxDepth);
                            }
                        }
                    }
                }
                else
                {
                    for (var i = 0; i < 8; i++)
                    {
                        children[i].Regenerate(splitter, setter);
                    }
                }
            }
            else
            {
                children = null;
                value = setter.GetValue(this);
            }
        }

        public void Draw()
        {
            if (children == null)
            {
                Gizmos.color = Color.Lerp(Color.green, Color.red, (float)depth / (maxDepth - 1));
                Gizmos.DrawWireCube(bounds.center, bounds.size);
            }
            else
            {
                for (var i = 0; i < 8; i++)
                {
                    children[i].Draw();
                }
            }
        }
    }
}