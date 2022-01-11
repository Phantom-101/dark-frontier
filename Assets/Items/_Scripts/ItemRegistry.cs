using System;
using UnityEngine;

#nullable enable
namespace DarkFrontier.Items._Scripts
{
    [Serializable]
    public class ItemRegistry
    {
        [SerializeField]
        private ItemPrototype[] prototypes;
        
        public ItemRegistry()
        {
            prototypes = Resources.FindObjectsOfTypeAll<ItemPrototype>();
        }

        public ItemRegistry(ItemPrototype[] prototypes)
        {
            this.prototypes = prototypes;
        }
        
        public ItemPrototype? Get(string id)
        {
            for (int i = 0, l = prototypes.Length; i < l; i++)
            {
                if (prototypes[i].id.Equals(id))
                {
                    return prototypes[i];
                }
            }

            return null;
        }
    }
}
#nullable restore