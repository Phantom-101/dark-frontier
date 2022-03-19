using System;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    [Serializable]
    public class IntSetMutator : ValueMutator<int>
    {
        [SerializeReference, JsonProperty("parameter")]
        public MutableValue<int> parameter;

        public IntSetMutator(MutableValue<int> parameter, int order) : base(order)
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
        public MutableValue<float> parameter;
        
        public FloatSetMutator(MutableValue<float> parameter, int order) : base(order)
        {
            this.parameter = parameter;
        }

        public override float Mutate(float value)
        {
            return parameter.Value;
        }
    }
}