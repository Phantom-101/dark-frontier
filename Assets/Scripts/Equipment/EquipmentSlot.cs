using UnityEngine;

public class EquipmentSlot : MonoBehaviour {
    public Structure Equipper;
    [SerializeReference] public EquipmentSlotData Data = new EquipmentSlotData ();

    public virtual void Tick () { if (Data.Equipment != null) Data.Equipment.Tick (this); }

    public virtual void FixedTick () { if (Data.Equipment != null) Data.Equipment.FixedTick (this); }
}
