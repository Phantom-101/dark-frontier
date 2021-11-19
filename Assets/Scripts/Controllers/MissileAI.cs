using DarkFrontier.Equipment;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Items.Prototypes;
using DarkFrontier.Locations;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.Controllers {
    [CreateAssetMenu (menuName = "AI/Missile")]
    public class MissileAI : AIBase {
        public Structure Target;
        public MissileSO Missile;
        public float DamageMultiplier;

        public override void Tick (Structure aStructure, float aDt) {
            if (Target == null || Missile == null) {
                aStructure.UHull = 0;
                return;
            }

            Vector3[] lTarget = new Vector3[2];
            lTarget[0].z = 1;

            float lAngle = aStructure.GetAngleTo (new Location (Target.transform));
            if (lAngle > Missile.HeadingAllowance) lTarget[1].y = 1;
            else if (lAngle < -Missile.HeadingAllowance) lTarget[1].y = -1;

            float lElevation = aStructure.GetElevationTo (new Location (Target.transform));
            if (lElevation > Missile.HeadingAllowance) lTarget[1].x = -1;
            else if (lElevation < -Missile.HeadingAllowance) lTarget[1].x = 1;

            var lEngines = aStructure.UEquipment.States<EnginePrototype.State>();
            var lCount = lEngines.Count;
            for (var lIndex = 0; lIndex < lCount; lIndex++) {
                var lEngine = lEngines[lIndex];
                lEngine.ManagedPropulsion = true;
                lEngine.LinearSetting = lTarget[0];
                lEngine.AngularSetting = lTarget[1];
            }

            if (Singletons.Get<NavigationManager> ().Distance (new Location (Target.transform), new Location (aStructure.transform), DistanceType.Chebyshev) <= Missile.DetonationRange * Target.UPrototype.ApparentSize) {
                Target.TakeDamage (Missile.Damage * DamageMultiplier, new Location (aStructure.transform));
                aStructure.UHull = 0;
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