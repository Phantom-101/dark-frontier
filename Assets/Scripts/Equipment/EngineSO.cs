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
    public float CorrectionStrictness;
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

    public override void Tick (EquipmentSlot slot, float dt) {
        EnsureDataType (slot);

        float consumption = GetConsumption (slot) * dt;
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

    public override void FixedTick (EquipmentSlot slot, float dt) {
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
            if (rb.velocity.sqrMagnitude > Sqr (GetLinearMaxSpeed (slot))) rb.velocity = rb.velocity.normalized * GetLinearMaxSpeed (slot);
            rb.maxAngularVelocity = GetAngularMaxSpeed (slot) * Mathf.Deg2Rad;
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
                float cur = slot.Equipper.transform.InverseTransformDirection (rb.velocity)[d] / GetLinearMaxSpeed (slot);
                float dif = data.LinearSetting[d] - cur;
                if (Mathf.Abs (dif) > LinearSleepThreshold) {
                    float mul = Mathf.Clamp (dif * CorrectionStrictness, -1, 1);
                    res[0][d] = GetLinearAcceleration (slot, d, mul);
                }
            } else res[0][d] = GetLinearAcceleration (slot, d, data.LinearSetting[d]);
        }
        // Angular
        for (int d = 0; d < 3; d++) {
            if (data.ManagedPropulsion) {
                float cur = slot.Equipper.transform.InverseTransformDirection (rb.angularVelocity * Mathf.Rad2Deg)[d] / GetAngularMaxSpeed (slot);
                float dif = data.AngularSetting[d] - cur;
                if (Mathf.Abs (dif) > AngularSleepThreshold) {
                    float mul = Mathf.Clamp (dif * CorrectionStrictness, -1, 1);
                    res[1][d] = GetAngularAcceleration (slot, d, mul);
                }
            } else res[1][d] = GetAngularAcceleration (slot, d, data.AngularSetting[d]);
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
            float mul = Mathf.Clamp (dif * CorrectionStrictness, -1, 1);
            res += Lerp (mul, LinearConsumptionPos[d], LinearConsumptionNeg[d]);
        }
        // Angular
        for (int d = 0; d < 3; d++) {
            float cur = slot.Equipper.transform.InverseTransformDirection (rb.angularVelocity * Mathf.Rad2Deg)[d] / MaxAngularSpeed;
            float dif = data.AngularSetting[d] - cur;
            float mul = Mathf.Clamp (dif * CorrectionStrictness, -1, 1);
            res += Lerp (mul, AngularConsumptionPos[d], AngularConsumptionNeg[d]);
        }
        return res;
    }

    private float GetLinearMaxSpeed (EquipmentSlot slot) => MaxLinearSpeed * GetLinearMaxSpeedMultiplierStat (slot).AppliedValue;
    private float GetAngularMaxSpeed (EquipmentSlot slot) => MaxAngularSpeed * GetAngularMaxSpeedMultiplierStat (slot).AppliedValue;
    private float GetLinearAcceleration (EquipmentSlot slot, int d, float s) => Lerp (s, LinearForcePos[d], LinearForceNeg[d]) * GetLinearAccelerationMultiplierStat (slot).AppliedValue;
    private float GetAngularAcceleration (EquipmentSlot slot, int d, float s) => Lerp (s, AngularForcePos[d], AngularForceNeg[d]) * GetAngularAccelerationMultiplierStat (slot).AppliedValue;
    private float Sqr (float n) => n * n;

    private Stat GetLinearMaxSpeedMultiplierStat (EquipmentSlot slot) => (slot.Data as EngineSlotData).LinearMaxSpeedMultiplierStat ?? ((slot.Data as EngineSlotData).LinearMaxSpeedMultiplierStat = slot.Equipper.Stats.GetStat (StatNames.LinearMaxSpeedMultiplier, 1));
    private Stat GetAngularMaxSpeedMultiplierStat (EquipmentSlot slot) => (slot.Data as EngineSlotData).AngularMaxSpeedMultiplierStat ?? ((slot.Data as EngineSlotData).AngularMaxSpeedMultiplierStat = slot.Equipper.Stats.GetStat (StatNames.AngularMaxSpeedMultiplier, 1));
    private Stat GetLinearAccelerationMultiplierStat (EquipmentSlot slot) => (slot.Data as EngineSlotData).LinearAccelerationMultiplierStat ?? ((slot.Data as EngineSlotData).LinearAccelerationMultiplierStat = slot.Equipper.Stats.GetStat (StatNames.LinearAccelerationMultiplier, 1));
    private Stat GetAngularAccelerationMultiplierStat (EquipmentSlot slot) => (slot.Data as EngineSlotData).AngularAccelerationMultiplierStat ?? ((slot.Data as EngineSlotData).AngularAccelerationMultiplierStat = slot.Equipper.Stats.GetStat (StatNames.AngularAccelerationMultiplier, 1));

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
    public Stat LinearMaxSpeedMultiplierStat;
    public Stat AngularMaxSpeedMultiplierStat;
    public Stat LinearAccelerationMultiplierStat;
    public Stat AngularAccelerationMultiplierStat;

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
