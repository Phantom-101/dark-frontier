using DarkFrontier.Equipment;
using DarkFrontier.UI.Equipment.Panels;
using DarkFrontier.UI.States;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DarkFrontier.UI.Equipment.Buttons {
    public class HangarBayButton : EquipmentButton {
        public Button Button;
        public EventTrigger EventTrigger;
        public Image Icon;
        public Image Center;
        public Image Side;
        public Image Bottom;
        public CanvasGroup TooltipGroup;
        public Text Tooltip;

        private HangarBayPrototype cache;

        public override void Enable () {
            base.Enable ();

            if (Slot == null) return;
            cache = Slot.Equipment as HangarBayPrototype;
            Icon.sprite = cache.Icon;
            Tooltip.text = Slot.Equipment.name;
            TooltipGroup.alpha = 0;
            EventTrigger.Entry clickEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
            clickEntry.callback.AddListener ((data) => cache.OnClicked (Slot));
            EventTrigger.triggers.Add (clickEntry);
            EventTrigger.Entry beginDragEntry = new EventTrigger.Entry { eventID = EventTriggerType.BeginDrag };
            beginDragEntry.callback.AddListener ((data) => {
                GameObject instantiated = UIStateManager.Instance.AddStateFromPrefab (cache.PanelPrefab);
                HangarBayPanel panel = instantiated.GetComponent<HangarBayPanel> ();
                panel.Slot = Slot;
                panel.Initialize ();
            });
            EventTrigger.triggers.Add (beginDragEntry);
        }

        public override void Tick (object aTicker, float aDt) {
            if (Slot.Equipment != cache) {
                Destroy (gameObject);
                return;
            }

            cache.EnsureStateType (Slot);
            HangarBayPrototype.State data = Slot.UState as HangarBayPrototype.State;

            Button.interactable = cache.CanClick (Slot);
            Center.fillAmount = (float) data.LaunchSlots.FindAll (s => s.Status == HangarBayPrototype.State.SlotState.SlotStatus.Launched).Count / cache.MaxSquadronSize;
            Side.fillAmount = (float) data.LaunchSlots.FindAll (s => s.Status == HangarBayPrototype.State.SlotState.SlotStatus.Loaded).Count / cache.MaxSquadronSize;
            Bottom.fillAmount = data.Durability / cache.Durability;
        }
    }
}