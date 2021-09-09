namespace DarkFrontier.Foundation.Serialization {
    public interface ISavableState<T> {
        T ToSerializable ();
        void FromSerializable (T serializable);
    }
}
