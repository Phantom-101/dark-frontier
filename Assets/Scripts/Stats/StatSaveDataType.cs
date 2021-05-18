using UnityEngine;

public class StatSaveDataType {
    public static string Default = "default";

    public static ISerializable<IStat> Parse (string json) {
        SerializedBase<IStat> data = JsonUtility.FromJson<SerializedBase<IStat>> (json);
        if (data.GetDataType () == Default) return JsonUtility.FromJson<StatSaveData> (json).GetSerializable ();
        return null;
    }
}