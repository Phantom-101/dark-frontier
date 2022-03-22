using System;

namespace DarkFrontier.Data.Pipelines.Components
{
    public class StartEvent : PipelineComponent<byte>
    {
        public PipelineComponent<byte>[] targets = Array.Empty<PipelineComponent<byte>>();
        
        private void Start()
        {
            Process(0);
        }

        public override void Process(byte input)
        {
            Processed(targets, input);
        }
    }
}
