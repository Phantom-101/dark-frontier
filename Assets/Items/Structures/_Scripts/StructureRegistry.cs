using System.Collections.Generic;

namespace DarkFrontier.Items.Structures
{
    public class StructureRegistry
    {
        public List<StructureComponent> Registry { get; } = new();

        public void Register(StructureComponent structure) => Registry.Add(structure);

        public void Unregister(StructureComponent structure) => Registry.Remove(structure);
    }
}