using UnityEngine;

public class ShieldSlot : EquipmentSlot {

    [SerializeReference] protected ShieldStrengths _strengths;

    public ShieldSO GetShield () { return _equipment as ShieldSO; }

    public ShieldStrengths GetStrengths () { return _strengths; }

    public void SetStrengths (ShieldStrengths strengths) { _strengths = strengths; }

    public override bool CanEquip (EquipmentSO equipment) {

        return base.CanEquip (equipment) || (equipment is ShieldSO && equipment.Meta <= _equipper.GetProfile ().MaxMeta);

    }

    protected override void ResetValues () {

        base.ResetValues ();

        _strengths = null;

    }

}
