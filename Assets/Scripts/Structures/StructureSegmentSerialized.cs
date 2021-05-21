using System;
using System.Collections.Generic;

[Serializable]
public class StructureSegmentSerialized : ISerialized<IStructureSegment> {
    public string Id;
    public ISerialized<IStat> MaxHitpoints;
    public float Hitpoints;
    public string StructureId;
    public List<ISerialized<IEquipmentSlot>> EquipmentSlots;

    public StructureSegmentSerialized (StructureSegmentInstance serializable) {
        Id = serializable.Id;
        MaxHitpoints = (serializable.MaxHitpoints as ISerializable<IStat>).GetSerialized ();
        Hitpoints = serializable.Hitpoints;
        StructureId = serializable.Structure.Id;
        EquipmentSlots = serializable.EquipmentSlots.FindAll (e => e is ISerializable<IEquipmentSlot>).ConvertAll (e => (e as ISerializable<IEquipmentSlot>).GetSerialized ());
    }

    public ISerializable<IStructureSegment> GetSerializable () { return new StructureSegmentInstance (this); }
}
