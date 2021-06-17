using UnityEngine;

[CreateAssetMenu (menuName = "AI/Missile")]
public class MissileAI : AI {
    public Structure Target;
    public MissileSO Missile;
    public Damage DamageMultiplier;
    public float RangeMultiplier;

    protected StructureStatModifier statModifier;

    public override AI GetAI () {
        MissileAI ai = CreateInstance<MissileAI> ();
        ai.Missile = Missile;
        ai.DamageMultiplier = DamageMultiplier;
        ai.RangeMultiplier = RangeMultiplier;
        return ai;
    }

    public override void Tick (Structure structure) {
        if (Target == null || Missile == null) {
            structure.Hull = 0;
            return;
        }

        if (statModifier == null) {
            statModifier = new StructureStatModifier {
                Name = "Launcher Range Modifier",
                Id = "launcher-range-modifier",
                Value = RangeMultiplier,
                Type = StructureStatModifierType.Multiplicative,
                Duration = 100,
            };
            structure.AddStatModifier (StructureStatType.LinearSpeedMultiplier, statModifier);
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
}
