using System;
using UnityEngine;

namespace DarkFrontier.Foundation {
    [Serializable]
    public class Id {
        public string Value { get => GetValue (); }
        [SerializeField] private string value;

        public Id () {
            value = "";
        }

        public Id (string value) {
            this.value = value;
        }

        private string GetValue () {
            if (!Guid.TryParse (value, out _)) value = Guid.NewGuid ().ToString ();
            return value;
        }

        public static implicit operator string (Id id) => id.Value;
    }
}
