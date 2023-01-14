#nullable enable
using System;
using System.Collections.Generic;
using DarkFrontier.Items.Conditions;
using UnityEngine;

namespace DarkFrontier.Items._Scripts
{
    [CreateAssetMenu(menuName = "Items/Prototypes/Item")]
    public class ItemPrototype : ScriptableObject
    {
        public string id = Guid.NewGuid().ToString();
        
        public new string name = "";

        [TextArea]
        public string description = "";

        public float volume;
        
        public Sprite? icon;

        public List<ItemTag> tags = new();

        public virtual ItemInstance NewInstance() => new(this);
    }
}
