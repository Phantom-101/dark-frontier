using System;
using UnityEngine;

namespace DarkFrontier.Equipment {


    [CreateAssetMenu (menuName = "Items/Equipment/Generator")]
    public class GeneratorPrototype : EquipmentPrototype {
        public float Generation;

        public override void Tick (EquipmentSlot aSlot, float aDt) {
            if (aSlot.Equipper == null) return;

            var lPool = Generation * aDt;
            var lCapacitors = aSlot.Equipper.uEquipment.States<CapacitorPrototype.State>();
            var lCount = lCapacitors.Count;
            for (var lIndex = 0; lIndex < lCount; lIndex++) {
                var lCapacitor = lCapacitors[lIndex];
                var lLack = (lCapacitor.Slot.Equipment as CapacitorPrototype)!.Capacitance - lCapacitor.Charge;
                var lAllocated = Mathf.Min (lPool, lLack, lCapacitor.ChargeLeft);
                lCapacitor.Charge += lAllocated;
                lCapacitor.ChargeLeft -= lAllocated;
                lPool -= lAllocated;
            }
        }

        public override void EnsureStateType (EquipmentSlot slot) {
            if (!(slot.UState is State)) slot.UState = GetNewState (slot);
        }

        public override EquipmentPrototype.State GetNewState (EquipmentSlot slot) => new State (slot, this);

        [Serializable]
        public new class State : EquipmentPrototype.State {
            public State (EquipmentSlot slot, GeneratorPrototype equipment) : base (slot, equipment) { }

            public override EquipmentPrototype.State.Serializable ToSerializable () {
                return new Serializable {
                    Durability = Durability,
                };
            }

            public override void FromSerializable (EquipmentPrototype.State.Serializable serializable) {
                Serializable converted = (serializable as Serializable)!;
                Durability = converted.Durability;
            }

            [Serializable]
            public new class Serializable : EquipmentPrototype.State.Serializable { }
        }
    }
}

