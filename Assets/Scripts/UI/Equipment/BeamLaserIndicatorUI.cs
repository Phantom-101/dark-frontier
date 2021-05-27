﻿using UnityEngine;
using UnityEngine.UI;

public class BeamLaserIndicatorUI : EquipmentIndicatorUI {
    public Button Button;
    public Image Icon;
    public Image Center;
    public Image Side;
    public Image Bottom;
    public CanvasGroup TooltipGroup;
    public Text Tooltip;

    private BeamLaserSO cache;

    public override void Initialize () {
        cache = Slot.Data.Equipment as BeamLaserSO;
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

        BeamLaserSlotData data = Slot.Data as BeamLaserSlotData;
        Button.interactable = cache.CanClick (Slot);
        Center.fillAmount = data.AccumulatedDamageMultiplier / cache.DamageInterval;
        Side.fillAmount = data.Heat / cache.MaxHeat;
        Bottom.fillAmount = data.Durability / cache.Durability;
    }
}