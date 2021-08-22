using RotaryHeart.Lib.SerializableDictionary;
using System;

namespace DarkFrontier.Foundation {
    [Serializable]
    public class IdMap<T> : SerializableDictionaryBase<string, T> { }
}
