#nullable enable
using Newtonsoft.Json;
using System;
using DarkFrontier.Attributes;
using DarkFrontier.Items.Structures;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Positioning.Sectors
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SectorInstance : IDetectable
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
        
        public bool SetComponent(SectorComponent component)
        {
            Component = component;
            return true;
        }

        public bool IsDetected(StructureInstance structure)
        {
            return true;
        }

        public VisualElement CreateSelector()
        {
            return new VisualElement();
        }

        public Vector3 GetSelectorPosition()
        {
            return Vector3.zero;
        }

        public VisualElement CreateSelected()
        {
            return new VisualElement();
        }
    }
}
