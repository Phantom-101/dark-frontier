using UnityEngine;

public class EquipmentSlotComponent : MonoBehaviour, IEquipmentSlot, ISerializable<IEquipmentSlot> {
    /*
     * As a note:
     * Constraints should be data in the equipment slot
     * All other data should be in the data slot
     */
    public EquipmentSlotInstance Data {
        get;
        private set;
    }
    public EquipmentSO Equipment {
        get => Data.Equipment;
    }

    public ISerialized<IEquipmentSlot> GetSerialized () { return Data.GetSerialized (); }
}

