namespace DarkFrontier.Data.Values
{
    public interface IObservable<T> : IValue<T>, INotifier<T>
    {
    }
}
