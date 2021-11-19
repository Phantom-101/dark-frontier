using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Locations;
using DarkFrontier.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using DarkFrontier.Controllers;
using DarkFrontier.Items;
using DarkFrontier.Items.Prototypes;
using UnityEngine;

namespace DarkFrontier.Equipment {

#nullable enable
    [CreateAssetMenu (menuName = "Items/Equipment/Weapons/Launcher")]
    public class LauncherPrototype : EquipmentPrototype {
        public float EnergyRequired;
        public float RechargeRate;
        public bool AutoCycle;
        public List<MissileSO> CompatibleMissiles = new List<MissileSO> ();

        public override void Tick (EquipmentSlot aSlot, float aDt) {
            if (aSlot.Equipper == null) return;

            State lState = (aSlot.UState as State)!;

            var lConsumption = RechargeRate * aDt;
            var lLack = EnergyRequired - lState.Charge;
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
            lState.Charge += lGiven;

            if (lState.Activated && (lState.Target == null || lState.Missile == null || !CompatibleMissiles.Contains (lState.Missile) || !aSlot.Equipper.ULocks.Keys.Any (lGetter => lGetter.UId.Value == lState.Target.UId) || !aSlot.Equipper.UInventory.HasQuantity (lState.Missile, 1) || (lState.Target.transform.position - aSlot.Equipper.transform.position).sqrMagnitude > lState.Missile.Range * lState.Missile.Range)) lState.Activated = false;

            if (lState.Activated && lState.Charge >= EnergyRequired) {
                lState.Charge = 0;
                Structure lStructure = Singletons.Get<Structures.StructureManager> ().SpawnStructure (lState.Missile!.MissileStructure, aSlot.Equipper.UFaction.UId.Value, aSlot.Equipper.USector.UId.Value, new Location (aSlot.transform));
                Singletons.Get<BehaviorManager> ().InitializeImmediately (lStructure);
                Singletons.Get<BehaviorManager> ().EnableImmediately (lStructure);
                MissileNpcController lNpcController = (MissileNpcController) lStructure.GetNpcController<MissileNpcController>();
                lNpcController.Target = lState.Target;
                lNpcController.Missile = lState.Missile;
                lNpcController.DamageMultiplier = aSlot.Equipper.ULocks.Where (lPair => lPair.Key.UValue == lState.Target).First ().Value;
                lStructure.UNpcController = lNpcController;
                aSlot.Equipper.UInventory.RemoveQuantity (lState.Missile, 1);
                lState.Activated = AutoCycle;
            }
        }

        public override bool CanClick (EquipmentSlot slot) {
            if (slot.Equipper == null) return false;

            State state = (slot.UState as State)!;

            if (state.Activated) {
                // If equipment is activated and selected is null or target
                // Assume user wants to deactivate equipment
                if (slot.Equipper.USelected.UValue == null || slot.Equipper.USelected.UValue == state.Target) return true;
                // If equipment is activated and selected is not null
                // Assume user wants to change target
                else {
                    if (state.Missile == null) return false;
                    if (!CompatibleMissiles.Contains (state.Missile)) return false;
                    if (!slot.Equipper.ULocks.Keys.Any (lGetter => lGetter.UValue == slot.Equipper.USelected.UValue)) return false;
                    if (!slot.Equipper.UInventory.HasQuantity (state.Missile, 1)) return false;
                    if ((slot.Equipper.USelected.UValue.transform.position - slot.Equipper.transform.position).sqrMagnitude > state.Missile.Range * state.Missile.Range) return false;
                    return true;
                }
            } else {
                // If equipment is not activated
                // Assume user wants to activate equipment
                if (slot.Equipper.USelected.UValue == null) return false;
                if (state.Missile == null) return false;
                if (!CompatibleMissiles.Contains (state.Missile)) return false;
                if (!slot.Equipper.ULocks.Keys.Any (lGetter => lGetter.UValue == slot.Equipper.USelected.UValue)) return false;
                if (!slot.Equipper.UInventory.HasQuantity (state.Missile, 1)) return false;
                if ((slot.Equipper.USelected.UValue.transform.position - slot.Equipper.transform.position).sqrMagnitude > state.Missile.Range * state.Missile.Range) return false;
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
                if (slot.Equipper.USelected.UValue == null || slot.Equipper.USelected.UValue == state.Target) state.Activated = false;
                // If equipment is activated and selected is not null
                // Assume user wants to change target
                else state.Target = slot.Equipper.USelected.UValue;
            } else {
                // If equipment is not activated
                // Assume user wants to activate equipment
                state.Activated = true;
                state.Target = slot.Equipper.USelected.UValue;
            }
        }

        public override void EnsureStateType (EquipmentSlot slot) {
            if (!(slot.UnsafeState is State)) slot.UnsafeState = GetNewState (slot);
        }

        public override EquipmentPrototype.State GetNewState (EquipmentSlot slot) => new State (slot, this);

        [Serializable]
        public new class State : EquipmentPrototype.State {
            public float Charge;
            public bool Activated;
            public Structure? Target;
            public MissileSO? Missile;

            public State (EquipmentSlot slot, LauncherPrototype equipment) : base (slot, equipment) { }

            public override EquipmentPrototype.State.Serializable ToSerializable () {
                return new Serializable {
                    Durability = Durability,
                    Charge = Charge,
                    Activated = Activated,
                    TargetId = Target == null ? "" : Target.UId,
                    MissileId = Missile == null ? "" : Missile.Id,
                };
            }

            public override void FromSerializable (EquipmentPrototype.State.Serializable serializable) {
                Serializable converted = (serializable as Serializable)!;
                Durability = converted.Durability;
                Charge = converted.Charge;
                Activated = converted.Activated;
                Target = Singletons.Get<Structures.StructureManager> ().GetStructure (converted.TargetId);
                Missile = ItemManager.Instance.GetItem (converted.MissileId) as MissileSO;
            }

            [Serializable]
            public new class Serializable : EquipmentPrototype.State.Serializable {
                public float Charge;
                public bool Activated;
                public string TargetId = "";
                public string MissileId = "";
            }
        }
    }
}
#nullable restore
