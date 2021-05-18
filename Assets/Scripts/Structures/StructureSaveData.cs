using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class InterfacedStructureSaveData : SerializedBase<IStructure> {
    public string Id;
    public string Name;
    public string Description;
    public string ControllerId;
    public float Hitpoints;
    public string SignatureSize;
    public string MaxHitpoints;
    public string ScannerStrength;
    public List<string> Segments;
    public List<string> ActiveLockIds;
    public List<float> ActiveLockProgresses;

    public InterfacedStructureSaveData (StructureInstance serializable) {
        DataType = StructureSaveDataType.Default;
        Id = serializable.Id;
        Name = serializable.Name;
        Description = serializable.Description;
        ControllerId = serializable.Controller.Id;
        Hitpoints = serializable.Hitpoints;
        SignatureSize = JsonUtility.ToJson (serializable.SignatureSize);
        MaxHitpoints = JsonUtility.ToJson (serializable.MaxHitpoints);
        ScannerStrength = JsonUtility.ToJson (serializable.ScannerStrength);
        Segments = serializable.Segments.FindAll ((e) => e is ISerializable<IStructureSegment>).ConvertAll ((e) => JsonUtility.ToJson ((e as ISerializable<IStructureSegment>).GetSerialized ()));
        ActiveLockIds = serializable.ActiveLocks.Keys.ToList ().ConvertAll ((e) => e.Id);
        ActiveLockProgresses = serializable.ActiveLocks.Values.ToList ();
    }

    public override ISerializable<IStructure> GetSerializable () { return new StructureInstance (this); }
}

