using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    public class IntMaxMutator : ValueMutator<int>
    {
        [field: SerializeReference]
        [JsonProperty("parameter")]
        public int Parameter { get; private set; }

        public IntMaxMutator(int parameter, int order) : base(order)
        {
            Parameter = parameter;
        }

        public override int Mutate(int value)
        {
            return Mathf.Max(value, Parameter);
        }
    }

    public class FloatMaxMutator : ValueMutator<float>
    {
        [field: SerializeReference]
        [JsonProperty("parameter")]
        public float Parameter { get; private set; }
        
        public FloatMaxMutator(float parameter, int order) : base(order)
        {
            Parameter = parameter;
        }

        public override float Mutate(float value)
        {
            return Mathf.Max(value, Parameter);
        }
    }
}