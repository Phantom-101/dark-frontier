using DarkFrontier.Foundation.Services;
using DarkFrontier.Structures;
using System;
using UnityEngine;

namespace DarkFrontier.Equipment {

#nullable enable
    [CreateAssetMenu (menuName = "Items/Equipment/Weapons/Pulse Laser")]
    public class PulseLaserPrototype : EquipmentPrototype {
        public float Range;
        public Damage Damage;
        public float EnergyRequired;
        public float RechargeRate;
        public AnimationCurve PreemptiveDamageMultiplier = new AnimationCurve ();
        public AnimationCurve PreemptiveRangeMultiplier = new AnimationCurve ();
        public GameObject? BeamPrefab;
        public float BeamWidth;

        public override void Tick (EquipmentSlot slot, float dt) {
            if (slot.Equipper == null) return;

            State state = (slot.State as State)!;

            float consumption = RechargeRate * dt;
            float lack = EnergyRequired - state.Charge;
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
            state.Charge += given;

            if (state.Activated && (state.Target == null || !slot.Equipper.Locks.ContainsKey (state.Target) || (state.Target.transform.position - slot.Equipper.transform.position).sqrMagnitude > GetRange (slot) * GetRange (slot))) state.Activated = false;

            if (state.Activated) {
                if (BeamPrefab != null) {
                    GameObject beam = Instantiate (BeamPrefab, slot.transform);
                    beam.transform.LookAt (state.Target!.transform);
                    beam.transform.localScale = Vector3.one;
                    beam.transform.localScale = new Vector3 (
                        BeamWidth / beam.transform.lossyScale.x,
                        BeamWidth / beam.transform.lossyScale.y,
                        Vector3.Distance (slot.transform.position, state.Target.transform.position) / beam.transform.lossyScale.z
                    );
                    Destroy (beam, 0.2f);
                }
                /* Use raycast?
                if (Physics.Raycast (slot.Equipper.transform.position, data.Target.transform.position - slot.Equipper.transform.position, out RaycastHit hit, Range)) {
                    Structure structure = hit.transform.gameObject.GetComponentInParent<Structure> ();
                    if (structure != null) structure.TakeDamage (Damage * HeatDamageMultiplier.Evaluate (data.Heat / MaxHeat), hit.point);
                }
                */
                state.Target!.TakeDamage (Damage * PreemptiveDamageMultiplier.Evaluate (state.Charge / EnergyRequired), slot.Equipper.transform.position);
                state.Charge = 0;
                state.Activated = false;
            }
        }

        public override bool CanClick (EquipmentSlot slot) {
            if (slot.Equipper == null) return false;

            State state = (slot.State as State)!;

            if (state.Activated) {
                // If equipment is activated and selected is null or target
                // Assume user wants to deactivate equipment
                if (slot.Equipper.Selected == null || slot.Equipper.Selected == state.Target) return true;
                // If equipment is activated and selected is not null
                // Assume user wants to change target
                else {
                    if (!slot.Equipper.Locks.ContainsKey (slot.Equipper.Selected)) return false;
                    if ((slot.Equipper.Selected.transform.position - slot.Equipper.transform.position).sqrMagnitude > GetRange (slot) * GetRange (slot)) return false;
                    return true;
                }
            } else {
                // If equipment is not activated
                // Assume user wants to activate equipment
                if (slot.Equipper.Selected == null) return false;
                if (!slot.Equipper.Locks.ContainsKey (slot.Equipper.Selected)) return false;
                if ((slot.Equipper.Selected.transform.position - slot.Equipper.transform.position).sqrMagnitude > GetRange (slot) * GetRange (slot)) return false;
                return true;
            }
        }

        public override void OnClicked (EquipmentSlot slot) {
            if (!CanClick (slot)) return;

            if (slot.Equipper == null) return;

            State state = (slot.State as State)!;

            if (state.Activated) {
                // If equipment is activated and selected is null or target
                // Assume user wants to deactivate equipment
                if (slot.Equipper.Selected == null || slot.Equipper.Selected == state.Target) state.Activated = false;
                // If equipment is activated and selected is not null
                // Assume user wants to change target
                else state.Target = slot.Equipper.Selected;
            } else {
                // If equipment is not activated
                // Assume user wants to activate equipment
                state.Activated = true;
                state.Target = slot.Equipper.Selected;
            }
        }

        public override void EnsureStateType (EquipmentSlot slot) {
            if (!(slot.UnsafeState is State)) slot.UnsafeState = GetNewState (slot);
        }

        public override EquipmentPrototype.State GetNewState (EquipmentSlot slot) => new State (slot, this);

        public float GetRange (EquipmentSlot slot) {
            return Range * GetRangeMultiplier (slot);
        }

        public float GetRangeMultiplier (EquipmentSlot slot) {
            return PreemptiveRangeMultiplier.Evaluate ((slot.State as State)!.Charge / EnergyRequired);
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
                    TargetId = Target == null ? "" : Target.Id,
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
#nullable restore
