using RotaryHeart.Lib.SerializableDictionary;
using System;

[Serializable]
public class StringToStructureStatDictionary : SerializableDictionaryBase<string, StructureStat> { };

[Serializable]
public class StringToFloatDictionary : SerializableDictionaryBase<string, float> { };

[Serializable]
public class ItemSOToIntDictionary : SerializableDictionaryBase<ItemSO, int> { };
