using System;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Game.Essentials;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    [Serializable, JsonObject(MemberSerialization.OptIn, IsReference = true)]
    public class ValueMutator<T> : IId
    {
        [field: SerializeReference]
        [JsonProperty("id")]
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        
        [field: SerializeReference]
        [JsonProperty("order")]
        public int Order { get; private set; }

        public ValueMutator() => Singletons.Get<IdRegistry>().Register(this);
        
        public ValueMutator(int order) : this() => Order = order;

        public virtual T Mutate(T value) => value;
    }
}
