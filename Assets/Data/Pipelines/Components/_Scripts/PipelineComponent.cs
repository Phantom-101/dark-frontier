using System.Collections.Generic;
using UnityEngine;

namespace DarkFrontier.Data.Pipelines.Components
{
    public class PipelineComponent<T> : MonoBehaviour, IPipelineInput<T>
    {
        public virtual void Process(T input)
        {
        }

        protected void Processed<TValue>(IReadOnlyList<IPipelineInput<TValue>> pipelines, TValue value)
        {
            for(int i = 0, l = pipelines.Count; i < l; i++)
            {
                pipelines[i].Process(value);
            }
        }
    }
}