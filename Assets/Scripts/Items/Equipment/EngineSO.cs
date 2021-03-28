using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Engine")]
public class EngineSO : EquipmentSO {

    public float MaxLinearSpeed;
    public float MaxAngularSpeed;
    [Tooltip ("x+, x-\ny+, y-\nz+, z-")]
    public float3x2 LinearForce;
    [Tooltip ("x+, x-\ny+, y-\nz+, z-")]
    public float3x2 AngularForce;
    [Tooltip ("x+, x-\ny+, y-\nz+, z-")]
    public float3x2 LinearConsumption;
    [Tooltip ("x+, x-\ny+, y-\nz+, z-")]
    public float3x2 AngularConsumption;
    public float InertialFactor;

    public override void SafeTick (EquipmentSlot slot) {

        slot.Energy -= GetConsumption (slot) * Time.deltaTime;
        slot.Durability -= Wear * Time.deltaTime;

    }

    public override void SafeFixedTick (EquipmentSlot slot) {

        Rigidbody rb = slot.Equipper.GetComponent<Rigidbody> ();
        ConstantForce cf = slot.Equipper.GetComponent<ConstantForce> ();

        rb.drag = 0;
        rb.angularDrag = 0;

        if (slot.Energy > 0) {

            if (rb.velocity.sqrMagnitude > MaxLinearSpeed * MaxLinearSpeed) rb.velocity = rb.velocity.normalized * MaxLinearSpeed;
            rb.maxAngularVelocity = MaxAngularSpeed * Mathf.Deg2Rad;

            float3x2 accels = GetAccelerations (slot);
            cf.relativeForce = accels[0];
            cf.relativeTorque = accels[1];

        } else {

            cf.relativeForce = Vector3.zero;
            cf.relativeTorque = Vector3.zero;

        }

    }

    public virtual float3x2 GetAccelerations (EquipmentSlot slot) {

        Rigidbody rb = slot.Equipper.GetComponent<Rigidbody> ();
        EngineSlot engine = slot as EngineSlot;

        float3x2 res = new float3x2 ();
        // Linear
        for (int d = 0; d < 3; d++) {
            float cur = slot.Equipper.transform.InverseTransformDirection (rb.velocity)[d] / MaxLinearSpeed;
            float dif = engine.Settings[0][d] - cur;
            float mul = Mathf.Pow (Mathf.Abs (dif), InertialFactor) * Mathf.Sign (dif);
            res[0][d] = mul > 0 ? mul * LinearForce[0][d] : -mul * LinearForce[1][d];
            res[0][d] *= slot.Equipper.GetStatAppliedValue ("linear_speed_multiplier");
        }
        // Angular
        for (int d = 0; d < 3; d++) {
            float cur = slot.Equipper.transform.InverseTransformDirection (rb.angularVelocity * Mathf.Rad2Deg)[d] / MaxAngularSpeed;
            float dif = engine.Settings[1][d] - cur;
            float mul = Mathf.Pow (Mathf.Abs (dif), InertialFactor) * Mathf.Sign (dif);
            res[1][d] = mul > 0 ? mul * AngularForce[0][d] : -mul * AngularForce[1][d];
            res[1][d] *= slot.Equipper.GetStatAppliedValue ("angular_speed_multiplier");
        }
        return res;

    }

    public virtual float GetConsumption (EquipmentSlot slot) {

        Rigidbody rb = slot.Equipper.GetComponent<Rigidbody> ();
        EngineSlot engine = slot as EngineSlot;

        float res = 0;
        // Linear
        for (int d = 0; d < 3; d++) {
            float cur = slot.Equipper.transform.InverseTransformDirection (rb.velocity)[d] / MaxLinearSpeed;
            float dif = engine.Settings[0][d] - cur;
            float mul = Mathf.Pow (Mathf.Abs (dif), InertialFactor) * Mathf.Sign (dif);
            res += mul > 0 ? mul * LinearConsumption[0][d] : -mul * LinearConsumption[1][d];
        }
        // Angular
        for (int d = 0; d < 3; d++) {
            float cur = slot.Equipper.transform.InverseTransformDirection (rb.angularVelocity * Mathf.Rad2Deg)[d] / MaxAngularSpeed;
            float dif = engine.Settings[1][d] - cur;
            float mul = Mathf.Pow (Mathf.Abs (dif), InertialFactor) * Mathf.Sign (dif);
            res += mul > 0 ? mul * AngularConsumption[0][d] : -mul * AngularConsumption[1][d];
        }
        return res;

    }

}
