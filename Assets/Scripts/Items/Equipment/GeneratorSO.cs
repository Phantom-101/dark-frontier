using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Generator")]
public class GeneratorSO : EquipmentSO {

    public float GenerationRate;

    public override void SafeTick (EquipmentSlot slot) {

        List<CapacitorSlot> caps = slot.Equipper.GetEquipment<CapacitorSlot> ();
        float left = GenerationRate * Time.deltaTime;
        foreach (CapacitorSlot cap in caps) {

            if (cap != null && cap.Equipment != null) {

                float transferred = Mathf.Min (left, cap.Equipment.EnergyStorage - cap.Energy);
                left -= transferred;
                cap.Energy += transferred;

                if (left <= 0) break;

            }

        }

        slot.Durability -= Wear * Time.deltaTime;

    }

}
