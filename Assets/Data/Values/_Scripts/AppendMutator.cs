using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    [Serializable]
    public class Vector3ListAppendMutator : ValueMutator<List<Vector3>>
    {
        [SerializeReference, JsonProperty("parameter")]
        public IValue<Vector3> parameter;

        public Vector3ListAppendMutator(IValue<Vector3> parameter, int order) : base(order)
        {
            this.parameter = parameter;
        }

        public override List<Vector3> Mutate(List<Vector3> value)
        {
            return new List<Vector3>(value) { parameter.Value };
        }
    }
}