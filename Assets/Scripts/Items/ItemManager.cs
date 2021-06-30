using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingletonBase<ItemManager> {
    [SerializeField] private List<ItemSO> _items;

    public ItemSO GetItem (string id) {
        return _items.Find (item => item.Id == id);
    }
}
