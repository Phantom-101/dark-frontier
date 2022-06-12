using System;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    [Serializable]
    public class IntMulMutator : ValueMutator<int>
    {
        [SerializeReference, JsonProperty("parameter")]
        public IValue<int> parameter;

        public IntMulMutator(IValue<int> parameter, int order) : base(order)
        {
            this.parameter = parameter;
        }

        public override int Mutate(int value)
        {
            return value * parameter.Value;
        }
    }

    [Serializable]
    public class FloatMulMutator : ValueMutator<float>
    {
        [SerializeReference, JsonProperty("parameter")]
        public IValue<float> parameter;
        
        public FloatMulMutator(IValue<float> parameter, int order) : base(order)
        {
            this.parameter = parameter;
        }

        public override float Mutate(float value)
        {
            return value * parameter.Value;
        }
    }
}