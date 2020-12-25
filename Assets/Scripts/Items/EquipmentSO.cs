using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment")]
public class EquipmentSO : ItemSO {

    [Header ("Stats")]
    public int Meta;
    public double EnergyStorage;
    public double EnergyRechargeRate;
    public double Durability;

    [Header ("Equip")]
    public EquipmentChangedEventChannelSO OnEquipChannel;
    public EquipmentChangedEventChannelSO OnUnequipChannel;
    public EquipmentChangedEventChannelSO OnDestroyChannel;

}
