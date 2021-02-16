using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Engine")]
public class EngineSO : EquipmentSO {

    public float ForwardPower;
    public float TurnPower;
    public float ForwardConsumption;
    public float TurnConsumption;
    public float Damp;
    public float AngularDamp;

    public override void SafeTick (EquipmentSlot slot) {

        slot.Energy -= (ForwardPower * ForwardConsumption + TurnPower * TurnConsumption) * Time.deltaTime;
        slot.Durability -= Wear * Time.deltaTime;

    }

    public override void SafeFixedTick (EquipmentSlot slot) {

        Rigidbody rb = slot.Equipper.GetComponent<Rigidbody> ();
        ConstantForce cf = slot.Equipper.GetComponent<ConstantForce> ();
        EngineSlot engine = slot as EngineSlot;

        if (slot.Energy > 0) {

            rb.drag = Damp * (1 - engine.ForwardSetting / 2);
            rb.angularDrag = AngularDamp * (1 - Mathf.Abs (engine.YawSetting) / 2);

            cf.relativeForce = Vector3.forward * ForwardPower * engine.ForwardSetting * slot.Equipper.GetStatAppliedValue ("speed_multiplier");
            cf.relativeTorque = (Vector3.up * TurnPower * engine.YawSetting + Vector3.left * TurnPower * engine.PitchSetting) * slot.Equipper.GetStatAppliedValue ("angular_speed_multiplier");

        } else {

            rb.drag = Damp;
            rb.angularDrag = AngularDamp;

            cf.relativeForce = Vector3.zero;
            cf.relativeTorque = Vector3.zero;

        }

    }

}
