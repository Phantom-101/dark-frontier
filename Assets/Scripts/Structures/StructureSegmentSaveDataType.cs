using UnityEngine;

public class StructureSegmentSaveDataType {
    public static string Default = "default";

    public static ISerializable<IStructureSegment> Parse (string json) {
        SerializedBase<IStructureSegment> data = JsonUtility.FromJson<SerializedBase<IStructureSegment>> (json);
        if (data.GetDataType () == Default) return JsonUtility.FromJson<StructureSegmentSaveData> (json).GetSerializable ();
        return null;
    }
}