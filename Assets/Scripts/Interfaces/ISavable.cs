public interface ISavable<T> {
    ISaveData<T> GetSaveData ();

    void LoadSaveData (ISaveData<T> data);
}
