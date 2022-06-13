using System.Collections.Generic;
using DarkFrontier.Items.Structures;

namespace DarkFrontier.UI.Indicators.Selectors
{
    public class DetectableRegistry
    {
        public List<ISelectable> Registry { get; } = new();
        
        public void Register(ISelectable selectable) => Registry.Add(selectable);

        public void Unregister(ISelectable selectable) => Registry.Remove(selectable);
    }
}