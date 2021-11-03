using DarkFrontier.Equipment;
using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.UI.Equipment.Buttons {
    public class BeamLaserButton : EquipmentButton {
        public Button Button;
        public Image Icon;
        public Image Center;
        public Image Side;
        public Image Bottom;
        public CanvasGroup TooltipGroup;
        public Text Tooltip;

        private BeamLaserPrototype cache;

        public override void Enable () {
            base.Enable ();

            if (Slot == null) return;
            cache = Slot.Equipment as BeamLaserPrototype;
            Icon.sprite = cache.Icon;
            Tooltip.text = Slot.Equipment.name;
            TooltipGroup.alpha = 0;
            Button.onClick.AddListener (() => cache.OnClicked (Slot));
        }

        public override void Tick (object aTicker, float aDt) {
            if (Slot.Equipment != cache) {
                Destroy (gameObject);
                return;
            }

            cache.EnsureStateType (Slot);
            BeamLaserPrototype.State state = Slot.UState as BeamLaserPrototype.State;

            Button.interactable = cache.CanClick (Slot);
            Center.fillAmount = state.AccumulatedDamageMultiplier / cache.DamageInterval;
            Side.fillAmount = state.Heat / cache.MaxHeat;
            Bottom.fillAmount = state.Durability / cache.Durability;
        }
    }
}