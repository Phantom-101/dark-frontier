using System;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Weapons/Miner")]
public class MinerSO : EquipmentSO {
    public float Range;
    public Damage DamagePerSecond;
    public float DamageInterval;
    public float EnergyConsumption;
    public float MaxHeat;
    public float HeatGeneration;
    public float CoolingRate;
    public float OverheatDamage;
    public AnimationCurve HeatDamageMultiplier;
    public StructureSOToFloatDictionary StructureDamageMultipliers;
    public GameObject BeamPrefab;
    public float BeamWidth;

    public override void OnAwake (EquipmentSlot slot) {
        EnsureDataType (slot);
    }

    public override void OnEquip (EquipmentSlot slot) {
        slot.Data = new MinerSlotData {
            Slot = slot,
            Equipment = this,
            Durability = Durability,
            AccumulatedDamageMultiplier = 0,
            Heat = 0,
            Activated = false,
            Target = null,
        };
    }

    public override void OnUnequip (EquipmentSlot slot) {
        MinerSlotData data = slot.Data as MinerSlotData;
        if (data.Beam != null) Destroy (data.Beam);
        slot.Data = new EquipmentSlotData {
            Slot = slot,
            Equipment = null,
            Durability = 0,
        };
    }

    public override void Tick (EquipmentSlot slot) {
        EnsureDataType (slot);

        MinerSlotData data = slot.Data as MinerSlotData;

        if (data.Activated && (data.Target == null || !slot.Equipper.Locks.ContainsKey (data.Target) || (data.Target.transform.position - slot.Equipper.transform.position).sqrMagnitude > Range * Range)) data.Activated = false;

        data.Heat = Mathf.Clamp (data.Heat - CoolingRate * Time.deltaTime + (data.Activated ? HeatGeneration * Time.deltaTime : 0), 0, MaxHeat);

        if (data.Activated) {
            if (data.Beam == null) data.Beam = Instantiate (BeamPrefab, slot.transform);
            data.Beam.transform.LookAt (data.Target.transform);
            data.Beam.transform.localScale = Vector3.one;
            data.Beam.transform.localScale = new Vector3 (
                BeamWidth / data.Beam.transform.lossyScale.x,
                BeamWidth / data.Beam.transform.lossyScale.y,
                Vector3.Distance (slot.transform.position, data.Target.transform.position) / data.Beam.transform.lossyScale.z
            );
            float consumption = EnergyConsumption * Time.deltaTime;
            float given = 0;
            slot.Equipper.GetEquipmentData<CapacitorSlotData> ().ForEach (capacitor => {
                float chargeLeft = capacitor.Charge;
                float dischargeLeft = capacitor.DischargeLeft;
                float allocated = Mathf.Min (chargeLeft, dischargeLeft, consumption - given);
                given += allocated;
                capacitor.Charge -= allocated;
                capacitor.DischargeLeft -= allocated;
            });
            data.AccumulatedDamageMultiplier += given / consumption * Time.deltaTime;
            /* Use raycast?
            if (Physics.Raycast (slot.Equipper.transform.position, data.Target.transform.position - slot.Equipper.transform.position, out RaycastHit hit, Range)) {
                Structure structure = hit.transform.gameObject.GetComponentInParent<Structure> ();
                if (structure != null) structure.TakeDamage (Damage * HeatDamageMultiplier.Evaluate (data.Heat / MaxHeat), hit.point);
            }
            */
            if (data.AccumulatedDamageMultiplier >= DamageInterval) {
                data.Target.TakeDamage (DamagePerSecond * data.AccumulatedDamageMultiplier * HeatDamageMultiplier.Evaluate (data.Heat / MaxHeat) * StructureDamageMultipliers[data.Target.Profile], slot.Equipper.transform.position);
                data.AccumulatedDamageMultiplier = 0;
            }
        } else {
            data.AccumulatedDamageMultiplier = 0;
            if (data.Beam != null) Destroy (data.Beam);
        }

        if (data.Heat == MaxHeat) {
            data.Durability = Mathf.Max (data.Durability - OverheatDamage * Time.deltaTime, 0);
            if (data.Durability == 0) OnUnequip (slot);
        }
    }

    public override void FixedTick (EquipmentSlot slot) { }

    public override bool CanClick (EquipmentSlot slot) {
        if (slot.Equipper.Selected == null) return false;
        if (!StructureDamageMultipliers.ContainsKey (slot.Equipper.Selected.Profile)) return false;
        if (!slot.Equipper.Locks.ContainsKey (slot.Equipper.Selected)) return false;
        if ((slot.Equipper.Selected.transform.position - slot.Equipper.transform.position).sqrMagnitude > Range * Range) return false;
        return true;
    }

    public override void OnClicked (EquipmentSlot slot) {
        MinerSlotData data = slot.Data as MinerSlotData;
        data.Activated = !data.Activated;
        if (data.Activated) data.Target = slot.Equipper.Selected;
    }

    public override void EnsureDataType (EquipmentSlot slot) {
        if (!(slot.Data is MinerSlotData)) slot.Data = new MinerSlotData {
            Slot = slot,
            Equipment = this,
            Durability = Durability,
            AccumulatedDamageMultiplier = 0,
            Heat = 0,
            Activated = false,
            Target = null,
        };
    }
}

[Serializable]
public class MinerSlotData : EquipmentSlotData {
    public float AccumulatedDamageMultiplier;
    public float Heat;
    public bool Activated;
    public Structure Target;
    public GameObject Beam;

    public override EquipmentSlotSaveData Save () {
        return new MinerSlotSaveData {
            EquipmentId = Equipment == null ? "" : Equipment.Id,
            Durability = Durability,
            AccumulatedDamageMultiplier = AccumulatedDamageMultiplier,
            Heat = Heat,
            Activated = Activated,
            TargetId = Target == null ? "" : Target.Id,
        };
    }
}

[Serializable]
public class MinerSlotSaveData : EquipmentSlotSaveData {
    public float AccumulatedDamageMultiplier;
    public float Heat;
    public bool Activated;
    public string TargetId;

    public override EquipmentSlotData Load () {
        return new MinerSlotData {
            Equipment = ItemManager.GetInstance ().GetItem (EquipmentId) as EquipmentSO,
            Durability = Durability,
            AccumulatedDamageMultiplier = AccumulatedDamageMultiplier,
            Heat = Heat,
            Activated = Activated,
            Target = StructureManager.Instance.GetStructure (TargetId),
        };
    }
}
