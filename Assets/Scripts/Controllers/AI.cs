using UnityEngine;

[CreateAssetMenu (menuName = "AI/Base")]
public class AI : ScriptableObject {
    public virtual void Tick (Structure structure, float dt) {
        structure.GetEquipmentData<EngineSlotData> ().ForEach (engine => {
            engine.LinearSetting = Vector3.zero;
            engine.AngularSetting = Vector3.zero;
        });
    }

    public virtual AI Copy () => CreateInstance<AI> ();
}
