using UnityEngine;

public class EquipmentIndicator : MonoBehaviour {

    [SerializeField] protected EquipmentSlot _slot;

    public EquipmentSlot GetSlot () { return _slot; }

    public void SetSlot (EquipmentSlot slot) { _slot = slot; }

}
