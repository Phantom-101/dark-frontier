using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Engine")]
public class EngineSO : EquipmentSO {

    public float ForwardPower;
    public float TurnPower;

    public override void FixedTick (EquipmentSlot slot) {

        ConstantForce cf = slot.GetEquipper ().GetComponent<ConstantForce> ();
        cf.relativeForce = Vector3.forward * ForwardPower * (slot as EngineSlot).GetForwardSetting ();
        cf.relativeTorque = Vector3.up * TurnPower * (slot as EngineSlot).GetTurnSetting ();

    }

}
