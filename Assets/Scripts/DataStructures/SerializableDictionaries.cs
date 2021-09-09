using RotaryHeart.Lib.SerializableDictionary;
using System;

[Serializable] public class StringToStructureStatDictionary : SerializableDictionaryBase<string, Stat> { }
[Serializable] public class StringToStatDictionary : SerializableDictionaryBase<string, Stat> { }
[Serializable] public class StringToFloatDictionary : SerializableDictionaryBase<string, float> { }
[Serializable] public class ItemSOToIntDictionary : SerializableDictionaryBase<ItemPrototype, int> { }
[Serializable] public class StructureSOToFloatDictionary : SerializableDictionaryBase<StructureSO, float> { }
[Serializable] public class StringToStatModifierDictionary : SerializableDictionaryBase<string, StatModifier> { }