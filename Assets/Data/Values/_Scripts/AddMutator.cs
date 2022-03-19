using System;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    [Serializable]
    public class IntAddMutator : ValueMutator<int>
    {
        [SerializeReference, JsonProperty("parameter")]
        public MutableValue<int> parameter;

        public IntAddMutator(MutableValue<int> parameter, int order) : base(order)
        {
            this.parameter = parameter;
        }

        public override int Mutate(int value)
        {
            return value + parameter.Value;
        }
    }

    [Serializable]
    public class FloatAddMutator : ValueMutator<float>
    {
        [SerializeReference, JsonProperty("parameter")]
        public MutableValue<float> parameter;
        
        public FloatAddMutator(MutableValue<float> parameter, int order) : base(order)
        {
            this.parameter = parameter;
        }

        public override float Mutate(float value)
        {
            return value + parameter.Value;
        }
    }
}