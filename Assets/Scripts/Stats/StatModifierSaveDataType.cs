using UnityEngine;

public class StatModifierSaveDataType {
    public static string Default = "default";

    public static ISerializable<IStatModifier> Parse (string json) {
        SerializedBase<IStatModifier> data = JsonUtility.FromJson<SerializedBase<IStatModifier>> (json);
        if (data.GetDataType () == Default) return JsonUtility.FromJson<StatModifierSaveData> (json).GetSerializable ();
        return null;
    }
}