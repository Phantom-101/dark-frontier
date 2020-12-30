using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Capacitor")]
public class CapacitorSO : EquipmentSO {

    public override void Tick (EquipmentSlot slot) {

        CapacitorSlot capacitor = slot as CapacitorSlot;

        foreach (EquipmentSlot s in slot.GetEquipper ().GetEquipment ()) {

            if (s == null || s.GetEquipment () == null) continue;

            float need = s.GetRequiredEnergy ();
            float rate = s.GetEquipment ().EnergyRechargeRate * Time.deltaTime;
            float has = capacitor.GetStoredEnergy ();
            float transferred = Mathf.Min (need, Mathf.Min (rate, has));
            capacitor.ChangeStoredEnergy (-transferred);
            s.ChangeStoredEnergy (transferred);

        }

        slot.TakeDamage (Wear * Time.deltaTime);

    }

}
