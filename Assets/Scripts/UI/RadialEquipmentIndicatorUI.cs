using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RadialEquipmentIndicatorUI : EquipmentIndicatorUI {

    [SerializeField] protected Image _energy;
    [SerializeField] protected float _energyMax;
    [SerializeField] protected float _energyOffset;
    [SerializeField] protected Image _durability;
    [SerializeField] protected float _durabilityMax;
    [SerializeField] protected float _durabilityOffset;
    [SerializeField] protected Image _charges;
    [SerializeField] protected float _chargesMax;
    [SerializeField] protected float _chargesOffset;
    [SerializeField] protected Image _icon;
    [SerializeField] protected Button _button;
    [SerializeField] protected RectTransform _buttonRect;
    [SerializeField] protected EventTrigger _eventTrigger;
    [SerializeField] protected Text _tooltip;
    [SerializeField] protected float _curAlpha = -1;

    private void Start () {

        _tooltip.text = "";

    }

    private void Update () {

        UIState current = UIStateManager.GetInstance ().GetState ();
        bool shouldShow = current == UIState.InSpace;

        if (!shouldShow) {

            if (_curAlpha != 0) {

                _curAlpha = 0;
                DisableAll ();
                TweenToCurAlpha ();

            }
            return;

        }

        if (_curAlpha == 0) EnableAll ();

        if (_slot == null) return;

        if (_slot.GetEquipment () == null) {

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

        float energyFill = Mathf.Clamp (_slot.GetStoredEnergy () / _slot.GetEquipment ().EnergyStorage * _energyMax + _energyOffset, _energyOffset, _energyMax + _energyOffset);
        if (float.IsNaN (energyFill)) energyFill = _energyMax + _energyOffset;
        _energy.fillAmount = energyFill;

        float durabilityFill = Mathf.Clamp (_slot.GetDurability () / _slot.GetEquipment ().Durability * _durabilityMax + _durabilityOffset, _durabilityOffset, _durabilityMax + _durabilityOffset);
        if (float.IsNaN (durabilityFill)) durabilityFill = _durabilityMax + _durabilityOffset;
        _durability.fillAmount = durabilityFill;

        float chargeFill = Mathf.Clamp (_slot.GetUsedInventorySize () / _slot.GetTotalInventorySize () * _chargesMax + _chargesOffset, _chargesOffset, _chargesMax + _chargesOffset);
        if (float.IsNaN (chargeFill)) chargeFill = _chargesMax + _chargesOffset;
        _charges.fillAmount = chargeFill;

        _icon.sprite = _slot.GetEquipment ().Icon;

    }

    public void Toggle () {

        if (_slot.IsActive ()) _slot.Deactivate ();
        else _slot.Activate ();

    }

    public void ShowTooltip () {

        _tooltip.text = _slot.GetEquipment ().Name;

    }

    public void HideTooltip () {

        _tooltip.text = "";

    }

    void TweenToCurAlpha () {

        LeanTween.alpha (_energy.rectTransform, _curAlpha, 0.2f).setIgnoreTimeScale (true);
        LeanTween.alpha (_durability.rectTransform, _curAlpha, 0.2f).setIgnoreTimeScale (true);
        LeanTween.alpha (_charges.rectTransform, _curAlpha, 0.2f).setIgnoreTimeScale (true);
        LeanTween.alpha (_icon.rectTransform, _curAlpha, 0.2f).setIgnoreTimeScale (true);
        LeanTween.alpha (_buttonRect, _curAlpha, 0.2f).setIgnoreTimeScale (true);

    }

    void DisableAll () {

        _button.interactable = false;
        _button.targetGraphic.raycastTarget = false;
        _eventTrigger.enabled = false;

    }

    void EnableAll () {

        _button.interactable = true;
        _button.targetGraphic.raycastTarget = true;
        _eventTrigger.enabled = true;

    }

}
