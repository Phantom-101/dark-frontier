using DarkFrontier.Foundation.Identification;
using DarkFrontier.Foundation.Services;
using System;

#nullable enable
namespace DarkFrontier.Factions {
    [Serializable]
    public class FactionGetter : IdGetter<Faction> {
        public FactionGetter () : base (Getter) { }
        private static Faction? Getter (string id) => Singletons.Get<FactionManager> ().Registry.Find (id);
    }
}
#nullable restore
