using System;
using System.Collections.Generic;
using DarkFrontier.Items.Conditions;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Items {
    [CreateAssetMenu (menuName = "Items/Item")]
    public class ItemPrototype : ScriptableObject {
        public string Name;
        [TextArea] public string Description;
        public string Id;
        public Sprite Icon;
        public float Volume;
        public List<ItemTag> Tags;
        public bool Usable;

        public virtual State NewState() => new State(this);

        public class State : IEquatable<State> {
            public readonly ItemPrototype uPrototype;

            public State(ItemPrototype aPrototype) {
                uPrototype = aPrototype;
            }

            public override bool Equals(object aObj) {
                if (ReferenceEquals(null, aObj)) return false;
                if (ReferenceEquals(this, aObj)) return true;
                if (aObj.GetType() != GetType()) return false;
                return Equals((State)aObj);
            }
            
            public bool Equals(State aOther) {
                if (ReferenceEquals(null, aOther)) return false;
                if (ReferenceEquals(this, aOther)) return true;
                return Equals(uPrototype, aOther.uPrototype);
            }

            public override int GetHashCode() {
                return uPrototype != null ? uPrototype.GetHashCode() : 0;
            }

            public class Converter : JsonConverter<State> {
                public override State ReadJson(JsonReader aReader, Type aObjectType, State aExistingValue, bool aHasExistingValue, JsonSerializer aSerializer) {
                    throw new NotImplementedException();
                }
                
                public override void WriteJson(JsonWriter aWriter, State aValue, JsonSerializer aSerializer) {
                    throw new NotImplementedException();
                }
            }
        }
    }
}

