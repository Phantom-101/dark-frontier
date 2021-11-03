using System.Linq;
using DarkFrontier.DataStructures;
using UnityEngine;

namespace DarkFrontier.Structures {
    public class StructureInventoryAdder : MonoBehaviour {
        [SerializeField] private ItemSOToIntDictionary items;

        public void Run (Structure structure) {
            items.Keys.ToList ().ForEach (k => structure.UInventory.AddQuantity (k, items[k]));
            Destroy (this);
        }
    }
}
