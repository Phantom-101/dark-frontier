using System;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Game.Essentials;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    [Serializable, JsonObject(MemberSerialization.OptIn, IsReference = true)]
    public class ConstantValue<T> : IId, IValue<T>
    {
        [field: SerializeReference]
        [JsonProperty("id")]
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        
        public T Value { get; private init; }
        
        public ConstantValue() => Singletons.Get<IdRegistry>().Register(this);

        public ConstantValue(T value) : this() => Value = value;
        
        public static implicit operator T(ConstantValue<T> constantValue) => constantValue.Value;
        
        public static implicit operator ConstantValue<T>(T value) => new(value);
    }
}