using UnityEngine;
using UnityEngine.UI;

public class LinearEquipmentIndicator : EquipmentIndicator {

    [SerializeField] protected Image _energy;
    [SerializeField] protected RectTransform _energyTransform;
    [SerializeField] protected float _energyMax;
    [SerializeField] protected Image _durability;
    [SerializeField] protected RectTransform _durabilityTransform;
    [SerializeField] protected float _durabilityMax;
    [SerializeField] protected Image _charges;
    [SerializeField] protected RectTransform _chargesTransform;
    [SerializeField] protected float _chargesMax;
    [SerializeField] protected Image _icon;
    [SerializeField] protected Image _outline;
    [SerializeField] protected Button _button;

    private void Start () {

        _button.onClick.AddListener (_slot.Activate);

    }

    private void Update () {

        if (_slot == null) return;

        if (_slot.GetEquipment () == null) {

            _energy.color = new Color (_energy.color.r, _energy.color.g, _energy.color.b, 0.25f);
            _durability.color = new Color (_durability.color.r, _durability.color.g, _durability.color.b, 0.25f);
            _charges.color = new Color (_charges.color.r, _charges.color.g, _charges.color.b, 0.25f);
            _icon.color = new Color (1, 1, 1, 0.25f);
            _outline.color = new Color (1, 1, 1, 0.25f);
            _button.interactable = false;

            return;

        }

        _energy.color = new Color (_energy.color.r, _energy.color.g, _energy.color.b, 1);
        _durability.color = new Color (_durability.color.r, _durability.color.g, _durability.color.b, 1);
        _charges.color = new Color (_charges.color.r, _charges.color.g, _charges.color.b, 1);
        _icon.color = new Color (1, 1, 1, 1);
        _outline.color = new Color (1, 1, 1, 1);
        _button.interactable = true;

        float energyFill = Mathf.Clamp (_slot.GetStoredEnergy () / _slot.GetEquipment ().EnergyStorage * _energyMax, 0, _energyMax);
        if (float.IsNaN (energyFill)) energyFill = _energyMax;
        _energyTransform.sizeDelta = new Vector2 (energyFill, _energyTransform.sizeDelta.y);

        float durabilityFill = Mathf.Clamp (_slot.GetDurability () / _slot.GetEquipment ().Durability * _durabilityMax, 0, _durabilityMax);
        if (float.IsNaN (durabilityFill)) durabilityFill = _durabilityMax;
        _durabilityTransform.sizeDelta = new Vector2 (durabilityFill, _durabilityTransform.sizeDelta.y);

        float chargeFill = Mathf.Clamp (_slot.GetUsedInventorySize () / _slot.GetTotalInventorySize () * _chargesMax, 0, _chargesMax);
        if (float.IsNaN (chargeFill)) chargeFill = _chargesMax;
        _chargesTransform.sizeDelta = new Vector2 (chargeFill, _chargesTransform.sizeDelta.y);

        _icon.sprite = _slot.GetEquipment ().Icon;

    }

}
