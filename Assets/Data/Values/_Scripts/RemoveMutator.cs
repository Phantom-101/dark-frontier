using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    [Serializable]
    public class Vector3ListRemoveMutator : ValueMutator<List<Vector3>>
    {
        [SerializeReference, JsonProperty("parameter")]
        public IValue<Vector3> parameter;

        public Vector3ListRemoveMutator(IValue<Vector3> parameter, int order) : base(order)
        {
            this.parameter = parameter;
        }

        public override List<Vector3> Mutate(List<Vector3> value)
        {
            var ret = new List<Vector3>(value);
            ret.Remove(parameter.Value);
            return ret;
        }
    }
}