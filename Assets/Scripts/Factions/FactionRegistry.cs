using System;
using System.Collections.Generic;

namespace DarkFrontier.Factions {
    [Serializable]
    public class FactionRegistry {
        public List<Faction> Factions { get; } = new List<Faction> ();
    }
}
