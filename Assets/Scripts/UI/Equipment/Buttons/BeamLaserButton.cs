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

        private BeamLaserSO cache;

        protected override void MultiInitialize () {
            if (Slot == null) return;
            cache = Slot.Data.Equipment as BeamLaserSO;
            Icon.sprite = cache.Icon;
            Tooltip.text = Slot.Data.Equipment.name;
            TooltipGroup.alpha = 0;
            Button.onClick.AddListener (() => cache.OnClicked (Slot));
        }

        protected override void InternalTick (float dt) {
            if (Slot.Data.Equipment != cache) {
                Destroy (gameObject);
                return;
            }

            cache.EnsureDataType (Slot);
            BeamLaserSlotData data = Slot.Data as BeamLaserSlotData;

            Button.interactable = cache.CanClick (Slot);
            Center.fillAmount = data.AccumulatedDamageMultiplier / cache.DamageInterval;
            Side.fillAmount = data.Heat / cache.MaxHeat;
            Bottom.fillAmount = data.Durability / cache.Durability;
        }
    }
}