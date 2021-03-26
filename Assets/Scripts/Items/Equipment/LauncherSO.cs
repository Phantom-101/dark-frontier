﻿using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Weapons/Launcher")]
public class LauncherSO : WeaponSO {

    public DamageProfile DamageMultiplier;
    public float RangeMultiplier;
    public float Spread;

    public override bool CanCycleStart (EquipmentSlot slot) {

        if (!base.CanCycleStart (slot)) return false;
        if ((slot as WeaponSlot).Target == null) return false;
        if (!(slot as WeaponSlot).CanFireAt ((slot as WeaponSlot).Target)) return false;
        if (!WithinRange (slot)) return false;
        return true;

    }

    public override void OnCycleStart (EquipmentSlot slot) {

        MissileSO missile = slot.Charge as MissileSO;
        GameObject vfx = Instantiate (missile.MissileStructure.Prefab, slot.transform);
        vfx.transform.parent = slot.Equipper.transform.parent;
        vfx.transform.Rotate (new Vector3 (Random.Range (-Spread, Spread), Random.Range (-Spread, Spread), 0));
        Structure s = vfx.GetComponent<Structure> ();
        s.Initialize ();
        MissileAI ai = s.AI as MissileAI;
        ai.SetTarget (slot.Target);
        ai.SetMissile (missile);
        ai.SetLauncher (slot.Equipment as LauncherSO, slot.Equipper.Locks[slot.Target]);

    }

    public virtual bool WithinRange (EquipmentSlot slot) {

        if (slot.Equipper == null || slot.Equipper.Selected == null) return false;

        MissileSO missile = slot.Charge as MissileSO;
        float range = missile.Range * (slot.Equipment as LauncherSO).RangeMultiplier;
        return (slot.Equipper.transform.localPosition - slot.Equipper.Selected.transform.localPosition).sqrMagnitude <= range * range;

    }

}
