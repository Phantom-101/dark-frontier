using System;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Weapons/Pulse Laser")]
public class PulseLaserSO : EquipmentSO {
    public float Range;
    public Damage Damage;
    public float EnergyRequired;
    public float RechargeRate;
    public AnimationCurve PreemptiveDamageMultiplier;
    public AnimationCurve PreemptiveRangeMultiplier;
    public GameObject BeamPrefab;
    public float BeamWidth;

    public override void OnAwake (EquipmentSlot slot) {
        EnsureDataType (slot);
    }

    public override void OnEquip (EquipmentSlot slot) {
        slot.Data = new PulseLaserSlotData {
            Slot = slot,
            Equipment = this,
            Durability = Durability,
            Charge = 0,
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

        PulseLaserSlotData data = slot.Data as PulseLaserSlotData;

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

        if (data.Activated && (data.Target == null || !slot.Equipper.Locks.ContainsKey (data.Target) || (data.Target.transform.position - slot.Equipper.transform.position).sqrMagnitude > GetRange (slot) * GetRange (slot))) data.Activated = false;

        if (data.Activated) {
            GameObject beam = Instantiate (BeamPrefab, slot.transform);
            beam.transform.LookAt (data.Target.transform);
            beam.transform.localScale = Vector3.one;
            beam.transform.localScale = new Vector3 (
                BeamWidth / beam.transform.lossyScale.x,
                BeamWidth / beam.transform.lossyScale.y,
                Vector3.Distance (slot.transform.position, data.Target.transform.position) / beam.transform.lossyScale.z
            );
            Destroy (beam, 0.2f);
            /* Use raycast?
            if (Physics.Raycast (slot.Equipper.transform.position, data.Target.transform.position - slot.Equipper.transform.position, out RaycastHit hit, Range)) {
                Structure structure = hit.transform.gameObject.GetComponentInParent<Structure> ();
                if (structure != null) structure.TakeDamage (Damage * HeatDamageMultiplier.Evaluate (data.Heat / MaxHeat), hit.point);
            }
            */
            data.Target.TakeDamage (Damage * PreemptiveDamageMultiplier.Evaluate (data.Charge / EnergyRequired), slot.Equipper.transform.position);
            data.Charge = 0;
            data.Activated = false;
        }
    }

    public override void FixedTick (EquipmentSlot slot) { }

    public override bool CanClick (EquipmentSlot slot) {
        PulseLaserSlotData data = slot.Data as PulseLaserSlotData;
        if (data.Activated) {
            // If equipment is activated and selected is null or target
            // Assume user wants to deactivate equipment
            if (slot.Equipper.Selected == null || slot.Equipper.Selected == data.Target) return true;
            // If equipment is activated and selected is not null
            // Assume user wants to change target
            else {
                if (!slot.Equipper.Locks.ContainsKey (slot.Equipper.Selected)) return false;
                if ((slot.Equipper.Selected.transform.position - slot.Equipper.transform.position).sqrMagnitude > GetRange (slot) * GetRange (slot)) return false;
                return true;
            }
        } else {
            // If equipment is not activated
            // Assume user wants to activate equipment
            if (slot.Equipper.Selected == null) return false;
            if (!slot.Equipper.Locks.ContainsKey (slot.Equipper.Selected)) return false;
            if ((slot.Equipper.Selected.transform.position - slot.Equipper.transform.position).sqrMagnitude > GetRange (slot) * GetRange (slot)) return false;
            return true;
        }
    }

    public override void OnClicked (EquipmentSlot slot) {
        if (!CanClick (slot)) return;
        PulseLaserSlotData data = slot.Data as PulseLaserSlotData;
        if (data.Activated) {
            // If equipment is activated and selected is null or target
            // Assume user wants to deactivate equipment
            if (slot.Equipper.Selected == null || slot.Equipper.Selected == data.Target) data.Activated = false;
            // If equipment is activated and selected is not null
            // Assume user wants to change target
            else data.Target = slot.Equipper.Selected;
        } else {
            // If equipment is not activated
            // Assume user wants to activate equipment
            data.Activated = true;
            data.Target = slot.Equipper.Selected;
        }
    }

    public override void EnsureDataType (EquipmentSlot slot) {
        if (!(slot.Data is PulseLaserSlotData)) slot.Data = new PulseLaserSlotData {
            Slot = slot,
            Equipment = this,
            Durability = Durability,
            Charge = 0,
            Activated = false,
            Target = null,
        };
    }

    public float GetRange (EquipmentSlot slot) {
        return Range * GetRangeMultiplier (slot);
    }

    public float GetRangeMultiplier (EquipmentSlot slot) {
        return PreemptiveRangeMultiplier.Evaluate ((slot.Data as PulseLaserSlotData).Charge / EnergyRequired);
    }
}

[Serializable]
public class PulseLaserSlotData : EquipmentSlotData {
    public float Charge;
    public bool Activated;
    public Structure Target;

    public override EquipmentSlotSaveData Save () {
        return new PulseLaserSlotSaveData {
            EquipmentId = Equipment == null ? "" : Equipment.Id,
            Durability = Durability,
            Charge = Charge,
            Activated = Activated,
            TargetId = Target == null ? "" : Target.Id,
        };
    }
}

[Serializable]
public class PulseLaserSlotSaveData : EquipmentSlotSaveData {
    public float Charge;
    public bool Activated;
    public string TargetId;

    public override EquipmentSlotData Load () {
        return new PulseLaserSlotData {
            Equipment = ItemManager.Instance.GetItem (EquipmentId) as EquipmentSO,
            Durability = Durability,
            Charge = Charge,
            Activated = Activated,
            Target = StructureManager.Instance.GetStructure (TargetId),
        };
    }
}
