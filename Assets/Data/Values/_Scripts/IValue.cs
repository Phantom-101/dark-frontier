using DarkFrontier.Game.Essentials;

namespace DarkFrontier.Data.Values
{
    public interface IValue<out T> : IId
    {
        T Value { get; }
    }
}