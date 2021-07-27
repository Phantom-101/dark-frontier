using System.Collections.Generic;

namespace DarkFrontier.Structures {
    public class StructureRegistry {
        public List<Structure> Structures { get; } = new List<Structure> ();

        public void Add (Structure structure) => Structures.Add (structure);
        public void Remove (Structure structure) => Structures.Remove (structure);
    }
}
