using DarkFrontier.Foundation.Identification;
using DarkFrontier.Foundation.Services;
using System;

#nullable enable
namespace DarkFrontier.Structures {
    [Serializable]
    public class StructureGetter : IdGetter<Structure> {
        private static readonly Lazy<StructureManager> iStructureManager = new Lazy<StructureManager>(() => Singletons.Get<StructureManager>(), false);
    
        public StructureGetter () : base (iGetter) { }
        private static Func<string, Structure?> iGetter = aId => iStructureManager.Value.Registry.Find(aId);
    }
}
#nullable restore
