using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LauncherIndicatorUI : EquipmentIndicatorUI {
    public Button Button;
    public Image Icon;
    public Image Center;
    public Image Side;
    public Image Bottom;
    public Button SwitchButton;
    public CanvasGroup TooltipGroup;
    public Text Tooltip;

    private LauncherSO cache;

    public override void Initialize () {
        cache = Slot.Data.Equipment as LauncherSO;
        Tooltip.text = Slot.Data.Equipment.name;
        TooltipGroup.alpha = 0;
        Button.onClick.AddListener (() => cache.OnClicked (Slot));
        SwitchButton.onClick.AddListener (() => {
            List<MissileSO> candidates = cache.CompatibleMissiles.FindAll (e => Slot.Equipper.HasInventoryCount (e, 1));
            LauncherSlotData data = Slot.Data as LauncherSlotData;
            if (candidates.Count == 0) data.Missile = null;
            else data.Missile = candidates[(candidates.IndexOf (data.Missile) + 1) % candidates.Count];
        });
    }

    private void Update () {
        if (Slot.Data.Equipment != cache) {
            Destroy (gameObject);
            return;
        }

        LauncherSlotData data = Slot.Data as LauncherSlotData;
        Button.interactable = cache.CanClick (Slot);
        if (data.Missile == null) Icon.sprite = cache.Icon;
        else Icon.sprite = data.Missile.Icon;
        Center.fillAmount = data.Charge / cache.EnergyRequired;
        Side.fillAmount = cache.CompatibleMissiles.Contains (data.Missile) ? 1 : 0;
        Bottom.fillAmount = data.Durability / cache.Durability;
    }
}
