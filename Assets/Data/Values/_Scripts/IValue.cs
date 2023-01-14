namespace DarkFrontier.Data.Values
{
    public interface IValue<out T>
    {
        T Value { get; }
    }
}