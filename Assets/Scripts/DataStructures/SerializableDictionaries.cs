using RotaryHeart.Lib.SerializableDictionary;
using System;

[Serializable] public class StringToStructureStatDictionary : SerializableDictionaryBase<string, StructureStat> { }
[Serializable] public class StructureStatTypeToStructureStatDictionary : SerializableDictionaryBase<StructureStatType, StructureStat> { }
[Serializable] public class StringToFloatDictionary : SerializableDictionaryBase<string, float> { }
[Serializable] public class ItemSOToIntDictionary : SerializableDictionaryBase<ItemSO, int> { }
[Serializable] public class StructureSOToFloatDictionary : SerializableDictionaryBase<StructureSO, float> { }