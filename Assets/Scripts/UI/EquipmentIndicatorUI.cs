using UnityEngine;

public class EquipmentIndicatorUI : MonoBehaviour {

    [SerializeField] protected EquipmentSlot _slot;

    public EquipmentSlot Slot { get => _slot; set => _slot = value; }

}
