using DarkFrontier.Equipment;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.AI {
    public class HangarManagedCraftAI : AIBase {
        public HangarLaunchableSO Launchable;
        public HangarBayPrototype.State State;

        public override void Tick (Structure structure, float dt) {
            if (Launchable == null || State.Slot == null) {
                structure.Hull = 0;
                return;
            }

            if (State.Target == null) {
                MoveTo (structure, State.Slot.Equipper);
                return;
            }

            float disToHangar = NavigationManager.Instance.GetLocalDistance (structure, State.Slot.Equipper);
            float disFromHangarToTarget = NavigationManager.Instance.GetLocalDistance (State.Target, State.Slot.Equipper);

            if (disToHangar > Launchable.SignalConnectionRange) {
                return;
            }

            if (disFromHangarToTarget > Launchable.MaxOperationalRange) {
                MoveTo (structure, State.Slot.Equipper);
                return;
            }

            MoveTo (structure, State.Target);

            structure.Selected = State.Target;
            structure.Lock (structure.Selected);
            structure.GetEquipmentStates<BeamLaserPrototype.State> ().ForEach (state => {
                state.Activated = false;
                state.Slot.Equipment.OnClicked (state.Slot);
                state.Activated = true;
            });
            structure.GetEquipmentStates<PulseLaserPrototype.State> ().ForEach (state => {
                state.Activated = false;
                state.Slot.Equipment.OnClicked (state.Slot);
                state.Activated = true;
            });
            structure.GetEquipmentStates<LauncherPrototype.State> ().ForEach (state => {
                state.Activated = false;
                state.Slot.Equipment.OnClicked (state.Slot);
                state.Activated = true;
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

            structure.GetEquipmentStates<EnginePrototype.State> ().ForEach (state => {
                state.ManagedPropulsion = true;
                state.LinearSetting = targetSettings[0];
                state.AngularSetting = targetSettings[1];
            });
        }

        public override AIBase Copy () {
            HangarManagedCraftAI ret = CreateInstance<HangarManagedCraftAI> ();
            ret.Launchable = Launchable;
            ret.State = State;
            return ret;
        }
    }
}