using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    public class IntMulMutator : ValueMutator<int>
    {
        [field: SerializeReference]
        [JsonProperty("parameter")]
        public int Parameter { get; private set; }

        public IntMulMutator(int parameter, int order) : base(order)
        {
            Parameter = parameter;
        }

        public override int Mutate(int value)
        {
            return value * Parameter;
        }
    }

    public class FloatMulMutator : ValueMutator<float>
    {
        [field: SerializeReference]
        [JsonProperty("parameter")]
        public float Parameter { get; private set; }
        
        public FloatMulMutator(float parameter, int order) : base(order)
        {
            Parameter = parameter;
        }

        public override float Mutate(float value)
        {
            return value * Parameter;
        }
    }
}