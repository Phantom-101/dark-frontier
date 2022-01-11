using System;
using Newtonsoft.Json;
using Unity.Mathematics;
using UnityEngine;
using Math = DarkFrontier.Foundation.Mathematics.Math;

namespace DarkFrontier.Equipment {

#nullable enable
    [CreateAssetMenu (menuName = "Items/Equipment/Capacitor")]
    public class CapacitorPrototype : EquipmentPrototype {
        public float Capacitance;
        public float MaxChargeRate;
        public float MaxDischargeRate;

        public override void OnAwake (EquipmentSlot slot) => EnsureStateType (slot);
        public override void OnEquip (EquipmentSlot slot) => slot.UState = GetNewState (slot);
        public override void OnUnequip (EquipmentSlot slot) { }

        public override void Tick (EquipmentSlot aSlot, float aDt) {
            State state = (aSlot.UState as State)!;

            state.ChargeLeft = MaxChargeRate * aDt;
            state.DischargeLeft = MaxDischargeRate * aDt;
        }

        public override void FixedTick (EquipmentSlot slot, float aDt) { }
        public override bool CanClick(EquipmentSlot slot) => false;
        public override void OnClicked (EquipmentSlot slot) { }

        public override void EnsureStateType (EquipmentSlot slot) {
            if (!(slot.UState is State)) slot.UState = GetNewState (slot);
        }

        public override EquipmentPrototype.State GetNewState (EquipmentSlot slot) => new State (slot, this);

        [Serializable]
        [JsonObject(MemberSerialization.OptIn)]
        public new class State : EquipmentPrototype.State {
            [JsonProperty]
            public float Charge;
            
            [JsonProperty]
            public float ChargeLeft;
            
            [JsonProperty]
            public float DischargeLeft;

            public State (EquipmentSlot slot, CapacitorPrototype equipment) : base (slot, equipment) { }

            public float Recharge(float v) {
                var ret = Math.Min(((CapacitorPrototype)Equipment).Capacitance - Charge, ChargeLeft, v);
                Charge += ret;
                ChargeLeft += ret;
                return ret;
            }
            
            public float Discharge(float v) {
                var ret = Math.Min(Charge, DischargeLeft, v);
                Charge -= ret;
                DischargeLeft -= ret;
                return ret;
            }
            
            public override EquipmentPrototype.State.Serializable ToSerializable () {
                return new Serializable {
                    Durability = Durability,
                    Charge = Charge,
                    ChargeLeft = ChargeLeft,
                    DischargeLeft = DischargeLeft,
                };
            }

            public override void FromSerializable (EquipmentPrototype.State.Serializable serializable) {
                Serializable converted = (serializable as Serializable)!;
                Durability = converted.Durability;
                Charge = converted.Charge;
                ChargeLeft = converted.ChargeLeft;
                DischargeLeft = converted.DischargeLeft;
            }

            [Serializable]
            public new class Serializable : EquipmentPrototype.State.Serializable {
                public float Charge;
                public float ChargeLeft;
                public float DischargeLeft;
            }
        }
    }
}
#nullable restore
