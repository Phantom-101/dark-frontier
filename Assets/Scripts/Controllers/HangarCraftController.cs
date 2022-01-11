using DarkFrontier.Equipment;
using DarkFrontier.Items.Prototypes;
using DarkFrontier.Positioning.Navigation;
using DarkFrontier.Structures;

namespace DarkFrontier.Controllers
{
    public class HangarCraftController : Controller {
        public HangarLaunchableSO Launchable;
        public HangarBayPrototype.State State;

        public override void Tick(object aTicker, float aDt) {
            if (!(aTicker is Structure s)) return;
            if (Launchable == null || State.Slot == null) {
                s.uHull = 0;
                return;
            }

            if (State.Target == null) {
                MoveTo (s, s.transform, State.Slot.Equipper.transform.position);
                return;
            }

            var lDisToHangar = Navigation.Chebyshev(s.transform.position, State.Slot.Equipper.transform.position);
            var lDisFromHangarToTarget = Navigation.Chebyshev(State.Target.transform.position, State.Slot.Equipper.transform.position);

            if (lDisToHangar > Launchable.SignalConnectionRange) {
                return;
            }

            if (lDisFromHangarToTarget > Launchable.MaxOperationalRange) {
                MoveTo (s, s.transform, State.Slot.Equipper.transform.position);
                return;
            }

            MoveTo (s, s.transform, State.Target.transform.position);

            s.uSelected.UId.Value = State.Target.uId;
            s.Lock (s.uSelected.UValue);
            {
                var lStates = s.uEquipment.States<BeamLaserPrototype.State>();
                var lCount = lStates.Count;
                for (var lIndex = 0; lIndex < lCount; lIndex++) {
                    var lState = lStates[lIndex];
                    lState.Activated = false;
                    lState.Slot.Equipment.OnClicked (lState.Slot);
                    lState.Activated = true;
                }
            }
            {
                var lStates = s.uEquipment.States<PulseLaserPrototype.State>();
                var lCount = lStates.Count;
                for (var lIndex = 0; lIndex < lCount; lIndex++) {
                    var lState = lStates[lIndex];
                    lState.Activated = false;
                    lState.Slot.Equipment.OnClicked (lState.Slot);
                    lState.Activated = true;
                }
            }
            {
                var lStates = s.uEquipment.States<LauncherPrototype.State>();
                var lCount = lStates.Count;
                for (var lIndex = 0; lIndex < lCount; lIndex++) {
                    var lState = lStates[lIndex];
                    lState.Activated = false;
                    lState.Slot.Equipment.OnClicked (lState.Slot);
                    lState.Activated = true;
                }
            }
        }
    }
}