using UnityEngine.EventSystems;

public interface IEquipment : IIdentifiable, IInfo {
    int Tier {
        get;
    }
    EquipmentSize Size {
        get;
    }
    bool ShowUI {
        get;
    }

    void OnEquip (IEquipmentSlot slot);

    void OnUnequip (IEquipmentSlot slot);

    void Tick (IEquipmentSlot slot);

    void FixedTick (IEquipmentSlot slot);

    void OnPointerEnter (IEquipmentSlot slot, PointerEventData data);

    void OnPointerExit (IEquipmentSlot slot, PointerEventData data);

    void OnPointerDown (IEquipmentSlot slot, PointerEventData data);

    void OnPointerUp (IEquipmentSlot slot, PointerEventData data);

    void OnPointerClick (IEquipmentSlot slot, PointerEventData data);

    void OnDrag (IEquipmentSlot slot, PointerEventData data);

    void OnDrop (IEquipmentSlot slot, PointerEventData data);

    void OnScroll (IEquipmentSlot slot, PointerEventData data);

    void OnUpdateSelected (IEquipmentSlot slot, BaseEventData data);

    void OnSelect (IEquipmentSlot slot, BaseEventData data);

    void OnDeselect (IEquipmentSlot slot, BaseEventData data);

    void OnMove (IEquipmentSlot slot, AxisEventData data);

    void OnInitializePotentialDrag (IEquipmentSlot slot, PointerEventData data);

    void OnBeginDrag (IEquipmentSlot slot, PointerEventData data);

    void OnEndDrag (IEquipmentSlot slot, PointerEventData data);

    void OnSubmit (IEquipmentSlot slot, BaseEventData data);

    void OnCancel (IEquipmentSlot slot, BaseEventData data);
}
