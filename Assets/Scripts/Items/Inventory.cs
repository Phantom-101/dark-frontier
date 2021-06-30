using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Inventory {
    [SerializeField]
    private ItemSOToIntDictionary _quantities;
    [SerializeField]
    private Stat _volume;
    public float Volume { get => _volume.AppliedValue; }
    [SerializeField]
    private int _precision;
    public int Precision { get => _precision; }
    [SerializeField]
    private float _storedVolume;
    public float StoredVolume { get => _storedVolume; }
    public bool Overburdened { get => _storedVolume > _volume.AppliedValue; }

    public Inventory (float size) : this (size, 1) { }
    public Inventory (float size, int precision) : this (new Stat { BaseValue = size }, precision) { }
    public Inventory (Stat size, int precision) : this (new ItemSOToIntDictionary (), size, precision) { }
    public Inventory (ItemSOToIntDictionary quantities, Stat size, int precision) {
        _quantities = quantities;
        _volume = size;
        _precision = precision;
        RecalculateStoredVolume ();
    }

    public int GetQuantity (ItemSO item) => _quantities.TryGet (item, 0);
    public bool HasQuantity (ItemSO item, int quantity) => GetQuantity (item) >= quantity;

    public int AddQuantity (ItemSO item, int quantity) {
        if (Overburdened) return quantity;
        float remainingVolume = RoundToPrecision (_volume.AppliedValue - _storedVolume);
        int canFit = (int) (remainingVolume / item.Volume);
        int added = Math.Min (canFit, quantity);
        _quantities[item] = GetQuantity (item) + added;
        RecalculateStoredVolume ();
        return quantity - added;
    }

    public int RemoveQuantity (ItemSO item, int quantity) {
        int has = GetQuantity (item);
        int removed = Math.Min (has, quantity);
        _quantities[item] = has - removed;
        RecalculateStoredVolume ();
        return removed;
    }

    private void Optimize () => _quantities.Where (pair => pair.Value == 0).ToList ().ForEach (pair => _quantities.Remove (pair.Key));
    private float RoundToPrecision (float value) => (float) Math.Round (value, _precision);
    private float GetVolume (ItemSO item, int amount) => RoundToPrecision (RoundToPrecision (item.Volume) * amount);

    private void RecalculateStoredVolume () {
        _storedVolume = 0;
        Optimize ();
        foreach (KeyValuePair<ItemSO, int> pair in _quantities) {
            _storedVolume = RoundToPrecision (_storedVolume + GetVolume (pair.Key, pair.Value));
        }
    }
}
