public class GeneratorSlot : EquipmentSlot {

    GeneratorSO Generator { get { return _equipment as GeneratorSO; } }

    public override bool CanEquip (EquipmentSO equipment) {

        return equipment == null || (base.CanEquip (equipment) && equipment is GeneratorSO);

    }

}
