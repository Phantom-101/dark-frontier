using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Shield")]
public class ShieldSO : EquipmentSO {

    public float Radius;
    public float MaxStrength;
    public float RechargeRate;

    public override void OnAwake (EquipmentSlot slot) {

        ShieldSlot shield = slot as ShieldSlot;
        if (shield.Shield != null) OnEquip (slot);

    }

    public override void OnEquip (EquipmentSlot slot) {

        ShieldSlot shield = slot as ShieldSlot;
        ShieldSO s = shield.Shield;
        if (s != null) shield.Strength = s.MaxStrength;

    }

    public override void SafeTick (EquipmentSlot slot) {

        ShieldSlot shield = slot as ShieldSlot;
        ShieldSO s = shield.Shield;
        shield.Strength += s.RechargeRate * Time.deltaTime;
        slot.Durability -= Wear * Time.deltaTime;

    }

}
