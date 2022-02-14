#nullable enable
using Newtonsoft.Json;
using System;
using DarkFrontier.Attributes;
using DarkFrontier.Items.Structures;
using UnityEngine;
using UnityEngine.AddressableAssets;
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

        [field: SerializeReference]
        [JsonProperty("selector-addressable-key")]
        public string SelectorAddressableKey { get; private set; } = "";
        
        public bool SetComponent(SectorComponent component)
        {
            Component = component;
            return true;
        }

        public bool IsDetected(StructureInstance structure)
        {
            return structure.Sector != this;
        }

        public VisualElement CreateSelector()
        {
            return Addressables.LoadAssetAsync<VisualTreeAsset>(SelectorAddressableKey).WaitForCompletion().CloneTree();
        }

        public Vector3 GetSelectorPosition()
        {
            return Component == null ? Vector3.zero : UnityEngine.Camera.main!.WorldToViewportPoint(Position);
        }

        public VisualElement CreateSelected()
        {
            return new VisualElement();
        }
    }
}
