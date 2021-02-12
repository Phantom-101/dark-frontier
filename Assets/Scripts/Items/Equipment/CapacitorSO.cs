using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Capacitor")]
public class CapacitorSO : EquipmentSO {

    public override void SafeTick (EquipmentSlot slot) {

        CapacitorSlot cap = slot as CapacitorSlot;

        foreach (EquipmentSlot s in slot.Equipper.GetEquipment ()) {

            if (s == null || s.Equipment == null) continue;

            float need = s.Equipment.EnergyStorage - s.Energy;
            float rate = s.Equipment.EnergyRechargeRate * Time.deltaTime;
            float has = cap.Energy;
            float transferred = Mathf.Min (need, Mathf.Min (rate, has));
            cap.Energy -= transferred;
            s.Energy += transferred;

        }

        slot.Durability -= Wear * Time.deltaTime;

    }

}
