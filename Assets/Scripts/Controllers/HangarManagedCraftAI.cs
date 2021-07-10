using UnityEngine;

public class HangarManagedCraftAI : AI {
    public HangarLaunchableSO Launchable;
    public HangarBaySlotData HangarBay;

    public override void Tick (Structure structure) {
        if (Launchable == null || HangarBay.Slot == null) {
            structure.Hull = 0;
            return;
        }

        float disToHangar = NavigationManager.Instance.GetLocalDistance (structure, HangarBay.Slot.Equipper);
        float disFromHangarToTarget = NavigationManager.Instance.GetLocalDistance (HangarBay.Target, HangarBay.Slot.Equipper);

        if (disToHangar > Launchable.SignalConnectionRange) return;

        if (HangarBay.Target == null || disFromHangarToTarget > Launchable.MaxOperationalRange) {
            MoveTo (structure, HangarBay.Slot.Equipper);
            return;
        }

        MoveTo (structure, HangarBay.Target);

        structure.Selected = HangarBay.Target;
        structure.Lock (structure.Selected);
        structure.GetEquipmentData<BeamLaserSlotData> ().ForEach (data => {
            data.Activated = false;
            data.Equipment.OnClicked (data.Slot);
            data.Activated = true;
        });
        structure.GetEquipmentData<PulseLaserSlotData> ().ForEach (data => {
            data.Activated = false;
            data.Equipment.OnClicked (data.Slot);
            data.Activated = true;
        });
        structure.GetEquipmentData<LauncherSlotData> ().ForEach (data => {
            data.Activated = false;
            data.Equipment.OnClicked (data.Slot);
            data.Activated = true;
        });
    }

    private void MoveTo (Structure structure, Structure target) {
        Vector3[] targetSettings = new Vector3[2];
        targetSettings[0].z = 1;

        float angle = structure.GetAngleTo (target.transform.localPosition);
        if (angle > Launchable.HeadingAllowance) targetSettings[1].y = 1;
        else if (angle < -Launchable.HeadingAllowance) targetSettings[1].y = -1;

        float elevation = structure.GetElevationTo (target.transform.localPosition);
        if (elevation > Launchable.HeadingAllowance) targetSettings[1].x = -1;
        else if (elevation < -Launchable.HeadingAllowance) targetSettings[1].x = 1;

        structure.GetEquipmentData<EngineSlotData> ().ForEach (engine => {
            engine.ManagedPropulsion = true;
            engine.LinearSetting = targetSettings[0];
            engine.AngularSetting = targetSettings[1];
        });
    }

    public override AI Copy () {
        HangarManagedCraftAI ret = CreateInstance<HangarManagedCraftAI> ();
        ret.Launchable = Launchable;
        ret.HangarBay = HangarBay;
        return ret;
    }
}
