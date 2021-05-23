using UnityEngine.EventSystems;

public class EquipmentButton : EventTrigger {
    public IEquipmentSlot Slot;

    public override void OnBeginDrag (PointerEventData data) {
        if (Slot != null && Slot.Data?.Equipment != null) Slot.Data.Equipment.OnBeginDrag (Slot, data);
    }

    public override void OnCancel (BaseEventData data) {
        if (Slot != null && Slot.Data?.Equipment != null) Slot.Data.Equipment.OnCancel (Slot, data);
    }

    public override void OnDeselect (BaseEventData data) {
        if (Slot != null && Slot.Data?.Equipment != null) Slot.Data.Equipment.OnDeselect (Slot, data);
    }

    public override void OnDrag (PointerEventData data) {
        if (Slot != null && Slot.Data?.Equipment != null) Slot.Data.Equipment.OnDrag (Slot, data);
    }

    public override void OnDrop (PointerEventData data) {
        if (Slot != null && Slot.Data?.Equipment != null) Slot.Data.Equipment.OnDrop (Slot, data);
    }

    public override void OnEndDrag (PointerEventData data) {
        if (Slot != null && Slot.Data?.Equipment != null) Slot.Data.Equipment.OnEndDrag (Slot, data);
    }

    public override void OnInitializePotentialDrag (PointerEventData data) {
        if (Slot != null && Slot.Data?.Equipment != null) Slot.Data.Equipment.OnInitializePotentialDrag (Slot, data);
    }

    public override void OnMove (AxisEventData data) {
        if (Slot != null && Slot.Data?.Equipment != null) Slot.Data.Equipment.OnMove (Slot, data);
    }

    public override void OnPointerClick (PointerEventData data) {
        if (Slot != null && Slot.Data?.Equipment != null) Slot.Data.Equipment.OnPointerClick (Slot, data);
    }

    public override void OnPointerDown (PointerEventData data) {
        if (Slot != null && Slot.Data?.Equipment != null) Slot.Data.Equipment.OnPointerDown (Slot, data);
    }

    public override void OnPointerEnter (PointerEventData data) {
        if (Slot != null && Slot.Data?.Equipment != null) Slot.Data.Equipment.OnPointerEnter (Slot, data);
    }

    public override void OnPointerExit (PointerEventData data) {
        if (Slot != null && Slot.Data?.Equipment != null) Slot.Data.Equipment.OnPointerExit (Slot, data);
    }

    public override void OnPointerUp (PointerEventData data) {
        if (Slot != null && Slot.Data?.Equipment != null) Slot.Data.Equipment.OnPointerUp (Slot, data);
    }

    public override void OnScroll (PointerEventData data) {
        if (Slot != null && Slot.Data?.Equipment != null) Slot.Data.Equipment.OnScroll (Slot, data);
    }

    public override void OnSelect (BaseEventData data) {
        if (Slot != null && Slot.Data?.Equipment != null) Slot.Data.Equipment.OnSelect (Slot, data);
    }

    public override void OnSubmit (BaseEventData data) {
        if (Slot != null && Slot.Data?.Equipment != null) Slot.Data.Equipment.OnSubmit (Slot, data);
    }

    public override void OnUpdateSelected (BaseEventData data) {
        if (Slot != null && Slot.Data?.Equipment != null) Slot.Data.Equipment.OnUpdateSelected (Slot, data);
    }
}
