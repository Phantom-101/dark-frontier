using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SingleInventory : IInventory, ISaveTo<SingleInventorySaveData> {
    public ItemSO Item { get => item; }
    [SerializeField] private ItemSO item;

    public int Quantity { get => quantity; }
    [SerializeField] private int quantity;

    public float Volume { get => volume; set => volume = value; }
    [SerializeField] private float volume;

    public float StoredVolume { get => storedVolume; }
    [SerializeField] private float storedVolume;
    public bool Overburdened { get => storedVolume > volume; }

    public int Precision { get => precision; set { precision = value; RecalculateStoredVolume (); } }
    [SerializeField] private int precision;

    public SingleInventory (float volume) : this (volume, 1) { }
    public SingleInventory (float volume, int precision) : this (null, 0, volume, precision) { }
    public SingleInventory (ItemSO item, int quantity, float volume, int precision) {
        this.item = item;
        this.quantity = quantity;
        this.volume = volume;
        this.precision = precision;
        RecalculateStoredVolume ();
    }
    public SingleInventory (SingleInventorySaveData saveData) {
        item = ItemManager.Instance.GetItem (saveData.ItemId);
        quantity = saveData.Quantity;
        volume = saveData.Volume;
        precision = saveData.Precision;
    }

    public int GetQuantity (ItemSO item) => item == this.item ? quantity : 0;
    public bool HasQuantity (ItemSO item, int quantity) => GetQuantity (item) >= quantity;

    public int AddQuantity (ItemSO item, int quantity) {
        if (Overburdened) return 0;
        Optimize ();
        if (this.item != null && this.item != item) return 0;
        float remainingVolume = RoundToPrecision (volume - storedVolume);
        int canFit = (int) (remainingVolume / item.Volume);
        int added = Math.Min (canFit, quantity);
        this.quantity += added;
        RecalculateStoredVolume ();
        return added;
    }

    public int RemoveQuantity (ItemSO item, int quantity) {
        Optimize ();
        if (this.item != null && this.item != item) return 0;
        int removed = Math.Min (this.quantity, quantity);
        this.quantity -= removed;
        RecalculateStoredVolume ();
        return removed;
    }

    public List<ItemSO> GetStoredItems () => item == null ? new List<ItemSO> () : new List<ItemSO> { item };

    private void Optimize () => item = quantity == 0 ? null : item;
    private float RoundToPrecision (float value) => (float) Math.Round (value, precision);

    private void RecalculateStoredVolume () {
        Optimize ();
        storedVolume = item == null ? 0 : RoundToPrecision (RoundToPrecision (item.Volume) * quantity);
    }

    public SingleInventorySaveData Save () {
        return new SingleInventorySaveData {
            ItemId = item.Id,
            Quantity = quantity,
            Volume = volume,
            Precision = precision,
        };
    }
}

[Serializable]
public class SingleInventorySaveData : ILoadTo<SingleInventory> {
    public string ItemId;
    public int Quantity;
    public float Volume;
    public int Precision;

    public SingleInventory Load () => new SingleInventory (this);
}