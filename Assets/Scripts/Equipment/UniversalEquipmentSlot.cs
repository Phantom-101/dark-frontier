using UnityEngine;

public class UniversalEquipmentSlot : MonoBehaviour {
    /*
     * As a note:
     * Constraints should be data in the equipment slot
     * All other data should be in the data slot
     */
    public EquipmentSlotInstance Data {
        get;
        private set;
    }
}

