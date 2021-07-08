using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CappedSingleInventory : IInventory, ISaveTo<CappedSingleInventorySaveData> {
    public ItemSO Item { get => item; }
    [SerializeField] private ItemSO item;

    public int Quantity { get => quantity; }
    [SerializeField] private int quantity;

    public int MaxQuantity { get => maxQuantity; }
    [SerializeField] private int maxQuantity;

    public float Volume { get => volume.AppliedValue; }
    [SerializeField] private Stat volume;

    public float StoredVolume { get => storedVolume; }
    [SerializeField] private float storedVolume;
    public bool Overburdened { get => storedVolume > volume.AppliedValue; }

    public int Precision { get => precision; }
    [SerializeField] private int precision;

    public CappedSingleInventory (int maxQuantity, float volume) : this (maxQuantity, volume, 1) { }
    public CappedSingleInventory (int maxQuantity, float volume, int precision) : this (maxQuantity, new Stat (StatNames.InventoryVolume, volume), precision) { }
    public CappedSingleInventory (int maxQuantity, Stat volume, int precision) : this (null, 0, maxQuantity, volume, precision) { }
    public CappedSingleInventory (ItemSO item, int quantity, int maxQuantity, Stat volume, int precision) {
        this.item = item;
        this.quantity = quantity;
        this.maxQuantity = maxQuantity;
        this.volume = volume;
        this.precision = precision;
        RecalculateStoredVolume ();
    }
    public CappedSingleInventory (CappedSingleInventorySaveData saveData) {
        item = ItemManager.Instance.GetItem (saveData.ItemId);
        quantity = saveData.Quantity;
        maxQuantity = saveData.MaxQuantity;
        volume = saveData.Volume.Load ();
        precision = saveData.Precision;
    }

    public int GetQuantity (ItemSO item) => item == this.item ? quantity : 0;
    public bool HasQuantity (ItemSO item, int quantity) => GetQuantity (item) >= quantity;

    public int AddQuantity (ItemSO item, int quantity) {
        if (Overburdened) return 0;
        Optimize ();
        if (this.item != null && this.item != item) return 0;
        float remainingVolume = RoundToPrecision (volume.AppliedValue - storedVolume);
        int canFit = (int) (remainingVolume / item.Volume);
        int remainingQuantity = maxQuantity - this.quantity;
        int added = Math.Min (canFit, Math.Min (remainingQuantity, quantity));
        this.item = item;
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

    public CappedSingleInventorySaveData Save () {
        return new CappedSingleInventorySaveData {
            ItemId = item.Id,
            Quantity = quantity,
            MaxQuantity = maxQuantity,
            Volume = volume.Save (),
            Precision = precision,
        };
    }
}

[Serializable]
public class CappedSingleInventorySaveData : ILoadTo<CappedSingleInventory> {
    public string ItemId;
    public int Quantity;
    public int MaxQuantity;
    public StatSaveData Volume;
    public int Precision;

    public CappedSingleInventory Load () => new CappedSingleInventory (this);
}
