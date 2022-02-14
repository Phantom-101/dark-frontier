using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    public class IntClampMutator : ValueMutator<int>
    {
        [field: SerializeReference]
        [JsonProperty("min")]
        public int Min { get; private set; }
        
        [field: SerializeReference]
        [JsonProperty("max")]
        public int Max { get; private set; }

        public IntClampMutator(int min, int max, int order) : base(order)
        {
            Min = min;
            Max = max;
        }

        public override int Mutate(int value)
        {
            return Mathf.Clamp(value, Min, Max);
        }
    }

    public class FloatClampMutator : ValueMutator<float>
    {
        [field: SerializeReference]
        [JsonProperty("min")]
        public float Min { get; private set; }
        
        [field: SerializeReference]
        [JsonProperty("max")]
        public float Max { get; private set; }
        
        public FloatClampMutator(float min, float max, int order) : base(order)
        {
            Min = min;
            Max = max;
        }

        public override float Mutate(float value)
        {
            return Mathf.Clamp(value, Min, Max);
        }
    }
}