namespace DarkFrontier.Data.Composition
{
    public interface IHave<out T>
    {
        T Value { get; }
    }
}