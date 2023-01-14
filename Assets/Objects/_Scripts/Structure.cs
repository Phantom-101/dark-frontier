#nullable enable
using System.Collections.Generic;

namespace DarkFrontier.Objects
{
    public class Structure : DestructibleObject
    {
        public List<StructureComponent> components = new();

        public void Add(StructureComponent comp)
        {
            if (!components.Contains(comp))
            {
                components.Add(comp);
            }
        }

        public void Remove(StructureComponent comp)
        {
            components.Remove(comp);
        }
    }
}