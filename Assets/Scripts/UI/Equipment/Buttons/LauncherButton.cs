using System.Collections.Generic;
using DarkFrontier.Equipment;
using DarkFrontier.Items.Prototypes;
using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.UI.Equipment.Buttons {
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

        public override void Enable () {
            base.Enable ();

            if (Slot == null) return;
            cache = Slot.Equipment as LauncherPrototype;
            Tooltip.text = Slot.Equipment.name;
            TooltipGroup.alpha = 0;
            Button.onClick.AddListener (() => cache.OnClicked (Slot));
            SwitchButton.onClick.AddListener (() => {
                List<MissileSO> candidates = cache.CompatibleMissiles.FindAll (e => Slot.Equipper.UInventory.HasQuantity (e, 1));
                LauncherPrototype.State state = Slot.UState as LauncherPrototype.State;
                if (candidates.Count == 0) state.Missile = null;
                else state.Missile = candidates[(candidates.IndexOf (state.Missile) + 1) % candidates.Count];
            });
        }

        public override void Tick (object aTicker, float aDt) {
            if (Slot.Equipment != cache) {
                Destroy (gameObject);
                return;
            }

            cache.EnsureStateType (Slot);
            LauncherPrototype.State state = Slot.UState as LauncherPrototype.State;

            Button.interactable = cache.CanClick (Slot);
            if (state.Missile == null) Icon.sprite = cache.Icon;
            else Icon.sprite = state.Missile.Icon;
            Center.fillAmount = state.Charge / cache.EnergyRequired;
            Side.fillAmount = cache.CompatibleMissiles.Contains (state.Missile) ? 1 : 0;
            Bottom.fillAmount = state.Durability / cache.Durability;
        }
    }
}