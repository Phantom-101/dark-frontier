using System;

namespace DarkFrontier.Stats {
    [Serializable]
    public class StructureStatsModifier {
        public virtual StructureStats Modify (StructureStats stats) => stats;
    }
}
