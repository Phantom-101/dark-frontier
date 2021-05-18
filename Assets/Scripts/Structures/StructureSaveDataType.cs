using UnityEngine;

public class StructureSaveDataType {
    public static string Default = "default";

    public static ISerializable<IStructure> Parse (string json) {
        SerializedBase<IStructure> data = JsonUtility.FromJson<SerializedBase<IStructure>> (json);
        if (data.GetDataType () == Default) return JsonUtility.FromJson<InterfacedStructureSaveData> (json).GetSerializable ();
        return null;
    }
}