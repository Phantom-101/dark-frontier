using System;

[Serializable]
public class StructureSegmentSerialized : ISerialized<IStructureSegment> {
    public string Id;
    public ISerialized<IStat> MaxHitpoints;
    public float Hitpoints;
    public string StructureId;

    public StructureSegmentSerialized (StructureSegmentInstance serializable) {
        Id = serializable.Id;
        MaxHitpoints = (serializable.MaxHitpoints as ISerializable<IStat>).GetSerialized ();
        Hitpoints = serializable.Hitpoints;
        StructureId = serializable.Structure.Id;
    }

    public ISerializable<IStructureSegment> GetSerializable () { return new StructureSegmentInstance (this); }
}
