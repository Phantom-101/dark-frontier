using System;
using System.Collections.Generic;
using System.Linq;
using DarkFrontier.DataStructures;
using DarkFrontier.Foundation.Extensions;
using DarkFrontier.Serialization;
using UnityEngine;

namespace DarkFrontier.Items.Inventories {
    [Serializable]
    public class Inventory : IInventory, ISaveTo<InventorySaveData> {
        [SerializeField] private ItemPrototypeStateToIntDictionary quantities;

        public float UVolume { get => volume; set => volume = value; }
        [SerializeField] private float volume;

        public float UStoredVolume => storedVolume;
        [SerializeField] private float storedVolume;
        public bool UOverburdened => storedVolume > volume;

        public int UPrecision { get => precision; set { precision = value; RecalculateStoredVolume (); } }
        [SerializeField] private int precision;

        public Inventory (float volume) : this (volume, 1) { }
        public Inventory (float volume, int precision) : this (new ItemPrototypeStateToIntDictionary (), volume, precision) { }
        public Inventory (ItemPrototypeStateToIntDictionary quantities, float volume, int precision) {
            this.quantities = quantities;
            this.volume = volume;
            this.precision = precision;
            RecalculateStoredVolume ();
        }
        public Inventory (InventorySaveData saveData) {
            quantities = saveData.Quantities.ToDictionary (p => ItemManager.Instance.GetItem (p.Key).NewState(), p => p.Value).ToSerializable<ItemPrototype.State, int, ItemPrototypeStateToIntDictionary> ();
            volume = saveData.Volume;
            precision = saveData.Precision;
        }

        public int GetQuantity (ItemPrototype.State aItem) => quantities.TryGet (aItem, 0);
        public bool HasQuantity (ItemPrototype.State aItem, int aQuantity) => GetQuantity (aItem) >= aQuantity;

        public int AddQuantity (ItemPrototype.State aItem, int aQuantity) {
            if (UOverburdened) return 0;
            float remainingVolume = RoundToPrecision (volume - storedVolume);
            int canFit = (int) (remainingVolume / aItem.uPrototype.Volume);
            int added = Math.Min (canFit, aQuantity);
            quantities[aItem] = GetQuantity (aItem) + added;
            RecalculateStoredVolume ();
            return added;
        }

        public int RemoveQuantity (ItemPrototype.State aItem, int aQuantity) {
            int has = GetQuantity (aItem);
            int removed = Math.Min (has, aQuantity);
            quantities[aItem] = has - removed;
            RecalculateStoredVolume ();
            return removed;
        }

        public List<ItemPrototype.State> GetStoredItems () => quantities.Keys.ToList ();

        private void Optimize () => quantities.Where (aPair => aPair.Value == 0).ToList ().ForEach (pair => quantities.Remove (pair.Key));
        private float RoundToPrecision (float aValue) => (float) Math.Round (aValue, precision);
        private float GetVolume (ItemPrototype.State aItem, int aAmount) => RoundToPrecision (RoundToPrecision (aItem.uPrototype.Volume) * aAmount);

        private void RecalculateStoredVolume () {
            storedVolume = 0;
            Optimize ();
            foreach (KeyValuePair<ItemPrototype.State, int> lPair in quantities) {
                storedVolume = RoundToPrecision (storedVolume + GetVolume (lPair.Key, lPair.Value));
            }
        }

        public InventorySaveData Save () {
            return new InventorySaveData {
                Quantities = quantities.ToDictionary (aPair => aPair.Key.uPrototype.Id, p => p.Value),
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
}