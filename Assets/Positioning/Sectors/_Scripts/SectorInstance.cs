#nullable enable
using Newtonsoft.Json;
using System;
using DarkFrontier.Attributes;
using DarkFrontier.Positioning.Navigation;
using DarkFrontier.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

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
        
        private VisualElement _selector = null!;
        private VisualElement _selected = null!;
        private VisualElement _unselected = null!;
        
        public NavigationPathfinder? Pathfinder { get; private set; }

        public void UpdatePathfinder(GameObject gameObject) => gameObject.AddOrGet<NavigationPathfinder>().Initialize(gameObject.scene);
        
        public virtual VisualElement CreateSelector()
        {
            _selector = Addressables.LoadAssetAsync<VisualTreeAsset>(SelectorAddressableKey).WaitForCompletion().CloneTree();
            _selector.Q<Label>("name").text = Name;
            _selected = _selector.Q("selected");
            _unselected = _selector.Q("unselected");
            return _selector;
        }

        public virtual void UpdateSelector(SectorComponent component, bool selected)
        {
            var position = component.camera.WorldToViewportPoint(Position);
            if(position.z > 0)
            {
                _selector.style.visibility = Visibility.Visible;
                _selector.style.left = new StyleLength(new Length(position.x * 100, LengthUnit.Percent));
                _selector.style.top = new StyleLength(new Length(100 - position.y * 100, LengthUnit.Percent));
                
                _selected.style.visibility = selected ? Visibility.Visible : Visibility.Hidden;
                _unselected.style.visibility = selected ? Visibility.Hidden : Visibility.Visible;
                _unselected.pickingMode = selected ? PickingMode.Ignore : PickingMode.Position;
            }
            else
            {
                _selector.style.visibility = Visibility.Hidden;
            }
        }
    }
}
