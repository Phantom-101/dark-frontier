using System;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Game.Essentials;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    [Serializable, JsonObject(MemberSerialization.OptIn, IsReference = true)]
    public class ValueMutator<T> : IMutator<T>
    {
        [field: SerializeField] [JsonProperty("id")]
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        
        [field: SerializeField] [JsonProperty("order")]
        public int Order { get; private set; }

        public ValueMutator() => Singletons.Get<IdRegistry>().Register(this);
        
        public ValueMutator(int order) : this() => Order = order;

        public virtual T Mutate(T value) => value;
    }
}
