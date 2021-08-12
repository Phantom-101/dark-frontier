namespace DarkFrontier.Foundation {
    public interface INotifier<T> : INotifier {
        T Value { get; set; }
    }
}
