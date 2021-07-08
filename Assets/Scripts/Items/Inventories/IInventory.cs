using System.Collections.Generic;

public interface IInventory {
    float Volume { get; }
    float StoredVolume { get; }
    bool Overburdened { get; }
    int Precision { get; }

    int GetQuantity (ItemSO item);
    bool HasQuantity (ItemSO item, int quantity);
    int AddQuantity (ItemSO item, int quantity);
    int RemoveQuantity (ItemSO item, int quantity);
    List<ItemSO> GetStoredItems ();
}
