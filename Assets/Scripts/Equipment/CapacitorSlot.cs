using UnityEngine;

public class CapacitorSlot : EquipmentSlot {

    public CapacitorSO Capacitor { get { return _equipment as CapacitorSO; } }

    public override bool CanEquip (EquipmentSO equipment) {

        return equipment == null || (base.CanEquip (equipment) && equipment is CapacitorSO);

    }
    public float AllocateEnergy (float amount) {

        float allocated = Mathf.Min (amount, Energy);
        Energy -= allocated;

        return allocated;

    }
    public bool HasEnergy (float condition) { return Energy >= condition; }

}
