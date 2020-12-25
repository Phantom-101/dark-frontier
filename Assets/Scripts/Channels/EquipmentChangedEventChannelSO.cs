using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu (menuName = "Events/Equipment Changed Event Channel")]
public class EquipmentChangedEventChannelSO : ScriptableObject {

    public UnityAction<Structure, EquipmentSlot, EquipmentSO, EquipmentSO> OnChanged;

    public void RaiseEvent (Structure structure, EquipmentSlot slot, EquipmentSO prevEquipment, EquipmentSO newEquipment) {

        if (OnChanged != null) OnChanged.Invoke (structure, slot, prevEquipment, newEquipment);

    }

}
