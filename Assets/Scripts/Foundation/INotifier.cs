using System;

namespace DarkFrontier.Foundation {
    public interface INotifier {
        event EventHandler Notifier;
    }
}