using System.Collections.Generic;
using DarkFrontier.Items.Conditions;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.Items.Prototypes {
    [CreateAssetMenu (menuName = "Items/Item")]
    public class ItemPrototype : ScriptableObject {
        public string Name;
        [TextArea] public string Description;
        public string Id;
        public Sprite Icon;
        public float Volume;
        public List<ItemTag> Tags;
        public bool Usable;

        public virtual void OnUse (Structure user) {
            if (!Usable) return;
        }

        public virtual bool CanUse () {
            return Usable;
        }
    }
}

