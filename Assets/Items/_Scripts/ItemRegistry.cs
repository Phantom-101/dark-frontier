#nullable enable
using System;
using UnityEditor;
using UnityEngine;

namespace DarkFrontier.Items._Scripts
{
    [Serializable]
    public class ItemRegistry
    {
        [SerializeField]
        private ItemPrototype[] prototypes;
        
        public ItemRegistry()
        {
            var ids = AssetDatabase.FindAssets($"t: {typeof(ItemPrototype)}");
            var l = ids.Length;
            prototypes = new ItemPrototype[l];
            for(var i = 0; i < l; i++)
            {
                prototypes[i] = AssetDatabase.LoadAssetAtPath<ItemPrototype>(AssetDatabase.GUIDToAssetPath(ids[i]));
            }
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
