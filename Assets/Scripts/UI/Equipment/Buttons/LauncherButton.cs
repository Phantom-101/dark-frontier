using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.Equipment {
    public class LauncherButton : EquipmentButton {
        public Button Button;
        public Image Icon;
        public Image Center;
        public Image Side;
        public Image Bottom;
        public Button SwitchButton;
        public CanvasGroup TooltipGroup;
        public Text Tooltip;

        private LauncherPrototype cache;

        protected override void MultiInitialize () {
            if (Slot == null) return;
            cache = Slot.Equipment as LauncherPrototype;
            Tooltip.text = Slot.Equipment.name;
            TooltipGroup.alpha = 0;
            Button.onClick.AddListener (() => cache.OnClicked (Slot));
            SwitchButton.onClick.AddListener (() => {
                List<MissileSO> candidates = cache.CompatibleMissiles.FindAll (e => Slot.Equipper.Inventory.HasQuantity (e, 1));
                LauncherPrototype.State state = Slot.State as LauncherPrototype.State;
                if (candidates.Count == 0) state.Missile = null;
                else state.Missile = candidates[(candidates.IndexOf (state.Missile) + 1) % candidates.Count];
            });
        }

        protected override void InternalTick (float dt) {
            if (Slot.Equipment != cache) {
                Destroy (gameObject);
                return;
            }

            cache.EnsureStateType (Slot);
            LauncherPrototype.State state = Slot.State as LauncherPrototype.State;

            Button.interactable = cache.CanClick (Slot);
            if (state.Missile == null) Icon.sprite = cache.Icon;
            else Icon.sprite = state.Missile.Icon;
            Center.fillAmount = state.Charge / cache.EnergyRequired;
            Side.fillAmount = cache.CompatibleMissiles.Contains (state.Missile) ? 1 : 0;
            Bottom.fillAmount = state.Durability / cache.Durability;
        }
    }
}