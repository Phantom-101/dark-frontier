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

        float energyFill = Mathf.Clamp (_slot.GetStoredEnergy () / _slot.GetEquipment ().EnergyStorage * _energyMax, 0, _energyMax);
        if (float.IsNaN (energyFill)) energyFill = _energyMax;
        _energy.fillAmount = energyFill;

        float durabilityFill = Mathf.Clamp (_slot.GetDurability () / _slot.GetEquipment ().Durability * _durabilityMax, 0, _durabilityMax);
        if (float.IsNaN (durabilityFill)) durabilityFill = _durabilityMax;
        _durability.fillAmount = durabilityFill;

        float chargeFill = Mathf.Clamp (_slot.GetUsedInventorySize () / _slot.GetTotalInventorySize () * _chargesMax, 0, _chargesMax);
        if (float.IsNaN (chargeFill)) chargeFill = _chargesMax;
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

        LeanTween.alphaCanvas (_group, _curAlpha, 0.2f).setIgnoreTimeScale (true);

    }

    void DisableAll () {

        _group.blocksRaycasts = false;
        _group.interactable = false;

    }

    void EnableAll () {

        _group.blocksRaycasts = true;
        _group.interactable = true;

    }

}