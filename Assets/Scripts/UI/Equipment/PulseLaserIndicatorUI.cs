using UnityEngine;
using UnityEngine.UI;

public class PulseLaserIndicatorUI : EquipmentIndicatorUI {
    public Button Button;
    public Image Icon;
    public Image Center;
    public Image Side;
    public Image Bottom;
    public CanvasGroup TooltipGroup;
    public Text Tooltip;

    private PulseLaserSO cache;

    public override void Initialize () {
        cache = Slot.Data.Equipment as PulseLaserSO;
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

        PulseLaserSlotData data = Slot.Data as PulseLaserSlotData;
        Button.interactable = cache.CanClick (Slot);
        Center.fillAmount = data.Charge / cache.EnergyRequired;
        Side.fillAmount = cache.GetRangeMultiplier (Slot) / cache.PreemptiveRangeMultiplier.Evaluate (0);
        Bottom.fillAmount = data.Durability / cache.Durability;
    }
}
