#nullable enable
using System.Collections.Generic;
using DarkFrontier.Objects.Components;

namespace DarkFrontier.Objects
{
    public class Structure : SectorObject
    {
        public Detector detector = null!;
        public List<StructurePart> parts = new();
        public Selectable? selected;

        public void Add(StructurePart part)
        {
            if (!parts.Contains(part))
            {
                parts.Add(part);
            }
        }

        public void Remove(StructurePart part)
        {
            parts.Remove(part);
        }
    }
}