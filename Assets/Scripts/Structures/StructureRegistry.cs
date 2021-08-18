using System;
using System.Collections.Generic;

namespace DarkFrontier.Structures {
    [Serializable]
    public class StructureRegistry {
        public List<Structure> Structures { get; } = new List<Structure> ();
    }
}
