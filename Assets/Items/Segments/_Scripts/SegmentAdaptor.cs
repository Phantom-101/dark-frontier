#nullable enable
using System;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Items.Segments
{
    [Serializable]
    public class SegmentAdaptor
    {
        [field: SerializeReference] [JsonProperty("id")]
        public string id = Guid.NewGuid().ToString();
        public SegmentComponent slot = null!;
    }
}