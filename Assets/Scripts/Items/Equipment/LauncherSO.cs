using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Weapons/Launcher")]
public class LauncherSO : ChargeMajorWeaponSO {

    public override bool WithinRange (EquipmentSlot slot) {

        MissileSO missile = slot.GetCharge () as MissileSO;
        float range = missile.Range * (slot.GetEquipment () as LauncherSO).RangeMultiplier;
        return (slot.GetEquipper ().transform.localPosition - slot.GetEquipper ().GetTarget ().transform.localPosition).sqrMagnitude <= range * range;

    }

    public override void SpawnCharge (EquipmentSlot slot) {

        MissileSO missile = slot.GetCharge () as MissileSO;

        GameObject vfx = Instantiate (missile.MissileStructure.Prefab, slot.GetEquipper ().transform.parent);
        vfx.transform.localPosition = slot.GetLocalPosition ();
        Structure s = vfx.GetComponent<Structure> ();
        s.Initialize ();
        MissileAI ai = s.GetAI () as MissileAI;
        ai.SetTarget ((slot as WeaponSlot).GetTarget ());
        ai.SetMissile (missile);
        ai.SetLauncher (slot.GetEquipment () as LauncherSO);

    }

}
