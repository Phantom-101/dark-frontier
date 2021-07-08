using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Weapons/Launcher")]
public class LauncherSO : EquipmentSO {
    public float EnergyRequired;
    public float RechargeRate;
    public bool AutoCycle;
    public List<MissileSO> CompatibleMissiles;

    public override void OnAwake (EquipmentSlot slot) => EnsureDataType (slot);

    public override void OnEquip (EquipmentSlot slot) {
        slot.Data = new LauncherSlotData {
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

        LauncherSlotData data = slot.Data as LauncherSlotData;

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

        if (data.Activated && (data.Target == null || data.Missile == null || !CompatibleMissiles.Contains (data.Missile) || !slot.Equipper.Locks.ContainsKey (data.Target) || !slot.Equipper.Inventory.HasQuantity (data.Missile, 1) || (data.Target.transform.position - slot.Equipper.transform.position).sqrMagnitude > data.Missile.Range * data.Missile.Range)) data.Activated = false;

        if (data.Activated && data.Charge >= EnergyRequired) {
            data.Charge = 0;
            GameObject vfx = Instantiate (data.Missile.MissileStructure.Prefab, slot.transform);
            vfx.transform.parent = slot.Equipper.transform.parent;
            Structure s = vfx.GetComponent<Structure> ();
            s.Initialize ();
            MissileAI ai = CreateInstance<MissileAI> ();
            s.AI = ai;
            ai.Target = data.Target;
            ai.Missile = data.Missile;
            ai.DamageMultiplier = slot.Equipper.Locks[data.Target] / 100;
            slot.Equipper.Inventory.RemoveQuantity (data.Missile, 1);
            data.Activated = AutoCycle;
        }
    }

    public override void FixedTick (EquipmentSlot slot) { }

    public override bool CanClick (EquipmentSlot slot) {
        LauncherSlotData data = slot.Data as LauncherSlotData;
        if (data.Activated) {
            // If equipment is activated and selected is null or target
            // Assume user wants to deactivate equipment
            if (slot.Equipper.Selected == null || slot.Equipper.Selected == data.Target) return true;
            // If equipment is activated and selected is not null
            // Assume user wants to change target
            else {
                if (data.Missile == null) return false;
                if (!CompatibleMissiles.Contains (data.Missile)) return false;
                if (!slot.Equipper.Locks.ContainsKey (slot.Equipper.Selected)) return false;
                if (!slot.Equipper.Inventory.HasQuantity (data.Missile, 1)) return false;
                if ((slot.Equipper.Selected.transform.position - slot.Equipper.transform.position).sqrMagnitude > data.Missile.Range * data.Missile.Range) return false;
                return true;
            }
        } else {
            // If equipment is not activated
            // Assume user wants to activate equipment
            if (slot.Equipper.Selected == null) return false;
            if (data.Missile == null) return false;
            if (!CompatibleMissiles.Contains (data.Missile)) return false;
            if (!slot.Equipper.Locks.ContainsKey (slot.Equipper.Selected)) return false;
            if (!slot.Equipper.Inventory.HasQuantity (data.Missile, 1)) return false;
            if ((slot.Equipper.Selected.transform.position - slot.Equipper.transform.position).sqrMagnitude > data.Missile.Range * data.Missile.Range) return false;
            return true;
        }
    }

    public override void OnClicked (EquipmentSlot slot) {
        LauncherSlotData data = slot.Data as LauncherSlotData;
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
        if (!(slot.Data is LauncherSlotData)) slot.Data = new LauncherSlotData {
            Slot = slot,
            Equipment = this,
            Durability = Durability,
            Charge = 0,
            Activated = false,
            Target = null,
        };
    }
}

[Serializable]
public class LauncherSlotData : EquipmentSlotData {
    public float Charge;
    public bool Activated;
    public Structure Target;
    public MissileSO Missile;

    public override EquipmentSlotSaveData Save () {
        return new LauncherSlotSaveData {
            EquipmentId = Equipment == null ? "" : Equipment.Id,
            Durability = Durability,
            Charge = Charge,
            Activated = Activated,
            TargetId = Target == null ? "" : Target.Id,
            MissileId = Missile == null ? "" : Missile.Id,
        };
    }
}

[Serializable]
public class LauncherSlotSaveData : EquipmentSlotSaveData {
    public float Charge;
    public bool Activated;
    public string TargetId;
    public string MissileId;

    public override EquipmentSlotData Load () {
        return new LauncherSlotData {
            Equipment = ItemManager.Instance.GetItem (EquipmentId) as EquipmentSO,
            Durability = Durability,
            Charge = Charge,
            Activated = Activated,
            Target = StructureManager.Instance.GetStructure (TargetId),
            Missile = ItemManager.Instance.GetItem (MissileId) as MissileSO,
        };
    }
}
