using System;

#nullable enable
namespace DarkFrontier.Foundation.Identification {
    [Serializable]
    public class Id {
        public readonly string uId;

        public Id () => uId = Guid.NewGuid ().ToString ();
        public Id (string aId) => uId = aId;
    }
}
#nullable restore
