public class GeneratorSlot : EquipmentSlot {

    public GeneratorSO GetGenerator () { return _equipment as GeneratorSO; }

    public override bool CanEquip (EquipmentSO equipment) {

        return base.CanEquip (equipment) || (equipment is GeneratorSO && equipment.Meta <= _equipper.GetProfile ().MaxMeta);

    }

}
