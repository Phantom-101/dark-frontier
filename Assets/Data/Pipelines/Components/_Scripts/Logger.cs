using UnityEngine;

namespace DarkFrontier.Data.Pipelines.Components
{
    public class Logger : PipelineComponent<byte>
    {
        public string message;
        
        public override void Process(byte input)
        {
            Debug.Log(message);
        }
    }
}