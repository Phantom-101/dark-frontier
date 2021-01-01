using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Shield")]
public class ShieldSO : EquipmentSO {

    public float[] MaxStrengths;
    public float[] RechargeRates;

    public override void OnAwake (EquipmentSlot slot) {

        ShieldSlot shield = slot as ShieldSlot;

        if (shield.GetShield () != null && shield.GetStrengths () == null) OnEquip (slot);

    }

    public override void OnEquip (EquipmentSlot slot) {

        ShieldSlot shield = slot as ShieldSlot;

        if (shield.GetEquipment () != null) {

            ShieldSO so = shield.GetShield ();

            shield.SetStrengths (new ShieldStrengths (slot as ShieldSlot, so.MaxStrengths, so.MaxStrengths, so.RechargeRates));

        }

    }

    public override void Tick (EquipmentSlot slot) {

        ShieldSlot shield = slot as ShieldSlot;

        shield.GetStrengths ().Tick ();

        slot.TakeDamage (Wear * Time.deltaTime);

    }

}
