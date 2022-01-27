using System;
using System.Runtime.Serialization;
using DarkFrontier.Attributes;
using DarkFrontier.Items.Structures;
using Newtonsoft.Json;
using UnityEngine;


namespace DarkFrontier.Items._Scripts
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class ItemInstance : IInfo, IEquatable<ItemInstance>
    {
        [field: SerializeReference, Expandable]
        public ItemPrototype Prototype { get; private set; } = null!;

        [JsonProperty("prototype-id")]
        private string _prototypeId = "";

        [JsonProperty("id")]
        [field: SerializeReference]
        public string Id { get; private set; } = Guid.NewGuid().ToString();

        [JsonProperty("name")]
        [field: SerializeReference]
        public string Name { get; protected set; } = "";

        [JsonProperty("description")]
        [field: SerializeReference, TextArea]
        public string Description { get; protected set; } = "";

        public ItemInstance()
        {
        }

        public ItemInstance(ItemPrototype prototype)
        {
            Prototype = prototype;
        }

        private void PreSerialize()
        {
            _prototypeId = Prototype.id;
        }

        private void PostDeserialize()
        {
            Prototype = _prototypeId.Length > 0 ? new ItemRegistry().Get(_prototypeId)! : Prototype;
        }

        public bool Equals(ItemInstance? other)
        {
            if(other is null) return false;
            return ReferenceEquals(this, other) || Equals(Prototype, other.Prototype);
        }

        public override bool Equals(object? obj)
        {
            if(obj is null) return false;
            if(ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ItemInstance)obj);
        }

        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyMemberInGetHashCode
            return Prototype != null ? Prototype.GetHashCode() : 0;
            // ReSharper restore NonReadonlyMemberInGetHashCode
        }
    }
}
