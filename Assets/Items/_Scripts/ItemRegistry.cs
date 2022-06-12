#nullable enable
using System;
using DarkFrontier.Game.Essentials;
using UnityEngine;

namespace DarkFrontier.Items._Scripts
{
    public class ItemRegistry : MonoBehaviour
    {
        public ItemPrototype[] items = Array.Empty<ItemPrototype>();

        public void Register(IdRegistry registry)
        {
            for(int i = 0, l = items.Length; i < l; i++)
            {
                registry.Register(items[i]);
            }
            Destroy(this);
        }
    }
}
