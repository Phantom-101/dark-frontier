namespace DarkFrontier.Foundation.Events {
    public interface IValueNotifier<TValue> : INotifier {
        TValue Value { get; set; }
    }
}
