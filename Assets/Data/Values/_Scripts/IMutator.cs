using DarkFrontier.Game.Essentials;

namespace DarkFrontier.Data.Values
{
    public interface IMutator<T> : IId
    {
        int Order { get; }

        T Mutate(T value);
    }
}