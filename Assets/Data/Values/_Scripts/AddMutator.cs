using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    public class IntAddMutator : ValueMutator<int>
    {
        [field: SerializeReference]
        [JsonProperty("parameter")]
        public int Parameter { get; private set; }

        public IntAddMutator(int parameter, int order) : base(order)
        {
            Parameter = parameter;
        }

        public override int Mutate(int value)
        {
            return value + Parameter;
        }
    }

    public class FloatAddMutator : ValueMutator<float>
    {
        [field: SerializeReference]
        [JsonProperty("parameter")]
        public float Parameter { get; private set; }
        
        public FloatAddMutator(float parameter, int order) : base(order)
        {
            Parameter = parameter;
        }

        public override float Mutate(float value)
        {
            return value + Parameter;
        }
    }
}