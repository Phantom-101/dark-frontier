using System.Collections.Generic;
using DarkFrontier.Equipment;
using DarkFrontier.Items.Prototypes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DarkFrontier.UI.Equipment.Panels {
    public class HangarBayPanelLaunchable : MonoBehaviour {
        public HangarBayPrototype.State State;
        public HangarLaunchableSO Launchable;
        public EventTrigger EventTrigger;
        [SerializeField] private Image icon;
        [SerializeField] private Text quantity;

        private void Start () {
            EventTrigger.Entry clickEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
            clickEntry.callback.AddListener ((data) => {
                List<HangarBayPrototype.State.SlotState> unloaded = State.LaunchSlots.FindAll (s => s.Status == HangarBayPrototype.State.SlotState.SlotStatus.Unloaded);
                if (unloaded.Count > 0) {
                    if (State.Slot.Equipper.UInventory.RemoveQuantity (Launchable, 1) == 1) {
                        unloaded[0].Load (Launchable);
                    }
                }
            });
            EventTrigger.triggers.Add (clickEntry);

            EventTrigger.Entry beginDragEntry = new EventTrigger.Entry { eventID = EventTriggerType.BeginDrag };
            beginDragEntry.callback.AddListener ((data) => {
                List<HangarBayPrototype.State.SlotState> unloaded = State.LaunchSlots.FindAll (s => s.Status == HangarBayPrototype.State.SlotState.SlotStatus.Unloaded);
                int q = State.Slot.Equipper.UInventory.GetQuantity (Launchable);
                for (int i = 0; i < q && i < unloaded.Count; i++) {
                    if (State.Slot.Equipper.UInventory.RemoveQuantity (Launchable, 1) == 1) {
                        unloaded[i].Load (Launchable);
                    }
                }
            });
            EventTrigger.triggers.Add (beginDragEntry);
        }

        private void Update () {
            if (State.Slot.Equipper == null || Launchable == null) {
                Destroy (gameObject);
                return;
            }
            int q = State.Slot.Equipper.UInventory.GetQuantity (Launchable);
            if (q == 0) {
                Destroy (gameObject);
                return;
            }
            icon.sprite = Launchable.Icon;
            quantity.text = $"x{q}";
        }
    }
}