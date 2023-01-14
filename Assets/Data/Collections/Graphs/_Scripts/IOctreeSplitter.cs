namespace DarkFrontier.Data.Collections.Graphs
{
    public interface IOctreeSplitter<T>
    {
        bool ShouldSplit(OctreeNode<T> node);
    }
}