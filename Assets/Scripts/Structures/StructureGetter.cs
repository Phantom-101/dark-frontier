using DarkFrontier.Foundation.Identification;
using DarkFrontier.Foundation.Services;
using System;

#nullable enable
namespace DarkFrontier.Structures {
    [Serializable]
    public class StructureGetter : IdGetter<Structure> {
        public StructureGetter () : base (iGetter) { }
        private static Func<string, Structure?> iGetter = aId => Singletons.Get<StructureManager>().Registry.Find(aId);
    }
}
#nullable restore
