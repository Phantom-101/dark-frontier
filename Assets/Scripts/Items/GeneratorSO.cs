using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Generator")]
public class GeneratorSO : EquipmentSO {

    public float GenerationRate;

    public override void Tick (EquipmentSlot slot) {

        List<CapacitorSlot> capacitors = slot.GetEquipper ().GetEquipment<CapacitorSlot> ();
        float left = GenerationRate * Time.deltaTime;
        foreach (CapacitorSlot capacitor in capacitors) {

            if (capacitor != null) {

                float transferred = Mathf.Min (left, capacitor.GetRequiredEnergy ());
                left -= transferred;
                capacitor.ChangeStoredEnergy (transferred);

                if (left <= 0) break;

            }

        }

        slot.TakeDamage (Wear * Time.deltaTime);

    }

}
