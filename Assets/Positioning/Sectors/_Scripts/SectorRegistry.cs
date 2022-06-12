using System.Collections.Generic;

namespace DarkFrontier.Positioning.Sectors
{
    public class SectorRegistry
    {
        public List<SectorComponent> Registry { get; } = new();

        public void Register(SectorComponent sector) => Registry.Add(sector);

        public void Unregister(SectorComponent sector) => Registry.Remove(sector);
    }
}
