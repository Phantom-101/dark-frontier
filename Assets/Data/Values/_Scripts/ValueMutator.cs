using System;
using Newtonsoft.Json;
using UnityEngine;


namespace DarkFrontier.Data.Values
{
    public class ValueMutator<T>
    {
        [field: SerializeReference]
        [JsonProperty("id")]
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        
        [field: SerializeReference]
        [JsonProperty("order")]
        public virtual int Order { get; private set; }

        public ValueMutator(int order) => Order = order;

        public virtual T Mutate(T value) => value;
    }
}
