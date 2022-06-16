using System;
using System.Collections.Generic;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Game.Essentials;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    [Serializable, JsonObject(MemberSerialization.OptIn, IsReference = true)]
    public class MutableValue<T> : IValue<T>
    {
        [field: SerializeField] [JsonProperty("id")]
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        
        [SerializeField, JsonProperty("base-value")]
        public T baseValue;
        
        public T Value {
            get
            {
                var ret = baseValue;
                for(int i = 0, l = Mutators.Count; i < l; i++) ret = Mutators[i].Mutate(ret);
                return ret;
            }
        }
        
        [JsonProperty("mutators")]
        public List<IMutator<T>> Mutators { get; protected set; } = new();

        public MutableValue() => Singletons.Get<IdRegistry>().Register(this);

        public MutableValue(T baseValue) : this() => this.baseValue = baseValue;

        public void AddMutator(ValueMutator<T> mutator)
        {
            int i = 0, l = Mutators.Count;
            while(i < l && Mutators[i].Order <= mutator.Order) i++;
            Mutators.Insert(i, mutator);
        }

        public void RemoveMutator(ValueMutator<T> mutator) => Mutators.Remove(mutator);

        public static implicit operator T(MutableValue<T> mutableValue) => mutableValue.Value;
        
        public static implicit operator MutableValue<T>(T value) => new(value);
    }

    [Serializable]
    public class MutableInt : MutableValue<int>
    {
        public MutableInt()
        {
        }

        public MutableInt(int baseValue) : base(baseValue)
        {
        }
    }
    
    [Serializable]
    public class MutableFloat : MutableValue<float>
    {
        public MutableFloat()
        {
        }

        public MutableFloat(float baseValue) : base(baseValue)
        {
        }
    }
}
