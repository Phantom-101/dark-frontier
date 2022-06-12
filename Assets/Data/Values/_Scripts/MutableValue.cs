using System;
using System.Collections.Generic;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Game.Essentials;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    [Serializable, JsonObject(MemberSerialization.OptIn, IsReference = true)]
    public class MutableValue<T> : IId, IValue<T>
    {
        [field: SerializeReference]
        [JsonProperty("id")]
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        
        [SerializeReference, JsonProperty("base-value")]
        public T baseValue;
        
        public T Value {
            get
            {
                var ret = baseValue;
                for(int i = 0, l = mutators.Count; i < l; i++) ret = mutators[i].Mutate(ret);
                return ret;
            }
        }
        
        [SerializeField, JsonProperty("mutators")]
        protected List<ValueMutator<T>> mutators = new();

        public MutableValue() => Singletons.Get<IdRegistry>().Register(this);

        public MutableValue(T baseValue) : this() => this.baseValue = baseValue;

        public void AddMutator(ValueMutator<T> mutator)
        {
            int i = 0, l = mutators.Count;
            while(i < l && mutators[i].Order <= mutator.Order) i++;
            mutators.Insert(i, mutator);
        }

        public void RemoveMutator(ValueMutator<T> mutator) => mutators.Remove(mutator);

        public static implicit operator T(MutableValue<T> mutableValue) => mutableValue.Value;
        
        public static implicit operator MutableValue<T>(T value) => new(value);
    }
}
