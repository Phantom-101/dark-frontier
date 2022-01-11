using System.Linq;
using DarkFrontier.DataStructures;
using UnityEngine;

namespace DarkFrontier.Structures {
    public class StructureInventoryAdder : MonoBehaviour {
        [SerializeField] private ItemPrototypeToIntDictionary items;

        public void Run (Structure structure) {
            items.Keys.ToList ().ForEach (k => structure.uInventory.AddQuantity (k.NewState(), items[k]));
            Destroy (this);
        }
    }
}
