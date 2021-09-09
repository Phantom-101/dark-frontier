using DarkFrontier.Equipment;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.AI {
    [CreateAssetMenu (menuName = "AI/Missile")]
    public class MissileAI : AIBase {
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

            structure.GetEquipmentStates<EnginePrototype.State> ().ForEach (state => {
                state.ManagedPropulsion = true;
                state.LinearSetting = target[0];
                state.AngularSetting = target[1];
            });

            if (NavigationManager.Instance.GetLocalDistance (Target, structure) <= Missile.DetonationRange) {
                Target.TakeDamage (Missile.Damage * DamageMultiplier, structure.transform.position);
                structure.Hull = 0;
            }
        }

        public override AIBase Copy () {
            MissileAI ret = CreateInstance<MissileAI> ();
            ret.Target = Target;
            ret.Missile = Missile;
            ret.DamageMultiplier = DamageMultiplier;
            return ret;
        }
    }
}