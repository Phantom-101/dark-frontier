using System;
using UnityEngine;

#nullable enable
namespace DarkFrontier.Foundation.Identification {
    [Serializable]
    public class Id {
        public string Value { get => GetValue (); }
        [SerializeField] private string value;

        public bool IsEmpty { get => string.IsNullOrEmpty (value) || string.IsNullOrWhiteSpace (value); }

        public Id () => value = "";
        public Id (string value) => this.value = value;

        private string GetValue () {
            if (IsEmpty) value = Guid.NewGuid ().ToString ();
            return value;
        }

        public static implicit operator string (Id id) => id.Value;
    }
}
#nullable restore
