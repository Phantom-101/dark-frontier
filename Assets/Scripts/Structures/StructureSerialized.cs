using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class StructureSerialized : ISerialized<IStructure> {
    public string Id;
    public string Name;
    public string Description;
    public string ControllerId;
    public float Hitpoints;
    public ISerialized<IStat> SignatureSize;
    public ISerialized<IStat> MaxHitpoints;
    public ISerialized<IStat> ScannerStrength;
    public List<ISerialized<IStructureSegment>> Segments;
    public Dictionary<string, float> ActiveLocks;

    public StructureSerialized (StructureInstance serializable) {
        Id = serializable.Id;
        Name = serializable.Name;
        Description = serializable.Description;
        ControllerId = serializable.Controller.Id;
        Hitpoints = serializable.Hitpoints;
        SignatureSize = (serializable.SignatureSize as ISerializable<IStat>).GetSerialized ();
        MaxHitpoints = (serializable.MaxHitpoints as ISerializable<IStat>).GetSerialized ();
        ScannerStrength = (serializable.ScannerStrength as ISerializable<IStat>).GetSerialized ();
        Segments = serializable.Segments.FindAll (e => e is ISerializable<IStructureSegment>).ConvertAll (e => (e as ISerializable<IStructureSegment>).GetSerialized ());
        ActiveLocks = serializable.ActiveLocks.ToDictionary (k => k.Key.Id, k => k.Value);
    }

    public ISerializable<IStructure> GetSerializable () { return new StructureInstance (this); }
}

