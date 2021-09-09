using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.Equipment {
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

        protected override void MultiInitialize () {
            if (Slot == null) return;
            cache = Slot.Equipment as PulseLaserPrototype;
            Icon.sprite = cache.Icon;
            Tooltip.text = Slot.Equipment.name;
            TooltipGroup.alpha = 0;
            Button.onClick.AddListener (() => cache.OnClicked (Slot));
            DumpButton.onClick.AddListener (() => (Slot.State as PulseLaserPrototype.State).Charge = 0);
        }

        protected override void InternalTick (float dt) {
            if (Slot.Equipment != cache) {
                Destroy (gameObject);
                return;
            }

            cache.EnsureStateType (Slot);
            PulseLaserPrototype.State state = Slot.State as PulseLaserPrototype.State;

            Button.interactable = cache.CanClick (Slot);
            Center.fillAmount = state.Charge / cache.EnergyRequired;
            Side.fillAmount = cache.GetRangeMultiplier (Slot) / cache.PreemptiveRangeMultiplier.Evaluate (0);
            Bottom.fillAmount = state.Durability / cache.Durability;
        }
    }
}