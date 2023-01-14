namespace DarkFrontier.Data.Values
{
    public interface IMutator<T>
    {
        int Order { get; }

        T Mutate(T value);
    }
}