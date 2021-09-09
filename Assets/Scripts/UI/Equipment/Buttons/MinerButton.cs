using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.Equipment {
    public class MinerButton : EquipmentButton {
        public Button Button;
        public Image Icon;
        public Image Center;
        public Image Side;
        public Image Bottom;
        public CanvasGroup TooltipGroup;
        public Text Tooltip;

        private MinerPrototype cache;

        protected override void MultiInitialize () {
            if (Slot == null) return;
            cache = Slot.Equipment as MinerPrototype;
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
            MinerPrototype.State state = Slot.State as MinerPrototype.State;

            Button.interactable = cache.CanClick (Slot);
            Center.fillAmount = state.AccumulatedDamageMultiplier / cache.DamageInterval;
            Side.fillAmount = state.Heat / cache.MaxHeat;
            Bottom.fillAmount = state.Durability / cache.Durability;
        }
    }
}