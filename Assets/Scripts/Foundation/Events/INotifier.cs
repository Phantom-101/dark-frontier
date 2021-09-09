using System;

namespace DarkFrontier.Foundation.Events {
    public interface INotifier : INotifier<EventArgs> { }

    public interface INotifier<TEventArgs> {
        event EventHandler<TEventArgs> Notifier;
    }
}