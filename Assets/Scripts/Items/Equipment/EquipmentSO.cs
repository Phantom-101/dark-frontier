using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment")]
public class EquipmentSO : ItemSO {

    [Header ("Stats")]
    public int Tier;
    public float EnergyStorage;
    public float EnergyRechargeRate;
    public float Durability;
    public float Wear;
    public bool Activatable;
    public bool RequireCharge;
    public float InventorySize;
    public ChargeSO[] Charges;

    public virtual void OnAwake (EquipmentSlot slot) { }

    public virtual void OnEquip (EquipmentSlot slot) { }

    public virtual void OnUnequip (EquipmentSlot slot) { }

    public virtual bool CanActivate (EquipmentSlot slot) { return false; }

    public virtual bool CanSustain (EquipmentSlot slot) { return false; }

    public virtual bool Activate (EquipmentSlot slot) { return false; }

    public virtual void Deactivate (EquipmentSlot slot) { }

    public virtual void Tick (EquipmentSlot slot) { }

    public virtual void FixedTick (EquipmentSlot slot) { }

}
