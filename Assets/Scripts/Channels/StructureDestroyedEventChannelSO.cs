using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu (menuName = "Events/Structure Destroyed Event Channel")]
public class StructureDestroyedEventChannelSO : ScriptableObject {

    public UnityAction<Structure, StructureSO> OnStructureDestroyed;

    public void RaiseEvent (Structure destroyedStructure, StructureSO destroyedType) {

        if (OnStructureDestroyed != null) OnStructureDestroyed.Invoke (destroyedStructure, destroyedType);

    }

}
