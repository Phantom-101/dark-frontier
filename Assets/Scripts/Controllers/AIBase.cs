using DarkFrontier.Equipment;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.Controllers {
    [CreateAssetMenu (menuName = "AI/Base")]
    public class AIBase : ScriptableObject {
        public virtual void Tick (Structure aStructure, float aDt) {
            foreach (var lEngine in aStructure.GetEquipmentStates<EnginePrototype.State>()) {
                lEngine.ManagedPropulsion = true;
                lEngine.LinearSetting = Vector3.zero;
                lEngine.AngularSetting = Vector3.zero;
            }
        }

        public virtual AIBase Copy () => CreateInstance<AIBase> ();
    }
}