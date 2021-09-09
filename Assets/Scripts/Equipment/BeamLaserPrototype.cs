using DarkFrontier.Foundation.Services;
using DarkFrontier.Structures;
using System;
using UnityEngine;

namespace DarkFrontier.Equipment {

#nullable enable
    [CreateAssetMenu (menuName = "Items/Equipment/Weapons/Beam Laser")]
    public class BeamLaserPrototype : EquipmentPrototype {
        public float Range;
        public Damage DamagePerSecond;
        public float DamageInterval;
        public float EnergyConsumption;
        public float MaxHeat;
        public float HeatGeneration;
        public float CoolingRate;
        public float OverheatDamage;
        public AnimationCurve HeatDamageMultiplier = new AnimationCurve ();
        public GameObject? BeamPrefab;
        public float BeamWidth;

        public override void OnAwake (EquipmentSlot slot) => EnsureStateType (slot);
        public override void OnEquip (EquipmentSlot slot) => slot.UnsafeState = GetNewState (slot);

        public override void OnUnequip (EquipmentSlot slot) {
            State state = (slot.State as State)!;
            if (state.Beam != null) Destroy (state.Beam);
        }

        public override void Tick (EquipmentSlot slot, float dt) {
            if (slot.Equipper == null) return;

            State state = (slot.State as State)!;

            if (state.Activated && (state.Target == null || !slot.Equipper.Locks.ContainsKey (state.Target) || (state.Target.transform.position - slot.Equipper.transform.position).sqrMagnitude > Range * Range)) state.Activated = false;

            state.Heat = Mathf.Clamp (state.Heat - CoolingRate * dt + (state.Activated ? HeatGeneration * dt : 0), 0, MaxHeat);

            if (state.Activated) {
                if (state.Beam == null) state.Beam = Instantiate (BeamPrefab, slot.transform);
                state.Beam!.transform.LookAt (state.Target!.transform);
                state.Beam.transform.localScale = Vector3.one;
                state.Beam.transform.localScale = new Vector3 (
                    BeamWidth / state.Beam.transform.lossyScale.x,
                    BeamWidth / state.Beam.transform.lossyScale.y,
                    Vector3.Distance (slot.transform.position, state.Target.transform.position) / state.Beam.transform.lossyScale.z
                );
                float consumption = EnergyConsumption * dt;
                float given = 0;
                slot.Equipper.GetEquipmentStates<CapacitorPrototype.State> ().ForEach (capacitor => {
                    float chargeLeft = capacitor.Charge;
                    float dischargeLeft = capacitor.DischargeLeft;
                    float allocated = Mathf.Min (chargeLeft, dischargeLeft, consumption - given);
                    given += allocated;
                    capacitor.Charge -= allocated;
                    capacitor.DischargeLeft -= allocated;
                });
                state.AccumulatedDamageMultiplier += given / consumption * dt;
                /* Use raycast?
                if (Physics.Raycast (slot.Equipper.transform.position, data.Target.transform.position - slot.Equipper.transform.position, out RaycastHit hit, Range)) {
                    Structure structure = hit.transform.gameObject.GetComponentInParent<Structure> ();
                    if (structure != null) structure.TakeDamage (Damage * HeatDamageMultiplier.Evaluate (data.Heat / MaxHeat), hit.point);
                }
                */
                if (state.AccumulatedDamageMultiplier >= DamageInterval) {
                    state.Target.TakeDamage (DamagePerSecond * state.AccumulatedDamageMultiplier * HeatDamageMultiplier.Evaluate (state.Heat / MaxHeat), slot.Equipper.transform.position);
                    state.AccumulatedDamageMultiplier = 0;
                }
            } else {
                state.AccumulatedDamageMultiplier = 0;
                if (state.Beam != null) Destroy (state.Beam);
            }

            if (state.Heat == MaxHeat) {
                state.Durability = Mathf.Max (state.Durability - OverheatDamage * dt, 0);
                if (state.Durability == 0) OnUnequip (slot);
            }
        }

        public override void FixedTick (EquipmentSlot slot, float dt) { }

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
                    if ((slot.Equipper.Selected.transform.position - slot.Equipper.transform.position).sqrMagnitude > Range * Range) return false;
                    return true;
                }
            } else {
                // If equipment is not activated
                // Assume user wants to activate equipment
                if (slot.Equipper.Selected == null) return false;
                if (!slot.Equipper.Locks.ContainsKey (slot.Equipper.Selected)) return false;
                if ((slot.Equipper.Selected.transform.position - slot.Equipper.transform.position).sqrMagnitude > Range * Range) return false;
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

        [Serializable]
        public new class State : EquipmentPrototype.State {
            public float AccumulatedDamageMultiplier;
            public float Heat;
            public bool Activated;
            public Structure? Target;
            public GameObject? Beam;

            public State (EquipmentSlot slot, BeamLaserPrototype equipment) : base (slot, equipment) { }

            public override EquipmentPrototype.State.Serializable ToSerializable () {
                return new Serializable {
                    Durability = Durability,
                    AccumulatedDamageMultiplier = AccumulatedDamageMultiplier,
                    Heat = Heat,
                    Activated = Activated,
                    TargetId = Target == null ? "" : Target.Id,
                };
            }

            public override void FromSerializable (EquipmentPrototype.State.Serializable serializable) {
                Serializable converted = (serializable as Serializable)!;
                Durability = converted.Durability;
                AccumulatedDamageMultiplier = converted.AccumulatedDamageMultiplier;
                Heat = converted.Heat;
                Activated = converted.Activated;
                Target = Singletons.Get<StructureManager> ().GetStructure (converted.TargetId);
            }

            [Serializable]
            public new class Serializable : EquipmentPrototype.State.Serializable {
                public float AccumulatedDamageMultiplier;
                public float Heat;
                public bool Activated;
                public string TargetId = "";
            }
        }
    }
}
#nullable restore
