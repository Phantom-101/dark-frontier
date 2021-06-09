using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : MonoBehaviour {
    public Structure Equipper;
    public List<ItemConditionSO> Filters = new List<ItemConditionSO> ();
    [SerializeReference] public EquipmentSlotData Data = new EquipmentSlotData ();

    public void Tick () {
        Data.Slot = this;
        if (Data.Equipment != null) Data.Equipment.Tick (this);
    }

    public void FixedTick () {
        Data.Slot = this;
        if (Data.Equipment != null) Data.Equipment.FixedTick (this);
    }

    public bool ChangeEquipment (EquipmentSO target) {
        foreach (ItemConditionSO filter in Filters)
            if (!filter.MeetsCondition (target))
                return false;
        target.OnEquip (this);
        return true;
    }
}
