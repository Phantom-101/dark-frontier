using System;
using UnityEngine;

[Serializable]
public class StructureSegmentSaveData : SerializedBase<IStructureSegment> {
    public string Id;
    public string MaxHitpoints;
    public float Hitpoints;
    public string StructureId;

    public StructureSegmentSaveData (StructureSegmentInstance serializable) {
        DataType = StructureSegmentSaveDataType.Default;
        Id = serializable.Id;
        MaxHitpoints = JsonUtility.ToJson (serializable.MaxHitpoints);
        Hitpoints = serializable.Hitpoints;
        StructureId = serializable.Structure.Id;
    }

    public override ISerializable<IStructureSegment> GetSerializable () { return new StructureSegmentInstance (this); }
}
