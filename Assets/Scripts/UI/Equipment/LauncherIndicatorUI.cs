using UnityEngine;
using UnityEngine.UI;

public class LauncherIndicatorUI : EquipmentIndicatorUI {
    public Button Button;
    public Image Icon;
    public Image Center;
    public Image Side;
    public Image Bottom;
    public CanvasGroup TooltipGroup;
    public Text Tooltip;

    private LauncherSO cache;

    public override void Initialize () {
        cache = Slot.Data.Equipment as LauncherSO;
        Icon.sprite = cache.Icon;
        Tooltip.text = Slot.Data.Equipment.name;
        TooltipGroup.alpha = 0;
        Button.onClick.AddListener (() => cache.OnClicked (Slot));
    }

    private void Update () {
        if (Slot.Data.Equipment != cache) {
            Destroy (gameObject);
            return;
        }

        LauncherSlotData data = Slot.Data as LauncherSlotData;
        Button.interactable = cache.CanClick (Slot);
        Center.fillAmount = data.Charge / cache.EnergyRequired;
        Side.fillAmount = cache.CompatibleMissiles.Contains (data.Missile) ? 1 : 0;
        Bottom.fillAmount = data.Durability / cache.Durability;
    }
}
