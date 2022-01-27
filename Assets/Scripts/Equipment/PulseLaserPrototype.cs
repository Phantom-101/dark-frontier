using DarkFrontier.Foundation.Services;
using DarkFrontier.Locations;
using DarkFrontier.Structures;
using System;
using System.Collections;
using System.Linq;
using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Items.Structures;
using DarkFrontier.Visuals;
using UnityEngine;

namespace DarkFrontier.Equipment {


    [CreateAssetMenu (menuName = "Items/Equipment/Weapons/Pulse Laser")]
    public class PulseLaserPrototype : EquipmentPrototype {
        public float Range;
        public Damage Damage;
        public float EnergyRequired;
        public float RechargeRate;
        public AnimationCurve PreemptiveDamageMultiplier = new AnimationCurve ();
        public AnimationCurve PreemptiveRangeMultiplier = new AnimationCurve ();
        public GameObject? BeamPrefab;
        public Material? BeamMaterial;
        public AnimationCurve BeamAlpha = new AnimationCurve();
        public float BeamDuration;

        public override void Tick (EquipmentSlot aSlot, float aDt) {
            if (aSlot.Equipper == null) return;

            State lState = (aSlot.UState as State)!;

            var lConsumption = RechargeRate * aDt;
            var lLack = EnergyRequired - lState.Charge;
            var lRequest = Mathf.Min (lConsumption, lLack);
            float lGiven = 0;
            var lCapacitors = aSlot.Equipper.uEquipment.States<CapacitorPrototype.State>();
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
            lState.Charge += lGiven;

            if (lState.Activated && (lState.Target == null || !aSlot.Equipper.uLocks.Keys.Any (lGetter => lGetter.UId.Value == lState.Target.uId) || (lState.Target.transform.position - aSlot.Equipper.transform.position).sqrMagnitude > GetRange (aSlot) * GetRange (aSlot))) lState.Activated = false;

            if (lState.Activated) {
                if (BeamPrefab != null) {
                    var lVisuals = (LaserVisuals) Singletons.Get<BehaviorPooler>().Take("laser-visuals", SpawnVisuals);
                    lVisuals.uFrom = new Location(aSlot.transform);
                    lVisuals.uTo = new Location(lState.Target!.transform);
                    lVisuals.uMaterial = BeamMaterial;
                    lVisuals.uAlpha = BeamAlpha;
                    lVisuals.uPeriod = BeamDuration;
                    lVisuals.uRepeat = false;
                    lVisuals.Enable();
                    Singletons.Get<BehaviorPooler>().ReclaimAfter("laser-visuals", lVisuals, BeamDuration);
                }
                /* Use raycast?
                if (Physics.Raycast (slot.Equipper.transform.position, data.Target.transform.position - slot.Equipper.transform.position, out RaycastHit hit, Range)) {
                    Structure structure = hit.transform.gameObject.GetComponentInParent<Structure> ();
                    if (structure != null) structure.TakeDamage (Damage * HeatDamageMultiplier.Evaluate (data.Heat / MaxHeat), hit.point);
                }
                */
                lState.Target!.TakeDamage (Damage * PreemptiveDamageMultiplier.Evaluate (lState.Charge / EnergyRequired), new Location (aSlot.Equipper.transform));
                lState.Charge = 0;
                lState.Activated = false;
            }
        }

        public override bool CanClick (EquipmentSlot slot) {
            if (slot.Equipper == null) return false;

            State state = (slot.UState as State)!;

            if (state.Activated) {
                // If equipment is activated and selected is null or target
                // Assume user wants to deactivate equipment
                if (slot.Equipper.uSelected.UValue == null || slot.Equipper.uSelected.UValue == state.Target) return true;
                // If equipment is activated and selected is not null
                // Assume user wants to change target
                else {
                    if (!slot.Equipper.uLocks.Keys.Any (lGetter => lGetter.UValue == slot.Equipper.uSelected.UValue)) return false;
                    if ((slot.Equipper.uSelected.UValue.transform.position - slot.Equipper.transform.position).sqrMagnitude > GetRange (slot) * GetRange (slot)) return false;
                    return true;
                }
            } else {
                // If equipment is not activated
                // Assume user wants to activate equipment
                if (slot.Equipper.uSelected.UValue == null) return false;
                if (!slot.Equipper.uLocks.Keys.Any (lGetter => lGetter.UValue == slot.Equipper.uSelected.UValue)) return false;
                if ((slot.Equipper.uSelected.UValue.transform.position - slot.Equipper.transform.position).sqrMagnitude > GetRange (slot) * GetRange (slot)) return false;
                return true;
            }
        }

        public override void OnClicked (EquipmentSlot slot) {
            if (!CanClick (slot)) return;

            if (slot.Equipper == null) return;

            State state = (slot.UState as State)!;

            if (state.Activated) {
                // If equipment is activated and selected is null or target
                // Assume user wants to deactivate equipment
                if (slot.Equipper.uSelected.UValue == null || slot.Equipper.uSelected.UValue == state.Target) state.Activated = false;
                // If equipment is activated and selected is not null
                // Assume user wants to change target
                else state.Target = slot.Equipper.uSelected.UValue;
            } else {
                // If equipment is not activated
                // Assume user wants to activate equipment
                state.Activated = true;
                state.Target = slot.Equipper.uSelected.UValue;
            }
        }

        public override void EnsureStateType (EquipmentSlot slot) {
            if (!(slot.UState is State)) slot.UState = GetNewState (slot);
        }

        public override EquipmentPrototype.State GetNewState (EquipmentSlot slot) => new State (slot, this);

        public float GetRange (EquipmentSlot slot) {
            return Range * GetRangeMultiplier (slot);
        }

        public float GetRangeMultiplier (EquipmentSlot slot) {
            return PreemptiveRangeMultiplier.Evaluate ((slot.UState as State)!.Charge / EnergyRequired);
        }
        
        private IBehavior SpawnVisuals() {
            var lVisuals = Instantiate(BeamPrefab)!.GetComponent<LaserVisuals>();
            lVisuals.Disable();
            return lVisuals;
        }

        [Serializable]
        public new class State : EquipmentPrototype.State {
            public float Charge;
            public bool Activated;
            public Structure? Target;

            public State (EquipmentSlot slot, PulseLaserPrototype equipment) : base (slot, equipment) { }

            public override EquipmentPrototype.State.Serializable ToSerializable () {
                return new Serializable {
                    Durability = Durability,
                    Charge = Charge,
                    Activated = Activated,
                    TargetId = Target == null ? "" : Target.uId,
                };
            }

            public override void FromSerializable (EquipmentPrototype.State.Serializable serializable) {
                Serializable converted = (serializable as Serializable)!;
                Durability = converted.Durability;
                Charge = converted.Charge;
                Activated = converted.Activated;
                Target = Singletons.Get<StructureManager> ().GetStructure (converted.TargetId);
            }

            [Serializable]
            public new class Serializable : EquipmentPrototype.State.Serializable {
                public float Charge;
                public bool Activated;
                public string TargetId = "";
            }
        }
    }
}

