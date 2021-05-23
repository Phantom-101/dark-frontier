using System;
using UnityEngine;

public class NewEquipmentSlot : MonoBehaviour, IEquipmentSlot {
    /*
     * As a note:
     * Constraints should be data in the equipment slot
     * All other data should be in the data slot
     */
    public IEquipmentData Data {
        get;
        private set;
    }
    public string Id {
        get;
        private set;
    }

    public void FixedTick () { if (Data.Equipment != null) Data.Equipment.FixedTick (this); }

    public void Initialize () {
        if (string.IsNullOrWhiteSpace (Id)) Id = Guid.NewGuid ().ToString ();
        Data.Initialize ();
    }

    public void Tick () { if (Data.Equipment != null) Data.Equipment.Tick (this); }
}

