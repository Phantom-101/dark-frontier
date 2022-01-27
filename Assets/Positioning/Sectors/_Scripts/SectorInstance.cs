using Newtonsoft.Json;
using System;
using UnityEngine;


namespace DarkFrontier.Positioning.Sectors
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SectorInstance
    {
        [field: SerializeReference]
        [JsonProperty("id")]
        public string Id { get; private set; } = Guid.NewGuid().ToString();

        [field: SerializeReference]
        [JsonProperty("name")]
        public string Name { get; private set; } = "";

        [field: SerializeReference]
        [JsonProperty("description")]
        public string Description { get; private set; } = "";
        
        [field: SerializeReference]
        [JsonProperty("position")]
        public Vector3 Position { get; private set; }

        [field: SerializeReference]
        [JsonProperty("size")]
        public float Size { get; private set; } = 50000;

        [field: SerializeReference]
        [JsonProperty("selector-address")]
        public string SelectorPrefabAddress { get; private set; } = "";
    }
}
