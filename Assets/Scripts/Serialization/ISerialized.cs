public interface ISerialized<T> {
    string GetDataType ();

    ISerializable<T> GetSerializable ();
}

