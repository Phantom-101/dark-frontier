using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    [Serializable, JsonObject(MemberSerialization.OptIn, IsReference = true)]
    public class MutableValue<T>
    {
        [SerializeReference, JsonProperty("base-value")]
        public T baseValue;
        
        [field: SerializeReference] [JsonProperty("value")]
        public T Value { get; protected set; }
        
        [SerializeField, JsonProperty("mutators")]
        protected List<ValueMutator<T>> mutators = new();

        public MutableValue(T baseValue)
        {
            this.baseValue = baseValue;
            Value = baseValue;
        }

        public bool AddMutator(ValueMutator<T> mutator)
        {
            int i, l;
            for (i = 0, l = mutators.Count; i < l; i++)
            {
                if(mutators[i].Order == mutator.Order) return false;
                if(mutators[i].Order > mutator.Order) break;
            }
            mutators.Insert(i, mutator);
            Update();
            return true;
        }

        public bool RemoveMutator(ValueMutator<T> mutator)
        {
            if(!mutators.Remove(mutator)) return false;
            Update();
            return true;
        }

        public virtual void Update()
        {
            Value = baseValue;
            for(int i = 0, l = mutators.Count; i < l; i++)
            {
                Value = mutators[i].Mutate(Value);
            }
        }

        public static implicit operator T(MutableValue<T> mutableValue) => mutableValue.Value;
        public static implicit operator MutableValue<T>(T value) => new(value);
    }
}
