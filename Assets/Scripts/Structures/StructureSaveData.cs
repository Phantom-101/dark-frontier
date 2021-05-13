using System;
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

    public InterfacedStructureSaveData (StructureInstance serializable) {
        DataType = StructureSaveDataType.Default;
        Id = serializable.Id;
        Name = serializable.Name;
        Description = serializable.Description;
        ControllerId = serializable.Controller.Id;
        Hitpoints = serializable.Hitpoints;
        SignatureSize = JsonUtility.ToJson (serializable.SignatureSize);
        MaxHitpoints = JsonUtility.ToJson (serializable.MaxHitpoints);
    }

    public override ISerializable<IStructure> GetSerializable () { return new StructureInstance (this); }
}

