using UnityEngine;
using UnityEngine.UI;

public class PulseLaserIndicatorUI : EquipmentIndicatorUI {
    public Button Button;
    public Image Icon;
    public Image Center;
    public Image Side;
    public Image Bottom;
    public Button DumpButton;
    public CanvasGroup TooltipGroup;
    public Text Tooltip;

    private PulseLaserSO cache;

    public override void Initialize () {
        cache = Slot.Data.Equipment as PulseLaserSO;
        Icon.sprite = cache.Icon;
        Tooltip.text = Slot.Data.Equipment.name;
        TooltipGroup.alpha = 0;
        Button.onClick.AddListener (() => cache.OnClicked (Slot));
        DumpButton.onClick.AddListener (() => (Slot.Data as PulseLaserSlotData).Charge = 0);
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
