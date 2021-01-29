using UnityEngine;

public class CapacitorSlot : EquipmentSlot {

    public CapacitorSO GetCapacitor () { return _equipment as CapacitorSO; }

    public bool HasEnergy (float condition) { return _storedEnergy >= condition; }

    public float AllocateEnergy (float amount) {

        float allocate = Mathf.Min (amount, GetStoredEnergy ());
        ChangeStoredEnergy (-allocate);

        return allocate;

    }

    public override bool CanEquip (EquipmentSO equipment) {

        return base.CanEquip (equipment) || (equipment is CapacitorSO && equipment.Tier <= _equipper.GetProfile ().MaxEquipmentTier);

    }

}
