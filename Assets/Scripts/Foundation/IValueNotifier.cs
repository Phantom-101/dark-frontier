namespace DarkFrontier.Foundation {
    public interface IValueNotifier<T> : INotifier {
        T Value { get; set; }
    }
}
