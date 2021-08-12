using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : BehaviorBase {
    public Structure Equipper;
    public List<ItemConditionSO> Filters = new List<ItemConditionSO> ();
    [SerializeReference] public EquipmentSlotData Data = new EquipmentSlotData ();
    public Vector3 LocalPosition { get => transform.position - Equipper.transform.position + Equipper.transform.localPosition; }

    protected override void SingleInitialize () {
        Equipper = GetComponentInParent<Structure> ();
    }

    protected override void InternalTick (float dt) {
        if (Data.Equipment != null) Data.Equipment.Tick (this, dt);
    }

    protected override void InternalFixedTick (float dt) {
        if (Data.Equipment != null) Data.Equipment.FixedTick (this, dt);
    }

    public override bool Validate () {
        // Equipper should be non-null
        if (Equipper == null) {
            // Try get equipper
            Equipper = GetComponentInParent<Structure> ();
            // If null, there is no equipper in parent tree
            if (Equipper == null) return false;
        }
        // Data should be non-null
        if (Data == null) Data = new EquipmentSlotData ();
        // Data's slot should be this
        Data.Slot = this;
        return true;
    }

    public bool ChangeEquipment (EquipmentSO target) {
        foreach (ItemConditionSO filter in Filters)
            if (!filter.MeetsCondition (target))
                return false;
        target.OnEquip (this);
        return true;
    }
}
