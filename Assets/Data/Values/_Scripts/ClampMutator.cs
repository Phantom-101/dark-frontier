using System;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    [Serializable]
    public class IntClampMutator : ValueMutator<int>
    {
        [SerializeReference, JsonProperty("min")]
        public IValue<int> min;

        [SerializeReference, JsonProperty("max")]
        public IValue<int> max;

        public IntClampMutator(IValue<int> min, IValue<int> max, int order) : base(order)
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
        public IValue<float> min;

        [SerializeReference, JsonProperty("max")]
        public IValue<float> max;
        
        public FloatClampMutator(IValue<float> min, IValue<float> max, int order) : base(order)
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