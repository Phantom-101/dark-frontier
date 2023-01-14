namespace DarkFrontier.Data.Collections.Graphs
{
    public interface IOctreeSetter<T>
    {
        T GetValue(OctreeNode<T> node);
    }
}