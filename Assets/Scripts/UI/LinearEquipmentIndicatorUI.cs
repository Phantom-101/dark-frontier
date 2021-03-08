using UnityEngine;
using UnityEngine.UI;

public class LinearEquipmentIndicatorUI : EquipmentIndicatorUI {

    [SerializeField] protected CanvasGroup _group;
    [SerializeField] protected Image _energy;
    [SerializeField] protected float _energyMax;
    [SerializeField] protected Image _durability;
    [SerializeField] protected float _durabilityMax;
    [SerializeField] protected Image _charges;
    [SerializeField] protected float _chargesMax;
    [SerializeField] protected Image _icon;
    [SerializeField] protected Button _button;
    [SerializeField] protected Text _tooltip;
    [SerializeField] protected CanvasGroup _tooltipGroup;
    [SerializeField] protected float _curAlpha = -1;

    private void Start () {

        _tooltip.text = "";

    }

    private void Update () {

        if (Slot == null) return;

        if (Slot.Equipment == null) {

            if (_curAlpha != 0.25f) {

                _curAlpha = 0.25f;
                TweenToCurAlpha ();

            }

            return;

        }

        if (_curAlpha != 1) {

            _curAlpha = 1;
            TweenToCurAlpha ();

        }

        float energyFill = Mathf.Clamp (Slot.Energy / _slot.Equipment.EnergyStorage * _energyMax, 0, _energyMax);
        if (float.IsNaN (energyFill)) energyFill = _energyMax;
        _energy.fillAmount = energyFill;

        float durabilityFill = Mathf.Clamp (Slot.Durability / _slot.Equipment.Durability * _durabilityMax, 0, _durabilityMax);
        if (float.IsNaN (durabilityFill)) durabilityFill = _durabilityMax;
        _durability.fillAmount = durabilityFill;

        float chargeFill = Slot.Equipper.GetInventoryCount (Slot.Charge) > 0 ? 1 : 0;
        if (float.IsNaN (chargeFill)) chargeFill = _chargesMax;
        _charges.fillAmount = chargeFill;

        _icon.sprite = Slot.Equipment.Icon;

    }

    public void Toggle () {

        if (Slot.TargetState) Slot.TargetState = false;
        else Slot.TargetState = true;

    }

    public void ShowTooltip () {

        _tooltip.text = Slot.Equipment == null ? "None" : Slot.Equipment.Name;
        LeanTween.alphaCanvas (_tooltipGroup, 1, 0.2f).setIgnoreTimeScale (true);

    }

    public void HideTooltip () {

        _tooltip.text = "";
        LeanTween.alphaCanvas (_tooltipGroup, 0, 0.2f).setIgnoreTimeScale (true);

    }

    void TweenToCurAlpha () {

        LeanTween.alphaCanvas (_group, _curAlpha, 0.2f).setIgnoreTimeScale (true);

    }

}