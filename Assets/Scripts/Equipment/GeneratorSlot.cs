public class GeneratorSlot : EquipmentSlot {

    GeneratorSO Generator { get { return _equipment as GeneratorSO; } }

    public override bool CanEquip (EquipmentSO equipment) {

        return base.CanEquip (equipment) || (equipment is GeneratorSO && equipment.Tier <= Equipper.GetProfile ().MaxEquipmentTier);

    }

}
