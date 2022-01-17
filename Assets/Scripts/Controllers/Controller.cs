using DarkFrontier.Equipment;
using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Positioning.Navigation;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.Controllers
{
    public class Controller : ComponentBehavior {
        public override void Tick(object aTicker, float aDt) {
            if (!(aTicker is Structure s)) return;
            Stop(s);
        }

        protected void Stop(Structure s) => SetEngines(s, Vector3.zero, Vector3.zero);
        protected void MoveTo(Structure s, Transform t, Vector3 p) => SetEngines(s, Vector3.forward, new Vector3(-Navigation.AltitudeSign(t, p), Navigation.AzimuthSign(t, p), 0));
        protected void TurnTo(Structure s, Transform t, Vector3 p) => SetEngines(s, Vector3.zero, new Vector3(-Navigation.AltitudeSign(t, p), Navigation.AzimuthSign(t, p), 0));
        
        protected void SetEngines(Structure s, Vector3 linear, Vector3 angular) {
            var equipment = s.uEquipment.uAll;
            var len = equipment.Count;
            for (var i = 0; i < len; i++) {
                if (equipment[i].UState is not EnginePrototype.State engine) continue;
                engine.ManagedPropulsion = true;
                engine.LinearSetting = linear;
                engine.AngularSetting = angular;
            }
        }
    }
}