using DarkFrontier.Equipment;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LauncherButton : EquipmentButton {
    public Button Button;
    public Image Icon;
    public Image Center;
    public Image Side;
    public Image Bottom;
    public Button SwitchButton;
    public CanvasGroup TooltipGroup;
    public Text Tooltip;

    private LauncherSO cache;

    protected override void MultiInitialize () {
        if (Slot == null) return;
        cache = Slot.Data.Equipment as LauncherSO;
        Tooltip.text = Slot.Data.Equipment.name;
        TooltipGroup.alpha = 0;
        Button.onClick.AddListener (() => cache.OnClicked (Slot));
        SwitchButton.onClick.AddListener (() => {
            List<MissileSO> candidates = cache.CompatibleMissiles.FindAll (e => Slot.Equipper.Inventory.HasQuantity (e, 1));
            LauncherSlotData data = Slot.Data as LauncherSlotData;
            if (candidates.Count == 0) data.Missile = null;
            else data.Missile = candidates[(candidates.IndexOf (data.Missile) + 1) % candidates.Count];
        });
    }

    protected override void InternalTick (float dt) {
        if (Slot.Data.Equipment != cache) {
            Destroy (gameObject);
            return;
        }

        cache.EnsureDataType (Slot);
        LauncherSlotData data = Slot.Data as LauncherSlotData;

        Button.interactable = cache.CanClick (Slot);
        if (data.Missile == null) Icon.sprite = cache.Icon;
        else Icon.sprite = data.Missile.Icon;
        Center.fillAmount = data.Charge / cache.EnergyRequired;
        Side.fillAmount = cache.CompatibleMissiles.Contains (data.Missile) ? 1 : 0;
        Bottom.fillAmount = data.Durability / cache.Durability;
    }
}
