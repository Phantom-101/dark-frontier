using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Engine")]
public class EngineSO : EquipmentSO {

    public float ForwardPower;
    public float TurnPower;
    public float FuelConsumption;

    public override void Tick (EquipmentSlot slot) {

        base.Tick (slot);

        slot.ChangeStoredEnergy (-FuelConsumption * Time.deltaTime);
        slot.TakeDamage (Wear * Time.deltaTime);

    }

    public override void FixedTick (EquipmentSlot slot) {

        if (slot.GetStoredEnergy () > 0) {

            ConstantForce cf = slot.GetEquipper ().GetComponent<ConstantForce> ();
            cf.relativeForce = Vector3.forward * ForwardPower * (slot as EngineSlot).GetForwardSetting ();
            cf.relativeTorque = Vector3.up * TurnPower * (slot as EngineSlot).GetTurnSetting ();

        }

    }

}
