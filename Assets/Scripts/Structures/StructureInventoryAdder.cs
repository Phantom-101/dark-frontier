using System.Linq;
using UnityEngine;

public class StructureInventoryAdder : MonoBehaviour {
    [SerializeField] private ItemSOToIntDictionary items;

    public void Run (Structure structure) {
        items.Keys.ToList ().ForEach (k => structure.Inventory.AddQuantity (k, items[k]));
        Destroy (this);
    }
}
