using System.Collections.Generic;
using DarkFrontier.Items.Structures;

namespace DarkFrontier.UI.Indicators.Selectors
{
    public class DetectableRegistry
    {
        public List<IDetectable> Registry { get; } = new();
        
        public void Register(IDetectable detectable) => Registry.Add(detectable);

        public void Unregister(IDetectable detectable) => Registry.Remove(detectable);
    }
}