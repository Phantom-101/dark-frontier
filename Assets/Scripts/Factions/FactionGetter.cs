using DarkFrontier.Foundation.Identification;
using DarkFrontier.Foundation.Services;
using System;


namespace DarkFrontier.Factions {
    [Serializable]
    public class FactionGetter : IdGetter<Faction> {
        private static readonly Lazy<FactionManager> iFactionManager = new Lazy<FactionManager>(() => Singletons.Get<FactionManager>(), false);
        
        public FactionGetter () : base (iGetter) { }
        private static Func<string, Faction?> iGetter = aId => iFactionManager.Value.Registry.Find(aId);
    }
}

