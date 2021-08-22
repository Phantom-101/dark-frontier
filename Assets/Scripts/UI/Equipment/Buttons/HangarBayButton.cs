using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DarkFrontier.Equipment {
    public class HangarBayButton : EquipmentButton {
        public Button Button;
        public EventTrigger EventTrigger;
        public Image Icon;
        public Image Center;
        public Image Side;
        public Image Bottom;
        public CanvasGroup TooltipGroup;
        public Text Tooltip;

        private HangarBaySO cache;

        protected override void MultiInitialize () {
            if (Slot == null) return;
            cache = Slot.Data.Equipment as HangarBaySO;
            Icon.sprite = cache.Icon;
            Tooltip.text = Slot.Data.Equipment.name;
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

        protected override void InternalTick (float dt) {
            if (Slot.Data.Equipment != cache) {
                Destroy (gameObject);
                return;
            }

            cache.EnsureDataType (Slot);
            HangarBaySlotData data = Slot.Data as HangarBaySlotData;

            Button.interactable = cache.CanClick (Slot);
            Center.fillAmount = (float) data.LaunchSlots.FindAll (s => s.State == HangarBayLaunchSlotState.Launched).Count / cache.MaxSquadronSize;
            Side.fillAmount = (float) data.LaunchSlots.FindAll (s => s.State == HangarBayLaunchSlotState.Loaded).Count / cache.MaxSquadronSize;
            Bottom.fillAmount = data.Durability / cache.Durability;
        }
    }
}