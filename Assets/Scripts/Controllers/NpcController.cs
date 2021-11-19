using DarkFrontier.Equipment;
using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.Controllers {
    public class NpcController : ComponentBehavior {
        public override void Tick(object aTicker, float aDt) {
            if (!(aTicker is Structure lStructure)) return;
            var lEngines = lStructure.UEquipment.States<EnginePrototype.State>();
            var lCount = lEngines.Count;
            for (var lIndex = 0; lIndex < lCount; lIndex++) {
                var lEngine = lEngines[lIndex];
                lEngine.ManagedPropulsion = true;
                lEngine.LinearSetting = Vector3.zero;
                lEngine.AngularSetting = Vector3.zero;
            }
        }
        
        // TODO add nav functions
    }
}