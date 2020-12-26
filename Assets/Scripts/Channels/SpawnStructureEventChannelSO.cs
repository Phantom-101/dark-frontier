using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu (menuName = "Events/Spawn Structure Event Channel")]

public class SpawnStructureEventChannelSO : ScriptableObject {

    public UnityAction<StructureSO, Structure, Location> OnSpawnStructure;

    public void RaiseEvent (StructureSO structureProfile, Structure spawner, Location location) {

        if (OnSpawnStructure != null) OnSpawnStructure.Invoke (structureProfile, spawner, location);

    }

}
