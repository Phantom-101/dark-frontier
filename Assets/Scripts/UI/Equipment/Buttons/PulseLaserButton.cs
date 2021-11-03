using DarkFrontier.Equipment;
using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.UI.Equipment.Buttons {
    public class PulseLaserButton : EquipmentButton {
        public Button Button;
        public Image Icon;
        public Image Center;
        public Image Side;
        public Image Bottom;
        public Button DumpButton;
        public CanvasGroup TooltipGroup;
        public Text Tooltip;

        private PulseLaserPrototype cache;

        public override void Enable () {
            base.Enable ();

            if (Slot == null) return;
            cache = Slot.Equipment as PulseLaserPrototype;
            Icon.sprite = cache.Icon;
            Tooltip.text = Slot.Equipment.name;
            TooltipGroup.alpha = 0;
            Button.onClick.AddListener (() => cache.OnClicked (Slot));
            DumpButton.onClick.AddListener (() => (Slot.UState as PulseLaserPrototype.State).Charge = 0);
        }

        public override void Tick (object aTicker, float aDt) {
            if (Slot.Equipment != cache) {
                Destroy (gameObject);
                return;
            }

            cache.EnsureStateType (Slot);
            PulseLaserPrototype.State state = Slot.UState as PulseLaserPrototype.State;

            Button.interactable = cache.CanClick (Slot);
            Center.fillAmount = state.Charge / cache.EnergyRequired;
            Side.fillAmount = cache.GetRangeMultiplier (Slot) / cache.PreemptiveRangeMultiplier.Evaluate (0);
            Bottom.fillAmount = state.Durability / cache.Durability;
        }
    }
}