using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.Equipment {
    public class BeamLaserButton : EquipmentButton {
        public Button Button;
        public Image Icon;
        public Image Center;
        public Image Side;
        public Image Bottom;
        public CanvasGroup TooltipGroup;
        public Text Tooltip;

        private BeamLaserPrototype cache;

        protected override void MultiInitialize () {
            if (Slot == null) return;
            cache = Slot.Equipment as BeamLaserPrototype;
            Icon.sprite = cache.Icon;
            Tooltip.text = Slot.Equipment.name;
            TooltipGroup.alpha = 0;
            Button.onClick.AddListener (() => cache.OnClicked (Slot));
        }

        protected override void InternalTick (float dt) {
            if (Slot.Equipment != cache) {
                Destroy (gameObject);
                return;
            }

            cache.EnsureStateType (Slot);
            BeamLaserPrototype.State state = Slot.State as BeamLaserPrototype.State;

            Button.interactable = cache.CanClick (Slot);
            Center.fillAmount = state.AccumulatedDamageMultiplier / cache.DamageInterval;
            Side.fillAmount = state.Heat / cache.MaxHeat;
            Bottom.fillAmount = state.Durability / cache.Durability;
        }
    }
}