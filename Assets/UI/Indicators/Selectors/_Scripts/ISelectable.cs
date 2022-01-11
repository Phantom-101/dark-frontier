using DarkFrontier.Items.Structures;
using UnityEngine;

#nullable enable
namespace DarkFrontier.UI.Indicators.Selectors
{
    public interface ISelectable : IInfo
    {
        public ISelectable? Parent { get; }

        public ISelectable[] Children { get; }

        public Vector3 Position { get; }

        public bool IsDetected(StructureComponent structure);

        Selector? CreateSelector(Transform? root);
    }
}
#nullable restore