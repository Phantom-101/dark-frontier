using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Engine")]
public class EngineSO : EquipmentSO {

    public float ForwardPower;
    public float TurnPower;
    public float FuelConsumption;
    public float Damp;
    public float AngularDamp;

    public override void Tick (EquipmentSlot slot) {

        base.Tick (slot);

        slot.ChangeStoredEnergy (-FuelConsumption * Time.deltaTime);
        slot.TakeDamage (Wear * Time.deltaTime);

    }

    public override void FixedTick (EquipmentSlot slot) {

        Rigidbody rb = slot.GetEquipper ().GetComponent<Rigidbody> ();
        ConstantForce cf = slot.GetEquipper ().GetComponent<ConstantForce> ();
        EngineSlot engine = slot as EngineSlot;

        if (slot.GetStoredEnergy () > 0) {

            rb.drag = Damp * (1 - engine.GetForwardSetting () / 2);
            rb.angularDrag = AngularDamp * (1 - Mathf.Abs (engine.GetTurnSetting ()) / 2);

            cf.relativeForce = Vector3.forward * ForwardPower * (slot as EngineSlot).GetForwardSetting ();
            cf.relativeTorque = Vector3.up * TurnPower * (slot as EngineSlot).GetTurnSetting () + Vector3.left * TurnPower * (slot as EngineSlot).GetPitchSetting ();

        } else {

            rb.drag = Damp;
            rb.angularDrag = AngularDamp;

            cf.relativeForce = Vector3.zero;
            cf.relativeTorque = Vector3.zero;

        }

    }

}
