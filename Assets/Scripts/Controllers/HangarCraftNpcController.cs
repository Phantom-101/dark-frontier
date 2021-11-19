using DarkFrontier.Equipment;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Items.Prototypes;
using DarkFrontier.Locations;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.Controllers {
    public class HangarCraftNpcController : NpcController {
        public HangarLaunchableSO Launchable;
        public HangarBayPrototype.State State;

        public override void Tick(object aTicker, float aDt) {
            if (!(aTicker is Structure lStructure)) return;
            if (Launchable == null || State.Slot == null) {
                lStructure.UHull = 0;
                return;
            }

            if (State.Target == null) {
                MoveTo (lStructure, State.Slot.Equipper);
                return;
            }

            var lDisToHangar = Singletons.Get<NavigationManager> ().Distance (new Location (lStructure.transform), new Location (State.Slot.Equipper.transform), DistanceType.Chebyshev);
            var lDisFromHangarToTarget = Singletons.Get<NavigationManager> ().Distance (new Location (State.Target.transform), new Location (State.Slot.Equipper.transform), DistanceType.Chebyshev);

            if (lDisToHangar > Launchable.SignalConnectionRange) {
                return;
            }

            if (lDisFromHangarToTarget > Launchable.MaxOperationalRange) {
                MoveTo (lStructure, State.Slot.Equipper);
                return;
            }

            MoveTo (lStructure, State.Target);

            lStructure.USelected.UId.Value = State.Target.UId;
            lStructure.Lock (lStructure.USelected.UValue);
            {
                var lStates = lStructure.UEquipment.States<BeamLaserPrototype.State>();
                var lCount = lStates.Count;
                for (var lIndex = 0; lIndex < lCount; lIndex++) {
                    var lState = lStates[lIndex];
                    lState.Activated = false;
                    lState.Slot.Equipment.OnClicked (lState.Slot);
                    lState.Activated = true;
                }
            }
            {
                var lStates = lStructure.UEquipment.States<PulseLaserPrototype.State>();
                var lCount = lStates.Count;
                for (var lIndex = 0; lIndex < lCount; lIndex++) {
                    var lState = lStates[lIndex];
                    lState.Activated = false;
                    lState.Slot.Equipment.OnClicked (lState.Slot);
                    lState.Activated = true;
                }
            }
            {
                var lStates = lStructure.UEquipment.States<LauncherPrototype.State>();
                var lCount = lStates.Count;
                for (var lIndex = 0; lIndex < lCount; lIndex++) {
                    var lState = lStates[lIndex];
                    lState.Activated = false;
                    lState.Slot.Equipment.OnClicked (lState.Slot);
                    lState.Activated = true;
                }
            }
        }

        private void MoveTo (Structure aStructure, Structure aTarget) {
            Vector3[] lTarget = new Vector3[2];
            lTarget[0].z = 1;

            float lAngle = aStructure.GetAngleTo (new Location (aTarget.transform));
            if (lAngle > Launchable.HeadingAllowance) lTarget[1].y = 1;
            else if (lAngle < -Launchable.HeadingAllowance) lTarget[1].y = -1;

            float lElevation = aStructure.GetElevationTo (new Location (aTarget.transform));
            if (lElevation > Launchable.HeadingAllowance) lTarget[1].x = -1;
            else if (lElevation < -Launchable.HeadingAllowance) lTarget[1].x = 1;

            var lEngines = aStructure.UEquipment.States<EnginePrototype.State>();
            var lCount = lEngines.Count;
            for (var lIndex = 0; lIndex < lCount; lIndex++) {
                var lEngine = lEngines[lIndex];
                lEngine.ManagedPropulsion = true;
                lEngine.LinearSetting = lTarget[0];
                lEngine.AngularSetting = lTarget[1];
            }
        }
    }
}