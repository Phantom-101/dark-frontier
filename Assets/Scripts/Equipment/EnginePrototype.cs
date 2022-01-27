using System;
using DarkFrontier.Positioning.Navigation;
using UnityEngine;


namespace DarkFrontier.Equipment
{
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
        public float BankAmount;

        public override void OnAwake (EquipmentSlot slot) => EnsureStateType (slot);
        public override void OnEquip (EquipmentSlot slot) => slot.UState = GetNewState (slot);
        
        public override void Tick (EquipmentSlot aSlot, float aDt) {
            if (aSlot.Equipper == null) return;
            
            Rigidbody lRigidbody = aSlot.Equipper.GetComponent<Rigidbody> ();

            var (lAccelerations, lConsumption) = Calculate(aSlot);
            lConsumption *= aDt;
            
            float lGiven = 0;
            var equipment = aSlot.Equipper.uEquipment.uAll;
            var count = equipment.Count;
            for (var i = 0; i < count; i++) {
                if (equipment[i].UState is CapacitorPrototype.State capacitor) {
                    lGiven += capacitor.Discharge(lConsumption - lGiven);
                }
            }

            var lEnergySatisfaction = lConsumption == 0 ? 1 : Mathf.Clamp01 (lGiven / lConsumption);

            if (lEnergySatisfaction > 0) {
                if (lRigidbody.velocity.sqrMagnitude > Sqr (GetLinearMaxSpeed (aSlot))) lRigidbody.velocity = lRigidbody.velocity.normalized * GetLinearMaxSpeed (aSlot);
                lRigidbody.maxAngularVelocity = GetAngularMaxSpeed (aSlot) * Mathf.Deg2Rad;
            }
            
            ConstantForce lConstantForce = aSlot.Equipper.GetComponent<ConstantForce> ();
            
            lConstantForce.relativeForce = lEnergySatisfaction * lAccelerations[0];
            lConstantForce.relativeTorque = lEnergySatisfaction * lAccelerations[1];
        }

        public override void FixedTick (EquipmentSlot aSlot, float aDt) {
            if (aSlot.Equipper == null) return;

            Rigidbody lRigidbody = aSlot.Equipper.GetComponent<Rigidbody> ();

            lRigidbody.drag = 0;
            lRigidbody.angularDrag = 0;

            foreach (Transform lChild in aSlot.Equipper.transform) lChild.localEulerAngles = new Vector3 (0, 0, -BankAmount * lRigidbody.angularVelocity.y);
        }

        public override void EnsureStateType (EquipmentSlot aSlot) {
            if (!(aSlot.UState is State)) aSlot.UState = GetNewState (aSlot);
        }

        public override EquipmentPrototype.State GetNewState (EquipmentSlot aSlot) => new State (aSlot, this);

        private (Vector3[], float) Calculate (EquipmentSlot aSlot) {
            if (aSlot.Equipper == null) return (new Vector3[2], 0);

            State lState = (aSlot.UState as State)!;
            Rigidbody lRigidbody = aSlot.Equipper.GetComponent<Rigidbody> ();

            Vector3[] lAccelerations = new Vector3[2];
            float lConsumption = 0;
            
            // Linear
            for (var lDimension = 0; lDimension < 3; lDimension++) {
                if (lState.ManagedPropulsion) {
                    var lCurrentSpeed = aSlot.Equipper.transform.InverseTransformDirection (lRigidbody.velocity)[lDimension] / GetLinearMaxSpeed (aSlot);
                    var lSpeedDif = lState.LinearSetting[lDimension] - lCurrentSpeed;
                    lAccelerations[0][lDimension] = GetLinearAcceleration (aSlot, lDimension, Navigation.Sign(lSpeedDif));
                    lConsumption += Lerp (1, LinearConsumptionPos[lDimension], LinearConsumptionNeg[lDimension]);
                } else {
                    lAccelerations[0][lDimension] = GetLinearAcceleration (aSlot, lDimension, lState.LinearSetting[lDimension]);
                    lConsumption += Lerp (lState.LinearSetting[lDimension], LinearConsumptionPos[lDimension], LinearConsumptionNeg[lDimension]);
                }
            }
            
            // Angular
            for (var lDimension = 0; lDimension < 3; lDimension++) {
                if (lState.ManagedPropulsion) {
                    var lCurrentSpeed = aSlot.Equipper.transform.InverseTransformDirection (lRigidbody.angularVelocity * Mathf.Rad2Deg)[lDimension] / GetAngularMaxSpeed (aSlot);
                    var lSpeedDif = lState.AngularSetting[lDimension] - lCurrentSpeed;
                    
                    lAccelerations[1][lDimension] = GetAngularAcceleration (aSlot, lDimension, Navigation.Sign(lSpeedDif));
                    lConsumption += Lerp (1, AngularConsumptionPos[lDimension], AngularConsumptionNeg[lDimension]);
                } else {
                    lAccelerations[1][lDimension] = GetAngularAcceleration (aSlot, lDimension, lState.AngularSetting[lDimension]);
                    lConsumption += Lerp (lState.AngularSetting[lDimension], AngularConsumptionPos[lDimension], AngularConsumptionNeg[lDimension]);
                }
            }
            
            return (lAccelerations, lConsumption);
        }

        private float GetLinearMaxSpeed (EquipmentSlot slot) => MaxLinearSpeed * slot.Equipper!.uStats.UValues.LinearMaxSpeedMultiplier;
        private float GetAngularMaxSpeed (EquipmentSlot slot) => MaxAngularSpeed * slot.Equipper!.uStats.UValues.AngularMaxSpeedMultiplier;
        private float GetLinearAcceleration (EquipmentSlot slot, int d, float s) => Lerp (s, LinearForcePos[d], LinearForceNeg[d]) * slot.Equipper!.uStats.UValues.LinearAccelerationMultiplier;
        private float GetAngularAcceleration (EquipmentSlot slot, int d, float s) => Lerp (s, AngularForcePos[d], AngularForceNeg[d]) * slot.Equipper!.uStats.UValues.AngularAccelerationMultiplier;
        private static float Sqr (float n) => n * n;

        private static float Lerp (float p, float pos, float neg) {
            if (p >= 0) return p * pos;
            return -p * neg;
        }

        [Serializable]
        public new class State : EquipmentPrototype.State {
            public Vector3 LinearSetting;
            public Vector3 AngularSetting;
            public bool ManagedPropulsion;

            public State (EquipmentSlot slot, EnginePrototype equipment) : base (slot, equipment) { }

            public override EquipmentPrototype.State.Serializable ToSerializable () {
                return new Serializable {
                    Durability = Durability,
                    LinearSetting = new float[] { LinearSetting.x, LinearSetting.y, LinearSetting.z },
                    AngularSetting = new float[] { AngularSetting.x, AngularSetting.y, AngularSetting.z },
                    ManagedPropulsion = ManagedPropulsion,
                };
            }

            public override void FromSerializable (EquipmentPrototype.State.Serializable serializable) {
                Serializable converted = (serializable as Serializable)!;
                Durability = converted.Durability;
                LinearSetting = new Vector3 (converted.LinearSetting[0], converted.LinearSetting[1], converted.LinearSetting[2]);
                AngularSetting = new Vector3 (converted.AngularSetting[0], converted.AngularSetting[1], converted.AngularSetting[2]);
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

