using System;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Weapons/Beam Laser")]
public class BeamLaserSO : EquipmentSO {
    public float Range;
    public Damage Damage;
    public float EnergyRequired;
    public float RechargeRate;
    public float MaxHeat;
    public float HeatGeneration;
    public float CoolingRate;
    public float OverheatDamage;
    public AnimationCurve HeatDamageMultiplier;

    public override void OnAwake (EquipmentSlot slot) {
        EnsureDataType (slot);
    }

    public override void OnEquip (EquipmentSlot slot) {
        slot.Data = new BeamLaserSlotData {
            Slot = slot,
            Equipment = this,
            Durability = Durability,
            Charge = 0,
            Heat = 0,
            Activated = false,
            Target = null,
        };
    }

    public override void OnUnequip (EquipmentSlot slot) {
        slot.Data = new EquipmentSlotData {
            Slot = slot,
            Equipment = null,
            Durability = 0,
        };
    }

    public override void Tick (EquipmentSlot slot) {
        EnsureDataType (slot);

        BeamLaserSlotData data = slot.Data as BeamLaserSlotData;

        float consumption = RechargeRate * Time.deltaTime;
        float lack = EnergyRequired - data.Charge;
        float request = Mathf.Min (consumption, lack);
        float given = 0;
        slot.Equipper.GetEquipmentData<CapacitorSlotData> ().ForEach (capacitor => {
            float chargeLeft = capacitor.Charge;
            float dischargeLeft = capacitor.DischargeLeft;
            float allocated = Mathf.Min (chargeLeft, dischargeLeft, request - given);
            given += allocated;
            capacitor.Charge -= allocated;
            capacitor.DischargeLeft -= allocated;
        });
        data.Charge += given;

        if (data.Activated && (data.Target == null || !slot.Equipper.Locks.ContainsKey (data.Target) || (data.Target.transform.position - slot.Equipper.transform.position).sqrMagnitude > Range * Range)) data.Activated = false;

        data.Heat = Mathf.Clamp (data.Heat - CoolingRate * Time.deltaTime + (data.Activated ? HeatGeneration * Time.deltaTime : 0), 0, MaxHeat);

        if (data.Activated && data.Charge >= EnergyRequired) {
            data.Charge = 0;
            /* Use raycast?
            if (Physics.Raycast (slot.Equipper.transform.position, data.Target.transform.position - slot.Equipper.transform.position, out RaycastHit hit, Range)) {
                Structure structure = hit.transform.gameObject.GetComponentInParent<Structure> ();
                if (structure != null) structure.TakeDamage (Damage * HeatDamageMultiplier.Evaluate (data.Heat / MaxHeat), hit.point);
            }
            */
            data.Target.TakeDamage (Damage * HeatDamageMultiplier.Evaluate (data.Heat / MaxHeat), slot.Equipper.transform.position);
        }

        if (data.Heat == MaxHeat) {
            data.Durability = Mathf.Max (data.Durability - OverheatDamage * Time.deltaTime, 0);
            if (data.Durability == 0) OnUnequip (slot);
        }
    }

    public override void FixedTick (EquipmentSlot slot) { }

    public override bool CanClick (EquipmentSlot slot) {
        if (slot.Equipper.Selected == null) return false;
        if (!slot.Equipper.Locks.ContainsKey (slot.Equipper.Selected)) return false;
        if ((slot.Equipper.Selected.transform.position - slot.Equipper.transform.position).sqrMagnitude > Range * Range) return false;
        return true;
    }

    public override void OnClicked (EquipmentSlot slot) {
        BeamLaserSlotData data = slot.Data as BeamLaserSlotData;
        data.Activated = !data.Activated;
        if (data.Activated) data.Target = slot.Equipper.Selected;
    }

    public override void EnsureDataType (EquipmentSlot slot) {
        if (!(slot.Data is BeamLaserSlotData)) slot.Data = new BeamLaserSlotData {
            Slot = slot,
            Equipment = this,
            Durability = Durability,
            Charge = 0,
            Heat = 0,
            Activated = false,
            Target = null,
        };
    }
}

[Serializable]
public class BeamLaserSlotData : EquipmentSlotData {
    public float Charge;
    public float Heat;
    public bool Activated;
    public Structure Target;

    public override EquipmentSlotSaveData Save () {
        return new BeamLaserSlotSaveData {
            EquipmentId = Equipment == null ? "" : Equipment.Id,
            Durability = Durability,
            Charge = Charge,
            Heat = Heat,
            Activated = Activated,
            TargetId = Target == null ? "" : Target.Id,
        };
    }
}

[Serializable]
public class BeamLaserSlotSaveData : EquipmentSlotSaveData {
    public float Charge;
    public float Heat;
    public bool Activated;
    public string TargetId;

    public override EquipmentSlotData Load () {
        return new BeamLaserSlotData {
            Equipment = ItemManager.GetInstance ().GetItem (EquipmentId) as EquipmentSO,
            Durability = Durability,
            Charge = Charge,
            Heat = Heat,
            Activated = Activated,
            Target = StructureManager.GetInstance ().GetStructure (TargetId),
        };
    }
}
