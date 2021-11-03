using DarkFrontier.Foundation.Identification;
using DarkFrontier.Foundation.Services;
using System;

#nullable enable
namespace DarkFrontier.Structures {
    [Serializable]
    public class StructureGetter : IdGetter<Structure>, IEquatable<StructureGetter> {
        public StructureGetter () : base (iGetter) { }
        private static Func<string, Structure?> iGetter = aId => Singletons.Get<StructureManager>().Registry.Find(aId);
        
        public override bool Equals(object? aObj) {
            if (ReferenceEquals(null, aObj)) return false;
            if (ReferenceEquals(this, aObj)) return true;
            return aObj.GetType() == GetType() && Equals((StructureGetter) aObj);
        }
        
        public bool Equals(StructureGetter aOther) => UId.Value == aOther!.UId.Value;

        public override int GetHashCode() => UId.Value.GetHashCode();
    }
}
#nullable restore
