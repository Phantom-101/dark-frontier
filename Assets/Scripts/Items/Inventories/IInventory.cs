using System.Collections.Generic;

public interface IInventory {
    float Volume { get; set; }
    float StoredVolume { get; }
    bool Overburdened { get; }
    int Precision { get; set; }

    int GetQuantity (ItemPrototype item);
    bool HasQuantity (ItemPrototype item, int quantity);
    int AddQuantity (ItemPrototype item, int quantity);
    int RemoveQuantity (ItemPrototype item, int quantity);
    List<ItemPrototype> GetStoredItems ();
}
