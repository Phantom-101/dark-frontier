using System;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Engine")]
public class EngineSO : EquipmentSO {
    public float MaxLinearSpeed;
    public float MaxAngularSpeed;
    public Vector3 LinearForcePos;
    public Vector3 LinearForceNeg;
    public Vector3 AngularForcePos;
    public Vector3 AngularForceNeg;
    public Vector3 LinearConsumptionPos;
    public Vector3 LinearConsumptionNeg;
    public Vector3 AngularConsumptionPos;
    public Vector3 AngularConsumptionNeg;
    public float CorrectionSlack;
    public float LinearSleepThreshold;
    public float AngularSleepThreshold;
    public float BankAmount;

    public override void OnAwake (EquipmentSlot slot) {
        EnsureDataType (slot);
    }

    public override void OnEquip (EquipmentSlot slot) {
        slot.Data = new EngineSlotData {
            Slot = slot,
            Equipment = this,
            Durability = Durability,
            LinearSetting = Vector3.zero,
            AngularSetting = Vector3.zero,
            EnergySatisfaction = 0,
        };
    }

    public override void OnUnequip (EquipmentSlot slot) {
        slot.Data = new EquipmentSlotData {
            Slot = slot,
            Equipment = null,
            Durability = 0,
        };
    }

    public override void Tick (EquipmentSlot slot) {
        EnsureDataType (slot);

        float consumption = GetConsumption (slot) * Time.deltaTime;
        float given = 0;
        slot.Equipper.GetEquipmentData<CapacitorSlotData> ().ForEach (capacitor => {
            float chargeLeft = capacitor.Charge;
            float dischargeLeft = capacitor.DischargeLeft;
            float allocated = Mathf.Min (chargeLeft, dischargeLeft, consumption - given);
            given += allocated;
            capacitor.Charge -= allocated;
            capacitor.DischargeLeft -= allocated;
        });

        (slot.Data as EngineSlotData).EnergySatisfaction = Mathf.Clamp01 (given / (consumption == 0 ? 1 : consumption));
    }

    public override void FixedTick (EquipmentSlot slot) {
        EnsureDataType (slot);
        // Get data and references
        EngineSlotData data = slot.Data as EngineSlotData;
        Rigidbody rb = slot.Equipper.GetComponent<Rigidbody> ();
        ConstantForce cf = slot.Equipper.GetComponent<ConstantForce> ();
        // Set drags to 0
        rb.drag = 0;
        rb.angularDrag = 0;
        // Clamp velocities
        if (data.EnergySatisfaction > 0) {
            if (rb.velocity.sqrMagnitude > MaxLinearSpeed * MaxLinearSpeed) rb.velocity = rb.velocity.normalized * MaxLinearSpeed;
            rb.maxAngularVelocity = MaxAngularSpeed * Mathf.Deg2Rad;
        } else rb.maxAngularVelocity = 0;
        // Apply force
        Vector3[] accels = GetAccelerations (slot);
        cf.relativeForce = accels[0] * data.EnergySatisfaction;
        cf.relativeTorque = accels[1] * data.EnergySatisfaction;
        // Bank
        foreach (Transform t in slot.Equipper.transform) t.localEulerAngles = new Vector3 (0, 0, -BankAmount * rb.angularVelocity.y);
    }

    public override bool CanClick (EquipmentSlot slot) { return false; }

    public override void OnClicked (EquipmentSlot slot) { }

    public override void EnsureDataType (EquipmentSlot slot) {
        if (!(slot.Data is EngineSlotData)) slot.Data = new EngineSlotData {
            Slot = slot,
            Equipment = this,
            Durability = Durability,
            LinearSetting = Vector3.zero,
            AngularSetting = Vector3.zero,
            EnergySatisfaction = 0,
        };
    }

    private Vector3[] GetAccelerations (EquipmentSlot slot) {
        Rigidbody rb = slot.Equipper.GetComponent<Rigidbody> ();
        EngineSlotData data = slot.Data as EngineSlotData;

        Vector3[] res = new Vector3[2];
        // Linear
        for (int d = 0; d < 3; d++) {
            if (data.ManagedPropulsion) {
                float cur = slot.Equipper.transform.InverseTransformDirection (rb.velocity)[d] / MaxLinearSpeed;
                float dif = data.LinearSetting[d] - cur;
                if (Mathf.Abs (dif) > LinearSleepThreshold) {
                    float mul = Mathf.Clamp01 (Mathf.Pow (Mathf.Abs (dif), CorrectionSlack)) * Mathf.Sign (dif);
                    res[0][d] = Lerp (mul, LinearForcePos[d], LinearForceNeg[d]);
                    res[0][d] *= slot.Equipper.GetStatAppliedValue (StatType.LinearSpeedMultiplier, 1);
                }
            } else {
                res[0][d] = Lerp (data.LinearSetting[d], LinearForcePos[d], LinearForceNeg[d]);
                res[0][d] *= slot.Equipper.GetStatAppliedValue (StatType.LinearSpeedMultiplier, 1);
            }
        }
        // Angular
        for (int d = 0; d < 3; d++) {
            if (data.ManagedPropulsion) {
                float cur = slot.Equipper.transform.InverseTransformDirection (rb.angularVelocity * Mathf.Rad2Deg)[d] / MaxAngularSpeed;
                float dif = data.AngularSetting[d] - cur;
                if (Mathf.Abs (dif) > AngularSleepThreshold) {
                    float mul = Mathf.Clamp01 (Mathf.Pow (Mathf.Abs (dif), CorrectionSlack)) * Mathf.Sign (dif);
                    res[1][d] = Lerp (mul, AngularForcePos[d], AngularForceNeg[d]);
                    res[1][d] *= slot.Equipper.GetStatAppliedValue (StatType.AngularSpeedMultiplier, 1);
                }
            } else {
                res[1][d] = Lerp (data.AngularSetting[d], AngularForcePos[d], AngularForceNeg[d]);
                res[1][d] *= slot.Equipper.GetStatAppliedValue (StatType.AngularSpeedMultiplier, 1);
            }
        }
        return res;
    }

    private float GetConsumption (EquipmentSlot slot) {
        Rigidbody rb = slot.Equipper.GetComponent<Rigidbody> ();
        EngineSlotData data = slot.Data as EngineSlotData;

        float res = 0;
        // Linear
        for (int d = 0; d < 3; d++) {
            float cur = slot.Equipper.transform.InverseTransformDirection (rb.velocity)[d] / MaxLinearSpeed;
            float dif = data.LinearSetting[d] - cur;
            float mul = Mathf.Clamp01 (Mathf.Pow (Mathf.Abs (dif), CorrectionSlack)) * Mathf.Sign (dif);
            res += Lerp (mul, LinearConsumptionPos[d], LinearConsumptionNeg[d]);
        }
        // Angular
        for (int d = 0; d < 3; d++) {
            float cur = slot.Equipper.transform.InverseTransformDirection (rb.angularVelocity * Mathf.Rad2Deg)[d] / MaxAngularSpeed;
            float dif = data.AngularSetting[d] - cur;
            float mul = Mathf.Clamp01 (Mathf.Pow (Mathf.Abs (dif), CorrectionSlack)) * Mathf.Sign (dif);
            res += Lerp (mul, AngularConsumptionPos[d], AngularConsumptionNeg[d]);
        }
        return res;
    }

    private float Lerp (float p, float pos, float neg) {
        if (p >= 0) return p * pos;
        return -p * neg;
    }
}

[Serializable]
public class EngineSlotData : EquipmentSlotData {
    public Vector3 LinearSetting;
    public Vector3 AngularSetting;
    public float EnergySatisfaction;
    public bool ManagedPropulsion;

    public override EquipmentSlotSaveData Save () {
        return new EngineSlotSaveData {
            EquipmentId = Equipment == null ? "" : Equipment.Id,
            Durability = Durability,
            LinearSetting = new float[] { LinearSetting.x, LinearSetting.y, LinearSetting.z },
            AngularSetting = new float[] { AngularSetting.x, AngularSetting.y, AngularSetting.z },
            EnergySatisfaction = EnergySatisfaction,
            ManagedPropulsion = ManagedPropulsion,
        };
    }
}

[Serializable]
public class EngineSlotSaveData : EquipmentSlotSaveData {
    public float[] LinearSetting;
    public float[] AngularSetting;
    public float EnergySatisfaction;
    public bool ManagedPropulsion;

    public override EquipmentSlotData Load () {
        return new EngineSlotData {
            Equipment = ItemManager.Instance.GetItem (EquipmentId) as EquipmentSO,
            Durability = Durability,
            LinearSetting = new Vector3 (LinearSetting[0], LinearSetting[1], LinearSetting[2]),
            AngularSetting = new Vector3 (AngularSetting[0], AngularSetting[1], AngularSetting[2]),
            EnergySatisfaction = EnergySatisfaction,
            ManagedPropulsion = ManagedPropulsion,
        };
    }
}
