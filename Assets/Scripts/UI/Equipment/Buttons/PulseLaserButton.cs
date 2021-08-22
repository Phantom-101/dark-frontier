using DarkFrontier.Equipment;
using UnityEngine;
using UnityEngine.UI;

public class PulseLaserButton : EquipmentButton {
    public Button Button;
    public Image Icon;
    public Image Center;
    public Image Side;
    public Image Bottom;
    public Button DumpButton;
    public CanvasGroup TooltipGroup;
    public Text Tooltip;

    private PulseLaserSO cache;

    protected override void MultiInitialize () {
        if (Slot == null) return;
        cache = Slot.Data.Equipment as PulseLaserSO;
        Icon.sprite = cache.Icon;
        Tooltip.text = Slot.Data.Equipment.name;
        TooltipGroup.alpha = 0;
        Button.onClick.AddListener (() => cache.OnClicked (Slot));
        DumpButton.onClick.AddListener (() => (Slot.Data as PulseLaserSlotData).Charge = 0);
    }

    protected override void InternalTick (float dt) {
        if (Slot.Data.Equipment != cache) {
            Destroy (gameObject);
            return;
        }

        cache.EnsureDataType (Slot);
        PulseLaserSlotData data = Slot.Data as PulseLaserSlotData;

        Button.interactable = cache.CanClick (Slot);
        Center.fillAmount = data.Charge / cache.EnergyRequired;
        Side.fillAmount = cache.GetRangeMultiplier (Slot) / cache.PreemptiveRangeMultiplier.Evaluate (0);
        Bottom.fillAmount = data.Durability / cache.Durability;
    }
}
