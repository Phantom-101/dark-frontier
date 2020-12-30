using UnityEngine;
using UnityEngine.UI;

public class RadialEquipmentIndicator : EquipmentIndicator {

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

    private void Update () {

        if (_slot == null) return;

        if (_slot.GetEquipment () == null) {

            _energy.color = new Color (_energy.color.r, _energy.color.g, _energy.color.b, 0.25f);
            _durability.color = new Color (_durability.color.r, _durability.color.g, _durability.color.b, 0.25f);
            _charges.color = new Color (_charges.color.r, _charges.color.g, _charges.color.b, 0.25f);
            _icon.color = new Color (_icon.color.r, _icon.color.g, _icon.color.b, 0.25f);

            return;

        }

        _energy.color = new Color (_energy.color.r, _energy.color.g, _energy.color.b, 1);
        _durability.color = new Color (_durability.color.r, _durability.color.g, _durability.color.b, 1);
        _charges.color = new Color (_charges.color.r, _charges.color.g, _charges.color.b, 1);
        _icon.color = new Color (_icon.color.r, _icon.color.g, _icon.color.b, 1);

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

}
