using System;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    [Serializable]
    public class IntSetMutator : ValueMutator<int>
    {
        [SerializeReference, JsonProperty("parameter")]
        public IValue<int> parameter;

        public IntSetMutator(IValue<int> parameter, int order) : base(order)
        {
            this.parameter = parameter;
        }

        public override int Mutate(int value)
        {
            return parameter.Value;
        }
    }

    [Serializable]
    public class FloatSetMutator : ValueMutator<float>
    {
        [SerializeReference, JsonProperty("parameter")]
        public IValue<float> parameter;
        
        public FloatSetMutator(IValue<float> parameter, int order) : base(order)
        {
            this.parameter = parameter;
        }

        public override float Mutate(float value)
        {
            return parameter.Value;
        }
    }
}