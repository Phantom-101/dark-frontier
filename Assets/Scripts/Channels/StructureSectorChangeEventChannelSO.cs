using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu (menuName = "Events/Structure Sector Changed Event Channel")]
public class StructureSectorChangeEventChannelSO : ScriptableObject {

    public UnityAction<Structure, Sector, Sector> OnStructureSectorChanged;

    public void RaiseEvent (Structure structure, Sector prevSector, Sector newSector) {

        if (OnStructureSectorChanged != null) OnStructureSectorChanged.Invoke (structure, prevSector, newSector);

    }

}
