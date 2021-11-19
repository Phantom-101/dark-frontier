using DarkFrontier.Equipment;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.Controllers {
    [CreateAssetMenu (menuName = "AI/Base")]
    public class AIBase : ScriptableObject {
        public virtual void Tick (Structure aStructure, float aDt) {
            var lEngines = aStructure.UEquipment.States<EnginePrototype.State>();
            var lCount = lEngines.Count;
            for (var lIndex = 0; lIndex < lCount; lIndex++) {
                var lEngine = lEngines[lIndex];
                lEngine.ManagedPropulsion = true;
                lEngine.LinearSetting = Vector3.zero;
                lEngine.AngularSetting = Vector3.zero;
            }
        }

        public virtual AIBase Copy () => CreateInstance<AIBase> ();
    }
}