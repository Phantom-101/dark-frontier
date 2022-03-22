#nullable enable
using Newtonsoft.Json;
using System;
using DarkFrontier.Attributes;
using DarkFrontier.Positioning.Navigation;
using DarkFrontier.Utils;
using UnityEngine;

namespace DarkFrontier.Positioning.Sectors
{
    [Serializable, JsonObject(MemberSerialization.OptIn)]
    public class SectorInstance
    {
        [field: SerializeReference, ReadOnly]
        public SectorComponent? Component { get; private set; }
        
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
        [JsonProperty("selector-addressable-key")]
        public string SelectorAddressableKey { get; private set; } = "";
        
        public NavigationPathfinder? Pathfinder { get; private set; }

        public void UpdatePathfinder(GameObject gameObject) => ComponentUtils.AddOrGet<NavigationPathfinder>(gameObject).Initialize(gameObject.scene);
    }
}
