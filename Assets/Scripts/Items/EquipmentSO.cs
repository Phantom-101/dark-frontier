﻿using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment")]
public class EquipmentSO : ItemSO {

    [Header ("Stats")]
    public int Meta;
    public float EnergyStorage;
    public float EnergyRechargeRate;
    public float Durability;
    public bool Activatable;

    public virtual void OnEquip (EquipmentSlot slot) { }

    public virtual void OnUnequip (EquipmentSlot slot) { }

    public virtual bool CanActivate (EquipmentSlot slot) { return false; }

    public virtual bool CanSustain (EquipmentSlot slot) { return false; }

    public virtual void Activate (EquipmentSlot slot) { }

    public virtual void Deactivate (EquipmentSlot slot) { }

    public virtual void Tick (EquipmentSlot slot) { }

    public virtual void FixedTick (EquipmentSlot slot) { }

    public virtual void TakeDamage (EquipmentSlot slot, float amount) {

        slot.ChangeDurability (-amount);

        if (slot.GetDurability () <= 0) slot.Equip (null);

    }

}
