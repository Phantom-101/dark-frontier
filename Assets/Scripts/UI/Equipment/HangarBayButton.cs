using UnityEngine;
using UnityEngine.UI;

public class HangarBayButton : EquipmentButton {
    public Button Button;
    public Image Icon;
    public Image Center;
    public Image Side;
    public Image Bottom;
    public CanvasGroup TooltipGroup;
    public Text Tooltip;

    private HangarBaySO cache;

    public override void Initialize () {
        cache = Slot.Data.Equipment as HangarBaySO;
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

        HangarBaySlotData data = Slot.Data as HangarBaySlotData;
        Button.interactable = cache.CanClick (Slot);
        Center.fillAmount = (float) data.ActiveCrafts.Count / cache.MaxSquadronSize;
        Side.fillAmount = (float) data.StowedCrafts.Quantity / cache.MaxSquadronSize;
        Bottom.fillAmount = data.Durability / cache.Durability;
    }
}
