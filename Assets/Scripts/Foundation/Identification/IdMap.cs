using RotaryHeart.Lib.SerializableDictionary;
using System;

namespace DarkFrontier.Foundation.Identification {
    [Serializable] public class IdMap<T> : SerializableDictionaryBase<string, T> { }
}
