using System;
using System.Collections.Generic;
using System.Linq;
using DarkFrontier.Controllers;
using DarkFrontier.Equipment;
using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.UI.Equipment.Buttons {
    public class EquipmentButtonManager : MonoBehaviour {
        [SerializeField] private Transform parent;

        private readonly Dictionary<EquipmentSlot, EquipmentButton> buttons = new Dictionary<EquipmentSlot, EquipmentButton> ();

        private readonly Lazy<PlayerController> iPlayerController = new Lazy<PlayerController>(() => Singletons.Get<PlayerController>(), false);
        
        private void Update () {
            Structure player = iPlayerController.Value.UPlayer;
            if (player == null) return;

            foreach (EquipmentSlot key in buttons.Keys.ToArray ()) {
                if (!player.UEquipment.UAll.Contains (key)) {
                    buttons.Remove (key);
                    Destroy (key.gameObject);
                }
            }

            foreach (EquipmentSlot slot in player.UEquipment.UAll)
                if (slot.Equipment != null && slot.Equipment.ButtonPrefab != null && !buttons.ContainsKey (slot))
                    buttons[slot] = null;

            foreach (EquipmentSlot key in buttons.Keys.ToArray ()) {
                if (key.Equipment != null && key.Equipment.ButtonPrefab != null) {
                    if (buttons[key] == null) {
                        GameObject indicator = Instantiate (key.Equipment.ButtonPrefab, parent);
                        EquipmentButton comp = indicator.GetComponent<EquipmentButton> ();
                        comp.Slot = key;
                        Singletons.Get<BehaviorManager> ().InitializeImmediately (comp);
                        Singletons.Get<BehaviorManager> ().EnableImmediately (comp);
                        buttons[key] = comp;
                    }
                }
            }
        }
    }
}