using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    public class IntSetMutator : ValueMutator<int>
    {
        [field: SerializeReference]
        [JsonProperty("parameter")]
        public int Parameter { get; private set; }

        public IntSetMutator(int parameter, int order) : base(order)
        {
            Parameter = parameter;
        }

        public override int Mutate(int value)
        {
            return Parameter;
        }
    }

    public class FloatSetMutator : ValueMutator<float>
    {
        [field: SerializeReference]
        [JsonProperty("parameter")]
        public float Parameter { get; private set; }
        
        public FloatSetMutator(float parameter, int order) : base(order)
        {
            Parameter = parameter;
        }

        public override float Mutate(float value)
        {
            return Parameter;
        }
    }
}