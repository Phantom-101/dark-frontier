using System;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    [Serializable]
    public class IntMinMutator : ValueMutator<int>
    {
        [SerializeReference, JsonProperty("parameter")]
        public IValue<int> parameter;

        public IntMinMutator(IValue<int> parameter, int order) : base(order)
        {
            this.parameter = parameter;
        }

        public override int Mutate(int value)
        {
            return Mathf.Min(value, parameter.Value);
        }
    }

    [Serializable]
    public class FloatMinMutator : ValueMutator<float>
    {
        [SerializeReference, JsonProperty("parameter")]
        public IValue<float> parameter;
        
        public FloatMinMutator(IValue<float> parameter, int order) : base(order)
        {
            this.parameter = parameter;
        }

        public override float Mutate(float value)
        {
            return Mathf.Min(value, parameter.Value);
        }
    }
}