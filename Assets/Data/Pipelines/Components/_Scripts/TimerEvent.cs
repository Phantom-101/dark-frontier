using System;
using System.Collections;
using UnityEngine;

namespace DarkFrontier.Data.Pipelines.Components
{
    public class TimerEvent : PipelineComponent<byte>
    {
        public float time;
        public PipelineComponent<byte>[] targets = Array.Empty<PipelineComponent<byte>>();

        public override void Process(byte input)
        {
            StartCoroutine(Delay(input));
        }

        private IEnumerator Delay(byte input)
        {
            yield return new WaitForSeconds(time);
            Processed(targets, input);
        }
    }
}
