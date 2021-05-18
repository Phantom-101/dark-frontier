using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class StatSaveData : SerializedBase<IStat> {
    public float BaseValue;
    public List<string> Modifiers;
    public string Id;
    public string Name;
    public string Description;

    public StatSaveData (StatInstance serializable) {
        DataType = StatSaveDataType.Default;
        BaseValue = serializable.BaseValue;
        Modifiers = serializable.Modifiers.Values.ToList ().FindAll ((e) => e is ISerializable<IStatModifier>).ConvertAll ((e) => JsonUtility.ToJson ((e as ISerializable<IStatModifier>).GetSerialized ()));
        Id = serializable.Id;
        Name = serializable.Name;
        Description = serializable.Description;
    }

    public override ISerializable<IStat> GetSerializable () { return new StatInstance (this); }
}

