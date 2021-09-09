using DarkFrontier.Foundation.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Inventory : IInventory, ISaveTo<InventorySaveData> {
    [SerializeField] private ItemSOToIntDictionary quantities;

    public float Volume { get => volume; set => volume = value; }
    [SerializeField] private float volume;

    public float StoredVolume { get => storedVolume; }
    [SerializeField] private float storedVolume;
    public bool Overburdened { get => storedVolume > volume; }

    public int Precision { get => precision; set { precision = value; RecalculateStoredVolume (); } }
    [SerializeField] private int precision;

    public Inventory (float volume) : this (volume, 1) { }
    public Inventory (float volume, int precision) : this (new ItemSOToIntDictionary (), volume, precision) { }
    public Inventory (ItemSOToIntDictionary quantities, float volume, int precision) {
        this.quantities = quantities;
        this.volume = volume;
        this.precision = precision;
        RecalculateStoredVolume ();
    }
    public Inventory (InventorySaveData saveData) {
        quantities = saveData.Quantities.ToDictionary (p => ItemManager.Instance.GetItem (p.Key), p => p.Value).ToSerializable<ItemPrototype, int, ItemSOToIntDictionary> ();
        volume = saveData.Volume;
        precision = saveData.Precision;
    }

    public int GetQuantity (ItemPrototype item) => quantities.TryGet (item, 0);
    public bool HasQuantity (ItemPrototype item, int quantity) => GetQuantity (item) >= quantity;

    public int AddQuantity (ItemPrototype item, int quantity) {
        if (Overburdened) return 0;
        float remainingVolume = RoundToPrecision (volume - storedVolume);
        int canFit = (int) (remainingVolume / item.Volume);
        int added = Math.Min (canFit, quantity);
        quantities[item] = GetQuantity (item) + added;
        RecalculateStoredVolume ();
        return added;
    }

    public int RemoveQuantity (ItemPrototype item, int quantity) {
        int has = GetQuantity (item);
        int removed = Math.Min (has, quantity);
        quantities[item] = has - removed;
        RecalculateStoredVolume ();
        return removed;
    }

    public List<ItemPrototype> GetStoredItems () => quantities.Keys.ToList ();

    private void Optimize () => quantities.Where (pair => pair.Value == 0).ToList ().ForEach (pair => quantities.Remove (pair.Key));
    private float RoundToPrecision (float value) => (float) Math.Round (value, precision);
    private float GetVolume (ItemPrototype item, int amount) => RoundToPrecision (RoundToPrecision (item.Volume) * amount);

    private void RecalculateStoredVolume () {
        storedVolume = 0;
        Optimize ();
        foreach (KeyValuePair<ItemPrototype, int> pair in quantities) {
            storedVolume = RoundToPrecision (storedVolume + GetVolume (pair.Key, pair.Value));
        }
    }

    public InventorySaveData Save () {
        return new InventorySaveData {
            Quantities = quantities.ToDictionary (p => p.Key.Id, p => p.Value),
            Volume = volume,
            Precision = precision,
        };
    }
}

[Serializable]
public class InventorySaveData : ILoadTo<Inventory> {
    public Dictionary<string, int> Quantities;
    public float Volume;
    public int Precision;

    public Inventory Load () => new Inventory (this);
}