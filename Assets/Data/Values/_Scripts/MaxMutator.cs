using System;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    [Serializable]
    public class IntMaxMutator : ValueMutator<int>
    {
        [SerializeReference, JsonProperty("parameter")]
        public MutableValue<int> parameter;

        public IntMaxMutator(MutableValue<int> parameter, int order) : base(order)
        {
            this.parameter = parameter;
        }

        public override int Mutate(int value)
        {
            return Mathf.Max(value, parameter.Value);
        }
    }

    [Serializable]
    public class FloatMaxMutator : ValueMutator<float>
    {
        [SerializeReference, JsonProperty("parameter")]
        public MutableValue<float> parameter;
        
        public FloatMaxMutator(MutableValue<float> parameter, int order) : base(order)
        {
            this.parameter = parameter;
        }

        public override float Mutate(float value)
        {
            return Mathf.Max(value, parameter.Value);
        }
    }
}