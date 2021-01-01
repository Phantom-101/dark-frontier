using UnityEngine;

public class StructureInventoryAdder : MonoBehaviour {

    [SerializeField] private Structure _structure;
    [SerializeField] private ItemSO[] _items;
    [SerializeField] private int[] _quantities;
    [SerializeField] private bool _add;

    private void Update () {

        if (_add) {

            for (int i = 0; i < _items.Length; i++) {

                if (_structure.CanAddInventoryItem (_items[i], _quantities[i])) _structure.ChangeInventoryCount (_items[i], _quantities[i]);

            }

            _add = false;

        }

    }

}
