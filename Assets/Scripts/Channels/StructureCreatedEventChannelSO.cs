using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu (menuName = "Events/Structure Created Event Channel")]

public class StructureCreatedEventChannelSO : ScriptableObject {

    public UnityAction<StructureSO, Faction, Sector, Location> OnStructureCreated;

    public void RaiseEvent (StructureSO structureProfile, Faction owner, Sector sector, Location location) {

        if (OnStructureCreated != null) OnStructureCreated.Invoke (structureProfile, owner, sector, location);

    }

}
