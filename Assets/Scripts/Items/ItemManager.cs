using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    [SerializeField] private List<ItemSO> _items;

    private static ItemManager _instance;

    private void Awake () {
        _instance = this;
    }

    public ItemSO GetItem (string id) {

        ItemSO found = null;
        _items.ForEach (item => {

            if (item.Id == id) found = item;

        });

        return found;

    }

    public static ItemManager GetInstance () { return _instance; }

}
