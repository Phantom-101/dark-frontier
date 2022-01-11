using DarkFrontier.Foundation.Extensions;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Locations;
using DarkFrontier.Structures;
using System;
using System.Linq;
using DarkFrontier.DataStructures;
using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Items.Structures;
using DarkFrontier.Visuals;
using UnityEngine;

namespace DarkFrontier.Equipment {

#nullable enable
    [CreateAssetMenu (menuName = "Items/Equipment/Weapons/Phased Beam Laser")]
    public class PhasedBeamLaserPrototype : EquipmentPrototype {
        public float Range;
        public Damage DamagePerSecond;
        public float DamageInterval;
        public float EnergyConsumption;
        public float MaxHeat;
        public float HeatGeneration;
        public float CoolingRate;
        public float OverheatDamage;
        public AnimationCurve HeatDamageMultiplier = new AnimationCurve ();
        public StructurePrototypeToFloatDictionary DamageMultipliers = new StructurePrototypeToFloatDictionary ();
        public GameObject? BeamPrefab;
        public Material? BeamMaterial;
        public AnimationCurve BeamAlpha = new AnimationCurve();

        public override void OnUnequip (EquipmentSlot aSlot) {
            State lState = (aSlot.UState as State)!;
            if (lState.Beam == null) return;
            DisableVisuals(lState.Beam);
            lState.Beam = null;
        }

        public override void Tick (EquipmentSlot aSlot, float aDt) {
            if (aSlot.Equipper == null) return;

            State lState = (State) aSlot.UState;

            if (lState.Activated && (lState.Target == null || !aSlot.Equipper.uLocks.Keys.Any (lGetter => lGetter.UId.Value == lState.Target.uId) || (lState.Target.transform.position - aSlot.Equipper.transform.position).sqrMagnitude > Range * Range)) lState.Activated = false;

            lState.Heat = Mathf.Clamp (lState.Heat - CoolingRate * aDt + (lState.Activated ? HeatGeneration * aDt : 0), 0, MaxHeat);

            if (lState.Activated) {
                if (lState.Beam == null) {
                    lState.Beam = (LaserVisuals) Singletons.Get<BehaviorPooler>().Take("laser-visuals", SpawnVisuals);
                    lState.Beam.uFrom = new Location(aSlot.transform);
                    lState.Beam.uTo = new Location(lState.Target!.transform);
                    lState.Beam.uMaterial = BeamMaterial;
                    lState.Beam.uAlpha = BeamAlpha;
                    lState.Beam.uPeriod = DamageInterval;
                    lState.Beam.uRepeat = true;
                    lState.Beam.Enable();
                }
                
                var lConsumption = EnergyConsumption * aDt;
                float lGiven = 0;
                var lCapacitors = aSlot.Equipper.uEquipment.States<CapacitorPrototype.State>();
                var lCount = lCapacitors.Count;
                for (var lIndex = 0; lIndex < lCount; lIndex++) {
                    var lCapacitor = lCapacitors[lIndex];
                    var lChargeLeft = lCapacitor.Charge;
                    var lDischargeLeft = lCapacitor.DischargeLeft;
                    var lAllocated = Mathf.Min (lChargeLeft, lDischargeLeft, lConsumption - lGiven);
                    lGiven += lAllocated;
                    lCapacitor.Charge -= lAllocated;
                    lCapacitor.DischargeLeft -= lAllocated;
                }
                lState.AccumulatedDamageMultiplier += lGiven / lConsumption * aDt;
                /* Use raycast?
                if (Physics.Raycast (slot.Equipper.transform.position, data.Target.transform.position - slot.Equipper.transform.position, out RaycastHit hit, Range)) {
                    Structure structure = hit.transform.gameObject.GetComponentInParent<Structure> ();
                    if (structure != null) structure.TakeDamage (Damage * HeatDamageMultiplier.Evaluate (data.Heat / MaxHeat), hit.point);
                }
                */
                if (lState.AccumulatedDamageMultiplier >= DamageInterval) {
                    lState.Target.TakeDamage (DamagePerSecond * lState.AccumulatedDamageMultiplier * HeatDamageMultiplier.Evaluate (lState.Heat / MaxHeat) * DamageMultipliers.TryGet (lState.Target.uPrototype, 1), new Location (aSlot.Equipper.transform));
                    lState.AccumulatedDamageMultiplier = 0;
                }
            } else {
                lState.AccumulatedDamageMultiplier = 0;
                if (lState.Beam != null) {
                    DisableVisuals(lState.Beam);
                    lState.Beam = null;
                }
            }

            if (lState.Heat == MaxHeat) {
                lState.Durability = Mathf.Max (lState.Durability - OverheatDamage * aDt, 0);
                if (lState.Durability == 0) {
                    aSlot.ChangeEquipment(null);
                }
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
                    if ((slot.Equipper.uSelected.UValue.transform.position - slot.Equipper.transform.position).sqrMagnitude > Range * Range) return false;
                    return true;
                }
            } else {
                // If equipment is not activated
                // Assume user wants to activate equipment
                if (slot.Equipper.uSelected.UValue == null) return false;
                if (!slot.Equipper.uLocks.Keys.Any (lGetter => lGetter.UValue == slot.Equipper.uSelected.UValue)) return false;
                if ((slot.Equipper.uSelected.UValue.transform.position - slot.Equipper.transform.position).sqrMagnitude > Range * Range) return false;
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

        private IBehavior SpawnVisuals() {
            var lVisuals = Instantiate(BeamPrefab)!.GetComponent<LaserVisuals>();
            lVisuals.Disable();
            return lVisuals;
        }

        private void DisableVisuals(LaserVisuals aVisuals) {
            aVisuals.Disable();
            Singletons.Get<BehaviorPooler>().Reclaim("laser-visuals", aVisuals);
        }
        
        [Serializable]
        public new class State : EquipmentPrototype.State {
            public float AccumulatedDamageMultiplier;
            public float Heat;
            public bool Activated;
            public Structure? Target;
            public LaserVisuals? Beam;

            public State (EquipmentSlot slot, PhasedBeamLaserPrototype equipment) : base (slot, equipment) { }

            public override EquipmentPrototype.State.Serializable ToSerializable () {
                return new Serializable {
                    Durability = Durability,
                    AccumulatedDamageMultiplier = AccumulatedDamageMultiplier,
                    Heat = Heat,
                    Activated = Activated,
                    TargetId = Target == null ? "" : Target.uId,
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
