using System;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    [Serializable]
    public class IntClampMutator : ValueMutator<int>
    {
        [SerializeReference, JsonProperty("min")]
        public MutableValue<int> min;

        [SerializeReference, JsonProperty("max")]
        public MutableValue<int> max;

        public IntClampMutator(MutableValue<int> min, MutableValue<int> max, int order) : base(order)
        {
            this.min = min;
            this.max = max;
        }

        public override int Mutate(int value)
        {
            return Mathf.Clamp(value, min.Value, max.Value);
        }
    }

    [Serializable]
    public class FloatClampMutator : ValueMutator<float>
    {
        [SerializeReference, JsonProperty("min")]
        public MutableValue<float> min;

        [SerializeReference, JsonProperty("max")]
        public MutableValue<float> max;
        
        public FloatClampMutator(MutableValue<float> min, MutableValue<float> max, int order) : base(order)
        {
            this.min = min;
            this.max = max;
        }

        public override float Mutate(float value)
        {
            return Mathf.Clamp(value, min.Value, max.Value);
        }
    }
}