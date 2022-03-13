#nullable enable
using DarkFrontier.Utils;
using UnityEngine;

namespace DarkFrontier.Items.Structures
{
    public class StructureAuthoring : MonoBehaviour
    {
        public StructureInstance? instance;

        public void Generate()
        {
            var component = ComponentUtils.AddOrGet<StructureComponent>(gameObject);
            component.Initialize();
            component.Set(instance);
            Destroy(this);
        }
    }
}