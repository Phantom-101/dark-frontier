using System;

namespace DarkFrontier.Data.Values
{
    public interface INotifier : INotifier<EventArgs>
    {
    }

    public interface INotifier<T>
    {
        event EventHandler<T> OnNotify;
    }
}