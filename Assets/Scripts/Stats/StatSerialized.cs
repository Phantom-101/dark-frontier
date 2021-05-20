using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class StatSerialized : ISerialized<IStat> {
    public float BaseValue;
    public List<ISerialized<IStatModifier>> Modifiers;
    public string Id;
    public string Name;
    public string Description;

    public StatSerialized (StatInstance serializable) {
        BaseValue = serializable.BaseValue;
        Modifiers = serializable.Modifiers.Values.ToList ().FindAll (e => e is ISerializable<IStatModifier>).ConvertAll (e => (e as ISerializable<IStatModifier>).GetSerialized ());
        Id = serializable.Id;
        Name = serializable.Name;
        Description = serializable.Description;
    }

    public ISerializable<IStat> GetSerializable () { return new StatInstance (this); }
}

