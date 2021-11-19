using DarkFrontier.Equipment;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Items.Prototypes;
using DarkFrontier.Locations;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.Controllers {
    public class HangarManagedCraftAI : AIBase {
        public HangarLaunchableSO Launchable;
        public HangarBayPrototype.State State;

        public override void Tick (Structure structure, float aDt) {
            if (Launchable == null || State.Slot == null) {
                structure.UHull = 0;
                return;
            }

            if (State.Target == null) {
                MoveTo (structure, State.Slot.Equipper);
                return;
            }

            float disToHangar = Singletons.Get<NavigationManager> ().Distance (new Location (structure.transform), new Location (State.Slot.Equipper.transform), DistanceType.Chebyshev);
            float disFromHangarToTarget = Singletons.Get<NavigationManager> ().Distance (new Location (State.Target.transform), new Location (State.Slot.Equipper.transform), DistanceType.Chebyshev);

            if (disToHangar > Launchable.SignalConnectionRange) {
                return;
            }

            if (disFromHangarToTarget > Launchable.MaxOperationalRange) {
                MoveTo (structure, State.Slot.Equipper);
                return;
            }

            MoveTo (structure, State.Target);

            structure.USelected.UId.Value = State.Target.UId;
            structure.Lock (structure.USelected.UValue);
            {
                var lStates = structure.UEquipment.States<BeamLaserPrototype.State>();
                var lCount = lStates.Count;
                for (var lIndex = 0; lIndex < lCount; lIndex++) {
                    var lState = lStates[lIndex];
                    lState.Activated = false;
                    lState.Slot.Equipment.OnClicked (lState.Slot);
                    lState.Activated = true;
                }
            }
            {
                var lStates = structure.UEquipment.States<PulseLaserPrototype.State>();
                var lCount = lStates.Count;
                for (var lIndex = 0; lIndex < lCount; lIndex++) {
                    var lState = lStates[lIndex];
                    lState.Activated = false;
                    lState.Slot.Equipment.OnClicked (lState.Slot);
                    lState.Activated = true;
                }
            }
            {
                var lStates = structure.UEquipment.States<LauncherPrototype.State>();
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

        public override AIBase Copy () {
            HangarManagedCraftAI ret = CreateInstance<HangarManagedCraftAI> ();
            ret.Launchable = Launchable;
            ret.State = State;
            return ret;
        }
    }
}