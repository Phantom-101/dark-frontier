using System;
using UnityEngine;

namespace DarkFrontier.Equipment {

#nullable enable
    [CreateAssetMenu (menuName = "Items/Equipment/Engine")]
    public class EnginePrototype : EquipmentPrototype {
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

        public override void Tick (EquipmentSlot slot, float dt) {
            if (slot.Equipper == null) return;

            State state = (slot.State as State)!;

            float consumption = GetConsumption (slot) * dt;
            float given = 0;
            slot.Equipper.GetEquipmentStates<CapacitorPrototype.State> ().ForEach (capacitor => {
                float chargeLeft = capacitor.Charge;
                float dischargeLeft = capacitor.DischargeLeft;
                float allocated = Mathf.Min (chargeLeft, dischargeLeft, consumption - given);
                given += allocated;
                capacitor.Charge -= allocated;
                capacitor.DischargeLeft -= allocated;
            });

            state.EnergySatisfaction = Mathf.Clamp01 (given / (consumption == 0 ? 1 : consumption));
        }

        public override void FixedTick (EquipmentSlot slot, float dt) {
            if (slot.Equipper == null) return;

            State state = (slot.State as State)!;

            Rigidbody rb = slot.Equipper.GetComponent<Rigidbody> ();
            ConstantForce cf = slot.Equipper.GetComponent<ConstantForce> ();

            rb.drag = 0;
            rb.angularDrag = 0;

            if (state.EnergySatisfaction > 0) {
                if (rb.velocity.sqrMagnitude > Sqr (GetLinearMaxSpeed (slot))) rb.velocity = rb.velocity.normalized * GetLinearMaxSpeed (slot);
                rb.maxAngularVelocity = GetAngularMaxSpeed (slot) * Mathf.Deg2Rad;
            } else rb.maxAngularVelocity = 0;

            Vector3[] accels = GetAccelerations (slot);
            cf.relativeForce = accels[0] * state.EnergySatisfaction;
            cf.relativeTorque = accels[1] * state.EnergySatisfaction;

            foreach (Transform t in slot.Equipper.transform) t.localEulerAngles = new Vector3 (0, 0, -BankAmount * rb.angularVelocity.y);
        }

        public override void EnsureStateType (EquipmentSlot slot) {
            if (!(slot.UnsafeState is State)) slot.UnsafeState = GetNewState (slot);
        }

        public override EquipmentPrototype.State GetNewState (EquipmentSlot slot) => new State (slot, this);

        private Vector3[] GetAccelerations (EquipmentSlot slot) {
            if (slot.Equipper == null) return new Vector3[2];

            State state = (slot.State as State)!;

            Rigidbody rb = slot.Equipper.GetComponent<Rigidbody> ();

            Vector3[] res = new Vector3[2];
            // Linear
            for (int d = 0; d < 3; d++) {
                if (state.ManagedPropulsion) {
                    float cur = slot.Equipper.transform.InverseTransformDirection (rb.velocity)[d] / GetLinearMaxSpeed (slot);
                    float dif = state.LinearSetting[d] - cur;
                    if (Mathf.Abs (dif) > LinearSleepThreshold) {
                        float mul = Mathf.Clamp (dif * CorrectionStrictness, -1, 1);
                        res[0][d] = GetLinearAcceleration (slot, d, mul);
                    }
                } else res[0][d] = GetLinearAcceleration (slot, d, state.LinearSetting[d]);
            }
            // Angular
            for (int d = 0; d < 3; d++) {
                if (state.ManagedPropulsion) {
                    float cur = slot.Equipper.transform.InverseTransformDirection (rb.angularVelocity * Mathf.Rad2Deg)[d] / GetAngularMaxSpeed (slot);
                    float dif = state.AngularSetting[d] - cur;
                    if (Mathf.Abs (dif) > AngularSleepThreshold) {
                        float mul = Mathf.Clamp (dif * CorrectionStrictness, -1, 1);
                        res[1][d] = GetAngularAcceleration (slot, d, mul);
                    }
                } else res[1][d] = GetAngularAcceleration (slot, d, state.AngularSetting[d]);
            }
            return res;
        }

        private float GetConsumption (EquipmentSlot slot) {
            if (slot.Equipper == null) return 0;

            State state = (slot.State as State)!;

            Rigidbody rb = slot.Equipper.GetComponent<Rigidbody> ();

            float res = 0;
            // Linear
            for (int d = 0; d < 3; d++) {
                float cur = slot.Equipper.transform.InverseTransformDirection (rb.velocity)[d] / MaxLinearSpeed;
                float dif = state.LinearSetting[d] - cur;
                float mul = Mathf.Clamp (dif * CorrectionStrictness, -1, 1);
                res += Lerp (mul, LinearConsumptionPos[d], LinearConsumptionNeg[d]);
            }
            // Angular
            for (int d = 0; d < 3; d++) {
                float cur = slot.Equipper.transform.InverseTransformDirection (rb.angularVelocity * Mathf.Rad2Deg)[d] / MaxAngularSpeed;
                float dif = state.AngularSetting[d] - cur;
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

        private Stat GetLinearMaxSpeedMultiplierStat (EquipmentSlot slot) => (slot.State as State)!.LinearMaxSpeedMultiplierStat ?? ((slot.State as State)!.LinearMaxSpeedMultiplierStat = slot.Equipper!.Stats.GetStat (StatNames.LinearMaxSpeedMultiplier, 1));
        private Stat GetAngularMaxSpeedMultiplierStat (EquipmentSlot slot) => (slot.State as State)!.AngularMaxSpeedMultiplierStat ?? ((slot.State as State)!.AngularMaxSpeedMultiplierStat = slot.Equipper!.Stats.GetStat (StatNames.AngularMaxSpeedMultiplier, 1));
        private Stat GetLinearAccelerationMultiplierStat (EquipmentSlot slot) => (slot.State as State)!.LinearAccelerationMultiplierStat ?? ((slot.State as State)!.LinearAccelerationMultiplierStat = slot.Equipper!.Stats.GetStat (StatNames.LinearAccelerationMultiplier, 1));
        private Stat GetAngularAccelerationMultiplierStat (EquipmentSlot slot) => (slot.State as State)!.AngularAccelerationMultiplierStat ?? ((slot.State as State)!.AngularAccelerationMultiplierStat = slot.Equipper!.Stats.GetStat (StatNames.AngularAccelerationMultiplier, 1));

        private float Lerp (float p, float pos, float neg) {
            if (p >= 0) return p * pos;
            return -p * neg;
        }

        [Serializable]
        public new class State : EquipmentPrototype.State {
            public Vector3 LinearSetting;
            public Vector3 AngularSetting;
            public float EnergySatisfaction;
            public bool ManagedPropulsion;
            public Stat? LinearMaxSpeedMultiplierStat;
            public Stat? AngularMaxSpeedMultiplierStat;
            public Stat? LinearAccelerationMultiplierStat;
            public Stat? AngularAccelerationMultiplierStat;

            public State (EquipmentSlot slot, EnginePrototype equipment) : base (slot, equipment) { }

            public override EquipmentPrototype.State.Serializable ToSerializable () {
                return new Serializable {
                    Durability = Durability,
                    LinearSetting = new float[] { LinearSetting.x, LinearSetting.y, LinearSetting.z },
                    AngularSetting = new float[] { AngularSetting.x, AngularSetting.y, AngularSetting.z },
                    EnergySatisfaction = EnergySatisfaction,
                    ManagedPropulsion = ManagedPropulsion,
                };
            }

            public override void FromSerializable (EquipmentPrototype.State.Serializable serializable) {
                Serializable converted = (serializable as Serializable)!;
                Durability = converted.Durability;
                LinearSetting = new Vector3 (converted.LinearSetting[0], converted.LinearSetting[1], converted.LinearSetting[2]);
                AngularSetting = new Vector3 (converted.AngularSetting[0], converted.AngularSetting[1], converted.AngularSetting[2]);
                EnergySatisfaction = converted.EnergySatisfaction;
                ManagedPropulsion = converted.ManagedPropulsion;
            }

            [Serializable]
            public new class Serializable : EquipmentPrototype.State.Serializable {
                public float[] LinearSetting = new float[0];
                public float[] AngularSetting = new float[0];
                public float EnergySatisfaction;
                public bool ManagedPropulsion;
            }
        }
    }
}
#nullable restore
