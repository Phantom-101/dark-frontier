using UnityEngine;

public class ShieldSlot : EquipmentSlot {

    [SerializeField] protected float _strength;

    public ShieldSO Shield { get { return _equipment as ShieldSO; } }
    public float Strength { get => _strength; set { _strength = Mathf.Clamp (value, 0, Shield == null ? 0 : Shield.MaxStrength); } }

    public override void ResetValues () {

        base.ResetValues ();

        Strength = 0;

    }
    public override bool CanEquip (EquipmentSO equipment) {

        return equipment == null || (base.CanEquip (equipment) && equipment is ShieldSO);

    }

}
