using DarkFrontier.Foundation.Behaviors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

#nullable enable
namespace DarkFrontier.Structures {
    public class DockingPoint : ComponentBehavior, ICtorArgs<DockingPoint.State> {
        public State UState { get => iState; }
        [SerializeField] private State iState = new State ();

        public float UMaxVolume { get => iMaxVolume; }
        public float UMaxSize { get => iMaxSize; }
        public Structure? UDocker { get => iState.uDocker.UValue; }
        public string USerializationId { get => iSerializationId; }

#pragma warning disable IDE0044 // Add readonly modifier
        [SerializeField] private float iMaxVolume;
        [SerializeField] private float iMaxSize;
        [SerializeField] private string iSerializationId = "";
#pragma warning restore IDE0044 // Add readonly modifier

        public void Construct (State aState) => iState = aState;

        public bool CanAccept (Structure docker) {
            if (UDocker != null) return false;
            if ((docker.UPrototype?.Volume ?? 0) > UMaxVolume) return false;
            if ((docker.UPrototype?.ApparentSize ?? 0) > UMaxSize) return false;
            return true;
        }

        public bool TryAccept (Structure docker) {
            if (!CanAccept (docker)) return false;
            iState.uDocker.UId.Value = docker.UId;
            return true;
        }

        public bool CanRelease (Structure docker) => iState.uDocker.UId.Value == docker.UId;

        public bool TryRelease (Structure docker) {
            if (!CanRelease (docker)) return false;
            iState.uDocker.UId.Value = "";
            return true;
        }

        [Serializable]
        public class State {
            public readonly StructureGetter uDocker = new StructureGetter ();

            public State () { }
            public State (string aDockerId) => uDocker.UId.Value = aDockerId;

            public class Converter : JsonConverter<State> {
                public override State ReadJson (JsonReader aReader, Type aType, State? aValue, bool aExists, JsonSerializer aSerializer) {
                    JObject lObj = (JToken.ReadFrom (aReader) as JObject)!;

                    return new State (lObj.Value<string> ("DockerId"));
                }

                public override void WriteJson (JsonWriter aWriter, State? aValue, JsonSerializer aSerializer) {
                    if (aValue == null) {
                        aWriter.WriteNull ();
                    } else {
                        JObject lObj = new JObject {
                            new JProperty ("DockerId", aValue.uDocker.UId.Value),
                        };

                        lObj.WriteTo (aWriter);
                    }
                }
            }
        }

        public class Converter : JsonConverter<DockingPoint> {
            public override bool CanRead => false;

            public override DockingPoint ReadJson (JsonReader aReader, Type aType, DockingPoint? aValue, bool aExists, JsonSerializer aSerializer) {
                throw new NotImplementedException ("Reading not supported");
            }

            public override void WriteJson (JsonWriter aWriter, DockingPoint? aValue, JsonSerializer aSerializer) {
                if (aValue == null) {
                    aWriter.WriteNull ();
                } else {
                    JsonSerializer lSerializer = new JsonSerializer {
                        Formatting = Formatting.Indented,
                        TypeNameHandling = TypeNameHandling.All,
                    };
                    lSerializer.Converters.Add (new State.Converter ());

                    JObject lObj = JObject.FromObject (aValue.iState, lSerializer);

                    lObj.Add (new JProperty ("SerializationId", aValue.iSerializationId));

                    lObj.WriteTo (aWriter);
                }
            }
        }
    }
}
#nullable restore
