using DarkFrontier.Equipment;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Items.Prototypes;
using DarkFrontier.Locations;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.Controllers {
    public class MissileNpcController : NpcController {
        public Structure Target;
        public MissileSO Missile;
        public float DamageMultiplier;

        public override void Tick(object aTicker, float aDt) {
            if (!(aTicker is Structure lStructure)) return;
            if (Target == null || Missile == null) {
                lStructure.UHull = 0;
                return;
            }

            var lTarget = new Vector3[2];
            lTarget[0].z = 1;

            var lAngle = lStructure.GetAngleTo (new Location (Target.transform));
            if (lAngle > Missile.HeadingAllowance) lTarget[1].y = 1;
            else if (lAngle < -Missile.HeadingAllowance) lTarget[1].y = -1;

            var lElevation = lStructure.GetElevationTo (new Location (Target.transform));
            if (lElevation > Missile.HeadingAllowance) lTarget[1].x = -1;
            else if (lElevation < -Missile.HeadingAllowance) lTarget[1].x = 1;

            var lEngines = lStructure.UEquipment.States<EnginePrototype.State>();
            var lCount = lEngines.Count;
            for (var lIndex = 0; lIndex < lCount; lIndex++) {
                var lEngine = lEngines[lIndex];
                lEngine.ManagedPropulsion = true;
                lEngine.LinearSetting = lTarget[0];
                lEngine.AngularSetting = lTarget[1];
            }

            if (Singletons.Get<NavigationManager> ().Distance (new Location (Target.transform), new Location (lStructure.transform), DistanceType.Chebyshev) <= Missile.DetonationRange * Target.UPrototype.ApparentSize) {
                Target.TakeDamage (Missile.Damage * DamageMultiplier, new Location (lStructure.transform));
                lStructure.UHull = 0;
            }
        }
    }
}