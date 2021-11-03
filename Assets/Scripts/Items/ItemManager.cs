using System.Collections.Generic;
using DarkFrontier.Foundation;
using DarkFrontier.Items.Prototypes;
using UnityEngine;

namespace DarkFrontier.Items {
    public class ItemManager : SingletonBase<ItemManager> {
        [SerializeField] private List<ItemPrototype> _items;

        public ItemPrototype GetItem (string id) {
            return _items.Find (item => item.Id == id);
        }
    }
}
