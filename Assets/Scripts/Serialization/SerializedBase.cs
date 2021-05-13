public class SerializedBase<T> : ISerialized<T> {
    public string DataType;

    public string GetDataType () { return DataType; }

    public virtual ISerializable<T> GetSerializable () { return null; }
}

