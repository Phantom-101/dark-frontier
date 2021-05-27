﻿using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Weapons/Launcher")]
public class LauncherSO : EquipmentSO {
    public float EnergyRequired;
    public float RechargeRate;
    public List<MissileSO> CompatibleMissiles;
    public Damage DamageMultiplier;
    public float RangeMultiplier;

    public override void OnAwake (EquipmentSlot slot) {
        EnsureDataType (slot);
    }

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

        if (data.Activated && (data.Target == null || data.Missile == null || !CompatibleMissiles.Contains (data.Missile) || !slot.Equipper.Locks.ContainsKey (data.Target) || !slot.Equipper.HasInventoryCount (data.Missile, 1) || (data.Target.transform.position - slot.Equipper.transform.position).sqrMagnitude > data.Missile.Range * data.Missile.Range)) data.Activated = false;

        if (data.Activated && data.Charge >= EnergyRequired) {
            data.Charge = 0;
            GameObject vfx = Instantiate (data.Missile.MissileStructure.Prefab, slot.transform);
            vfx.transform.parent = slot.Equipper.transform.parent;
            Structure s = vfx.GetComponent<Structure> ();
            s.Initialize ();
            MissileAI ai = s.AI as MissileAI;
            ai.SetTarget (data.Target);
            ai.SetMissile (data.Missile);
            ai.SetLauncher (this, slot.Equipper.Locks[data.Target]);
            slot.Equipper.ChangeInventoryCount (data.Missile, -1);
            data.Activated = false;
        }
    }

    public override void FixedTick (EquipmentSlot slot) { }

    public override bool CanClick (EquipmentSlot slot) {
        if (slot.Equipper.Selected == null) return false;
        LauncherSlotData data = slot.Data as LauncherSlotData;
        if (data.Missile == null) return false;
        if (!CompatibleMissiles.Contains (data.Missile)) return false;
        if (!slot.Equipper.Locks.ContainsKey (slot.Equipper.Selected)) return false;
        if (!slot.Equipper.HasInventoryCount (data.Missile, 1)) return false;
        if ((slot.Equipper.Selected.transform.position - slot.Equipper.transform.position).sqrMagnitude > data.Missile.Range * data.Missile.Range) return false;
        return true;
    }

    public override void OnClicked (EquipmentSlot slot) {
        LauncherSlotData data = slot.Data as LauncherSlotData;
        data.Activated = !data.Activated;
        if (data.Activated) data.Target = slot.Equipper.Selected;
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
            Equipment = ItemManager.GetInstance ().GetItem (EquipmentId) as EquipmentSO,
            Durability = Durability,
            Charge = Charge,
            Activated = Activated,
            Target = StructureManager.GetInstance ().GetStructure (TargetId),
            Missile = ItemManager.GetInstance ().GetItem (MissileId) as MissileSO,
        };
    }
}
