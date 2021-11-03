using DarkFrontier.Foundation.Identification;
using DarkFrontier.Foundation.Services;
using System;

#nullable enable
namespace DarkFrontier.Locations {
    [Serializable]
    public class SectorGetter : IdGetter<Sector> {
        public SectorGetter () : base (Getter) { }
        private static Sector? Getter (string aId) => Singletons.Get<SectorManager> ().Registry.Find (aId);
    }
}
#nullable restore
