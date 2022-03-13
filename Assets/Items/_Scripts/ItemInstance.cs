#nullable enable
using System;
using DarkFrontier.Attributes;
using DarkFrontier.Items.Structures;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Items._Scripts
{
    [Serializable, JsonObject(MemberSerialization.OptIn, IsReference = true)]
    public class ItemInstance : IInfo, IEquatable<ItemInstance>
    {
        [field: SerializeReference, Expandable]
        public ItemPrototype Prototype { get; private set; } = null!;
        
        [field: SerializeReference] [JsonProperty("prototype-id")]
        public string PrototypeId { get; private set; } = "";
        
        [field: SerializeReference] [JsonProperty("id")]
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        
        [field: SerializeReference] [JsonProperty("name")]
        public string Name { get; protected set; } = "";
        
        [field: SerializeReference, TextArea] [JsonProperty("description")]
        public string Description { get; protected set; } = "";

        public ItemInstance()
        {
        }

        public ItemInstance(ItemPrototype prototype)
        {
            Prototype = prototype;
            PrototypeId = Prototype.id;
        }

        public virtual void ToSerialized()
        {
        }

        public virtual void FromSerialized()
        {
            Prototype = PrototypeId.Length > 0 ? new ItemRegistry().Get(PrototypeId)! : Prototype;
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
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return PrototypeId.GetHashCode();
        }
    }
}
