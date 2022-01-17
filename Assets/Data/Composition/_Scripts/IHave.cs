namespace DarkFrontier.Data.Composition
{
    public interface IHave<T>
    {
        void Get(ref T value);
    }
}