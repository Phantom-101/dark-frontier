using DarkFrontier.AI;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Structures;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkFrontier.Equipment {

#nullable enable
    [CreateAssetMenu (menuName = "Items/Equipment/Weapons/Launcher")]
    public class LauncherPrototype : EquipmentPrototype {
        public float EnergyRequired;
        public float RechargeRate;
        public bool AutoCycle;
        public List<MissileSO> CompatibleMissiles = new List<MissileSO> ();

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

            if (state.Activated && (state.Target == null || state.Missile == null || !CompatibleMissiles.Contains (state.Missile) || !slot.Equipper.Locks.ContainsKey (state.Target) || !slot.Equipper.Inventory.HasQuantity (state.Missile, 1) || (state.Target.transform.position - slot.Equipper.transform.position).sqrMagnitude > state.Missile.Range * state.Missile.Range)) state.Activated = false;

            if (state.Activated && state.Charge >= EnergyRequired) {
                state.Charge = 0;
                Structure structure = Singletons.Get<StructureManager> ().SpawnStructure (state.Missile!.MissileStructure, slot.Equipper.Faction.Id.Value, slot.Equipper.Sector.Id.Value, new Location (slot.transform));
                structure.TryInitialize ();
                MissileAI ai = CreateInstance<MissileAI> ();
                ai.Target = state.Target;
                ai.Missile = state.Missile;
                ai.DamageMultiplier = slot.Equipper.Locks[state.Target] / 100;
                structure.AI = ai;
                slot.Equipper.Inventory.RemoveQuantity (state.Missile, 1);
                state.Activated = AutoCycle;
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
                    if (state.Missile == null) return false;
                    if (!CompatibleMissiles.Contains (state.Missile)) return false;
                    if (!slot.Equipper.Locks.ContainsKey (slot.Equipper.Selected)) return false;
                    if (!slot.Equipper.Inventory.HasQuantity (state.Missile, 1)) return false;
                    if ((slot.Equipper.Selected.transform.position - slot.Equipper.transform.position).sqrMagnitude > state.Missile.Range * state.Missile.Range) return false;
                    return true;
                }
            } else {
                // If equipment is not activated
                // Assume user wants to activate equipment
                if (slot.Equipper.Selected == null) return false;
                if (state.Missile == null) return false;
                if (!CompatibleMissiles.Contains (state.Missile)) return false;
                if (!slot.Equipper.Locks.ContainsKey (slot.Equipper.Selected)) return false;
                if (!slot.Equipper.Inventory.HasQuantity (state.Missile, 1)) return false;
                if ((slot.Equipper.Selected.transform.position - slot.Equipper.transform.position).sqrMagnitude > state.Missile.Range * state.Missile.Range) return false;
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
                    TargetId = Target == null ? "" : Target.Id,
                    MissileId = Missile == null ? "" : Missile.Id,
                };
            }

            public override void FromSerializable (EquipmentPrototype.State.Serializable serializable) {
                Serializable converted = (serializable as Serializable)!;
                Durability = converted.Durability;
                Charge = converted.Charge;
                Activated = converted.Activated;
                Target = Singletons.Get<StructureManager> ().GetStructure (converted.TargetId);
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
