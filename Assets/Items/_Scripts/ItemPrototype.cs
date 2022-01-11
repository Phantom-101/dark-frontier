using System;
using System.Collections.Generic;
using DarkFrontier.Items.Conditions;
using UnityEngine;

#nullable enable
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

        public List<ItemTag> tags = new List<ItemTag>();

        public virtual ItemInstance NewState() => new ItemInstance(this);
    }
}
#nullable restore