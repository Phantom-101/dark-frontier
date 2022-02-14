using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    public class IntMinMutator : ValueMutator<int>
    {
        [field: SerializeReference]
        [JsonProperty("parameter")]
        public int Parameter { get; private set; }

        public IntMinMutator(int parameter, int order) : base(order)
        {
            Parameter = parameter;
        }

        public override int Mutate(int value)
        {
            return Mathf.Min(value, Parameter);
        }
    }

    public class FloatMinMutator : ValueMutator<float>
    {
        [field: SerializeReference]
        [JsonProperty("parameter")]
        public float Parameter { get; private set; }
        
        public FloatMinMutator(float parameter, int order) : base(order)
        {
            Parameter = parameter;
        }

        public override float Mutate(float value)
        {
            return Mathf.Min(value, Parameter);
        }
    }
}