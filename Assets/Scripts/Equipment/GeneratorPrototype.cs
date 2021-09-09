using System;
using UnityEngine;

namespace DarkFrontier.Equipment {

#nullable enable
    [CreateAssetMenu (menuName = "Items/Equipment/Generator")]
    public class GeneratorPrototype : EquipmentPrototype {
        public float Generation;

        public override void Tick (EquipmentSlot slot, float dt) {
            if (slot.Equipper == null) return;

            State state = (slot.State as State)!;

            float pool = Generation * dt;
            slot.Equipper.GetEquipmentStates<CapacitorPrototype.State> ().ForEach (capacitor => {
                float lack = (capacitor.Slot.Equipment as CapacitorPrototype)!.Capacitance - capacitor.Charge;
                float allocated = Mathf.Min (pool, lack, capacitor.ChargeLeft);
                capacitor.Charge += allocated;
                capacitor.ChargeLeft -= allocated;
                pool -= allocated;
            });
        }

        public override void EnsureStateType (EquipmentSlot slot) {
            if (!(slot.UnsafeState is State)) slot.UnsafeState = GetNewState (slot);
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
#nullable restore
