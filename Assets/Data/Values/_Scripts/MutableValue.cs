using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    [Serializable, JsonObject(MemberSerialization.OptIn, IsReference = true)]
    public class MutableValue<T>
    {
        [field: SerializeReference]
        [JsonProperty("base-value")]
        public T BaseValue { get; private set; }
        
        [field: SerializeReference]
        public T Value { get; private set; }
        
        private readonly List<ValueMutator<T>> _mutators = new();

        public MutableValue(T baseValue)
        {
            BaseValue = baseValue;
            Value = baseValue;
        }

        public bool AddMutator(ValueMutator<T> mutator)
        {
            int i, l;
            for (i = 0, l = _mutators.Count; i < l; i++)
            {
                if (_mutators[i].Order == mutator.Order)
                {
                    return false;
                }

                if (_mutators[i].Order > mutator.Order)
                {
                    break;
                }
            }
            _mutators.Insert(i, mutator);
            Recalculate();
            return true;
        }

        public bool RemoveMutator(ValueMutator<T> mutator)
        {
            if(_mutators.Remove(mutator))
            {
                Recalculate();
                return true;
            }
            return false;
        }

        private void Recalculate()
        {
            Value = BaseValue;
            for(int i = 0, l = _mutators.Count; i < l; i++)
            {
                Value = _mutators[i].Mutate(Value);
            }
        }

        public static implicit operator T(MutableValue<T> mutableValue) => mutableValue.Value;
        public static implicit operator MutableValue<T>(T value) => new(value);
    }
}
