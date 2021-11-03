using System;
using DarkFrontier.Items.Prototypes;
using DarkFrontier.Stats;
using RotaryHeart.Lib.SerializableDictionary;

namespace DarkFrontier.DataStructures {
    [Serializable] public class StringToStructureStatDictionary : SerializableDictionaryBase<string, Stat> { }
    [Serializable] public class StringToStatDictionary : SerializableDictionaryBase<string, Stat> { }
    [Serializable] public class StringToFloatDictionary : SerializableDictionaryBase<string, float> { }
    [Serializable] public class ItemSOToIntDictionary : SerializableDictionaryBase<ItemPrototype, int> { }
    [Serializable] public class StructurePrototypeToFloatDictionary : SerializableDictionaryBase<StructurePrototype, float> { }
    [Serializable] public class StringToStatModifierDictionary : SerializableDictionaryBase<string, StatModifier> { }
}