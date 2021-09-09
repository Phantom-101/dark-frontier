using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.Equipment {
    public class HangarBayPanelLaunchable : MonoBehaviour {
        public HangarBayPrototype.State State;
        public HangarLaunchableSO Launchable;
        [SerializeField] private Button button;
        [SerializeField] private Image icon;
        [SerializeField] private Text quantity;

        private void Start () {
            button.onClick.AddListener (() => {
                List<HangarBayPrototype.State.SlotState> unloaded = State.LaunchSlots.FindAll (s => s.Status == HangarBayPrototype.State.SlotState.SlotStatus.Unloaded);
                if (unloaded.Count > 0) {
                    if (State.Slot.Equipper.Inventory.RemoveQuantity (Launchable, 1) == 1) {
                        unloaded[0].Load (Launchable);
                    }
                }
            });
        }

        private void Update () {
            if (State.Slot.Equipper == null || Launchable == null) {
                Destroy (gameObject);
                return;
            }
            int q = State.Slot.Equipper.Inventory.GetQuantity (Launchable);
            if (q == 0) {
                Destroy (gameObject);
                return;
            }
            icon.sprite = Launchable.Icon;
            quantity.text = $"x{q}";
        }
    }
}