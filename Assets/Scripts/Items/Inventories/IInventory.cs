using System.Collections.Generic;
using DarkFrontier.Structures;

namespace DarkFrontier.Items.Inventories {
    public interface IInventory {
        float UVolume { get; set; }
        float UStoredVolume { get; }
        bool UOverburdened { get; }
        int UPrecision { get; set; }

        int GetQuantity (ItemPrototype.State aItem);
        bool HasQuantity (ItemPrototype.State aItem, int aQuantity);
        int AddQuantity (ItemPrototype.State aItem, int aQuantity);
        int RemoveQuantity (ItemPrototype.State aItem, int aQuantity);
        List<ItemPrototype.State> GetStoredItems ();
    }
}
