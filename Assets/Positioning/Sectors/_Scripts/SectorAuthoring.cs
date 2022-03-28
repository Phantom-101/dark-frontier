#nullable enable
using DarkFrontier.Utils;
using UnityEngine;

namespace DarkFrontier.Positioning.Sectors
{
    public class SectorAuthoring : MonoBehaviour
    {
        public SectorInstance? instance;

        public void Author()
        {
            var component = ComponentUtils.AddOrGet<SectorComponent>(gameObject);
            component.Initialize();
            component.Set(instance);
            Destroy(this);
        }
    }
}