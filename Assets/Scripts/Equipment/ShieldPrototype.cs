using System;
using UnityEngine;

namespace DarkFrontier.Equipment {

#nullable enable
    [CreateAssetMenu (menuName = "Items/Equipment/Shield")]
    public class ShieldPrototype : EquipmentPrototype {
        public float MaxStrength;
        public float RechargeConsumption;
        public float RechargeEfficiency;

        public override void Tick (EquipmentSlot aSlot, float aDt) {
            if (aSlot.Equipper == null) return;

            State lState = (aSlot.UState as State)!;

            var lConsumption = RechargeConsumption * aDt;
            var lLack = (MaxStrength - lState.Strength) / RechargeEfficiency;
            var lRequest = Mathf.Min (lConsumption, lLack);
            float lGiven = 0;
            var lCapacitors = aSlot.Equipper.UEquipment.States<CapacitorPrototype.State>();
            var lCount = lCapacitors.Count;
            for (var lIndex = 0; lIndex < lCount; lIndex++) {
                var lCapacitor = lCapacitors[lIndex];
                var lChargeLeft = lCapacitor.Charge;
                var lDischargeLeft = lCapacitor.DischargeLeft;
                var lAllocated = Mathf.Min (lChargeLeft, lDischargeLeft, lRequest - lGiven);
                lGiven += lAllocated;
                lCapacitor.Charge -= lAllocated;
                lCapacitor.DischargeLeft -= lAllocated;
            }
            lState.Strength += lGiven * RechargeEfficiency;
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
