public class WeaponSO : EquipmentSO {

    public override void OnCycleStart (EquipmentSlot slot) { (slot as WeaponSlot).Target = slot.Equipper.GetTarget (); }

}
