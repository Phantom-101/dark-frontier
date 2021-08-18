using DarkFrontier.Structures;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Missile")]
public class MissileAI : AI {
    public Structure Target;
    public MissileSO Missile;
    public float DamageMultiplier;

    public override void Tick (Structure structure, float dt) {
        if (Target == null || Missile == null) {
            structure.Hull = 0;
            return;
        }

        Vector3[] target = new Vector3[2];
        target[0].z = 1;

        float angle = structure.GetAngleTo (Target.transform.localPosition);
        if (angle > Missile.HeadingAllowance) target[1].y = 1;
        else if (angle < -Missile.HeadingAllowance) target[1].y = -1;

        float elevation = structure.GetElevationTo (Target.transform.localPosition);
        if (elevation > Missile.HeadingAllowance) target[1].x = -1;
        else if (elevation < -Missile.HeadingAllowance) target[1].x = 1;

        structure.GetEquipmentData<EngineSlotData> ().ForEach (engine => {
            engine.LinearSetting = target[0];
            engine.AngularSetting = target[1];
        });

        if (NavigationManager.Instance.GetLocalDistance (Target, structure) <= Missile.DetonationRange) {
            Target.TakeDamage (Missile.Damage * DamageMultiplier, structure.transform.position);
            structure.Hull = 0;
        }
    }

    public override AI Copy () {
        MissileAI ret = CreateInstance<MissileAI> ();
        ret.Target = Target;
        ret.Missile = Missile;
        ret.DamageMultiplier = DamageMultiplier;
        return ret;
    }
}
