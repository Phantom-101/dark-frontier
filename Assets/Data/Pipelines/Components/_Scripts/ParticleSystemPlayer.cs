using UnityEngine;

namespace DarkFrontier.Data.Pipelines.Components
{
    public class ParticleSystemPlayer : PipelineComponent<byte>
    {
        public ParticleSystem system;
        
        public override void Process(byte input)
        {
            system.Play();
        }
    }
}