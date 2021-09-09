using DarkFrontier.Equipment;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.AI {
    [CreateAssetMenu (menuName = "AI/Base")]
    public class AIBase : ScriptableObject {
        public virtual void Tick (Structure structure, float dt) {
            structure.GetEquipmentStates<EnginePrototype.State> ().ForEach (state => {
                state.ManagedPropulsion = true;
                state.LinearSetting = Vector3.zero;
                state.AngularSetting = Vector3.zero;
            });
        }

        public virtual AIBase Copy () => CreateInstance<AIBase> ();
    }
}