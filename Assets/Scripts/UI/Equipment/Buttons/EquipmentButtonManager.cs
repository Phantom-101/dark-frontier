using DarkFrontier.Equipment;
using DarkFrontier.Structures;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentButtonManager : MonoBehaviour {
    [SerializeField] private Transform parent;

    private readonly Dictionary<EquipmentSlot, EquipmentButton> buttons = new Dictionary<EquipmentSlot, EquipmentButton> ();

    private void Update () {
        Structure player = PlayerController.Instance.Player;
        if (player == null) return;

        foreach (EquipmentSlot key in buttons.Keys.ToArray ()) {
            if (!player.Equipment.Contains (key)) {
                buttons.Remove (key);
                Destroy (key.gameObject);
            }
        }

        foreach (EquipmentSlot slot in player.Equipment)
            if (slot.Data.Equipment != null && slot.Data.Equipment.ButtonPrefab != null && !buttons.ContainsKey (slot))
                buttons[slot] = null;

        foreach (EquipmentSlot key in buttons.Keys.ToArray ()) {
            if (key.Data.Equipment != null && key.Data.Equipment.ButtonPrefab != null) {
                if (buttons[key] == null) {
                    GameObject indicator = Instantiate (key.Data.Equipment.ButtonPrefab, parent);
                    EquipmentButton comp = indicator.GetComponent<EquipmentButton> ();
                    comp.Slot = key;
                    comp.TryInitialize ();
                    buttons[key] = comp;
                }
            }
        }
    }
}
