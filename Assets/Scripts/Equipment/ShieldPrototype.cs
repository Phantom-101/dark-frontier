using System;
using UnityEngine;

namespace DarkFrontier.Equipment {

#nullable enable
    [CreateAssetMenu (menuName = "Items/Equipment/Shield")]
    public class ShieldPrototype : EquipmentPrototype {
        public float MaxStrength;
        public float RechargeConsumption;
        public float RechargeEfficiency;

        public override void Tick (EquipmentSlot slot, float dt) {
            if (slot.Equipper == null) return;

            State state = (slot.State as State)!;

            float consumption = RechargeConsumption * dt;
            float lack = (MaxStrength - state.Strength) / RechargeEfficiency;
            float request = Mathf.Min (consumption, lack);
            float given = 0;
            slot.Equipper.GetEquipmentStates<CapacitorPrototype.State> ().ForEach (capacitor => {
                float chargeLeft = capacitor.Charge;
                float dischargeLeft = capacitor.DischargeLeft;
                float allocated = Mathf.Min (chargeLeft, dischargeLeft, request - given);
                given += allocated;
                capacitor.Charge -= allocated;
                capacitor.DischargeLeft -= allocated;
            });

            state.Strength += given * RechargeEfficiency;
        }

        public override void EnsureStateType (EquipmentSlot slot) {
            if (!(slot.UnsafeState is State)) slot.UnsafeState = GetNewState (slot);
        }

        public override EquipmentPrototype.State GetNewState (EquipmentSlot slot) => new State (slot, this);

        [Serializable]
        public new class State : EquipmentPrototype.State {
            public float Strength;

            public State (EquipmentSlot slot, ShieldPrototype equipment) : base (slot, equipment) { }

            public override EquipmentPrototype.State.Serializable ToSerializable () {
                return new Serializable {
                    Durability = Durability,
                    Strength = Strength,
                };
            }

            public override void FromSerializable (EquipmentPrototype.State.Serializable serializable) {
                Serializable converted = (serializable as Serializable)!;
                Durability = converted.Durability;
                Strength = converted.Strength;
            }

            [Serializable]
            public new class Serializable : EquipmentPrototype.State.Serializable {
                public float Strength;
            }
        }
    }
}
#nullable restore
