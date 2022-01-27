using Newtonsoft.Json;
using System;
using UnityEngine;


namespace DarkFrontier.Items.Segments
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class SegmentRecord
    {
        [JsonProperty]
        [field: SerializeReference]
        public string Name { get; private set; } = "";

        [JsonProperty]
        [field: SerializeReference]
        public SegmentInstance Instance { get; private set; } = new SegmentInstance();
    }
}
