using DarkFrontier.Foundation.Identification;
using DarkFrontier.Foundation.Services;
using System;

#nullable enable
namespace DarkFrontier.Locations {
    [Serializable]
    public class SectorGetter : IdGetter<Sector> {
        private static readonly Lazy<SectorManager> iSectorManager = new Lazy<SectorManager>(() => Singletons.Get<SectorManager>(), false);
        
        public SectorGetter () : base (iGetter) { }
        private static Func<string, Sector?> iGetter = aId => iSectorManager.Value.Registry.Find(aId);
    }
}
#nullable restore
