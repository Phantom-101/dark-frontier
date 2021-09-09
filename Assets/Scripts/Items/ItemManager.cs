using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingletonBase<ItemManager> {
    [SerializeField] private List<ItemPrototype> _items;

    public ItemPrototype GetItem (string id) {
        return _items.Find (item => item.Id == id);
    }
}
