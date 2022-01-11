using System;
using DarkFrontier.Items;
using DarkFrontier.Structures;
using DarkFrontier.Stats;
using RotaryHeart.Lib.SerializableDictionary;

namespace DarkFrontier.DataStructures {
    [Serializable] public class StringToStructureStatDictionary : SerializableDictionaryBase<string, Stat> { }
    [Serializable] public class StringToStatDictionary : SerializableDictionaryBase<string, Stat> { }
    [Serializable] public class StringToFloatDictionary : SerializableDictionaryBase<string, float> { }
    [Serializable] public class ItemPrototypeToIntDictionary : SerializableDictionaryBase<ItemPrototype, int> { }
    [Serializable] public class ItemPrototypeStateToIntDictionary : SerializableDictionaryBase<ItemPrototype.State, int> { }
    [Serializable] public class StructurePrototypeToFloatDictionary : SerializableDictionaryBase<StructurePrototype, float> { }
    [Serializable] public class StringToStatModifierDictionary : SerializableDictionaryBase<string, StatModifier> { }
}