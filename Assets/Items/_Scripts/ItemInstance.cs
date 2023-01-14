#nullable enable
using System;
using DarkFrontier.Attributes;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Game.Essentials;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Items._Scripts
{
    [Serializable, JsonObject(MemberSerialization.OptIn, IsReference = true)]
    public class ItemInstance : IEquatable<ItemInstance>
    {
        [field: SerializeReference, Expandable]
        public ItemPrototype Prototype { get; private set; } = null!;
        
        [field: SerializeReference] [JsonProperty("prototype-id")]
        public string? PrototypeId { get; private set; }
        
        [field: SerializeReference] [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        public ItemInstance()
        {
        }

        public ItemInstance(ItemPrototype prototype)
        {
            Prototype = prototype;
            PrototypeId = Prototype.id;
        }

        public virtual void OnSerialize()
        {
            PrototypeId = Prototype.id;
            // TODO write to serialization writers
        }
        
        public virtual void OnDeserialize()
        {
            if (PrototypeId != null)
            {
                var prototype = Singletons.Get<IdRegistry>().Get<ItemPrototype>(PrototypeId);
                Prototype = prototype == null ? Prototype : prototype;
            }
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
            return Prototype.id.GetHashCode();
        }
    }
}
