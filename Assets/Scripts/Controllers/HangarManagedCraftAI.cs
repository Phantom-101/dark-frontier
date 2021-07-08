using UnityEngine;

public class HangarManagedCraftAI : AI {
    public CraftSO Craft;
    public HangarBaySlotData HangarBay;

    public override void Tick (Structure structure) {
        if (HangarBay.Target == null) {
            structure.GetEquipmentData<EngineSlotData> ().ForEach (engine => {
                engine.ManagedPropulsion = true;
                engine.LinearSetting = Vector3.zero;
                engine.AngularSetting = Vector3.zero;
            });
            return;
        }

        Vector3[] target = new Vector3[2];
        target[0].z = 1;

        float angle = structure.GetAngleTo (HangarBay.Target.transform.localPosition);
        if (angle > Craft.HeadingAllowance) target[1].y = 1;
        else if (angle < -Craft.HeadingAllowance) target[1].y = -1;

        float elevation = structure.GetElevationTo (HangarBay.Target.transform.localPosition);
        if (elevation > Craft.HeadingAllowance) target[1].x = -1;
        else if (elevation < -Craft.HeadingAllowance) target[1].x = 1;

        structure.GetEquipmentData<EngineSlotData> ().ForEach (engine => {
            engine.ManagedPropulsion = true;
            engine.LinearSetting = target[0];
            engine.AngularSetting = target[1];
        });

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

    public override AI Copy () {
        HangarManagedCraftAI ret = CreateInstance<HangarManagedCraftAI> ();
        ret.Craft = Craft;
        ret.HangarBay = HangarBay;
        return ret;
    }
}
