using DarkFrontier.AI;
using DarkFrontier.Foundation.Serialization;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DarkFrontier.Equipment {

#nullable enable
    [CreateAssetMenu (menuName = "Items/Equipment/Weapons/Hangar Bay")]
    public class HangarBayPrototype : EquipmentPrototype {
        public float LoadingSpeed;
        public float FuelingSpeed;
        public float LaunchingSpeed;
        public int MaxSquadronSize;
        public List<HangarLaunchableSO> Launchables = new List<HangarLaunchableSO> ();
        public float RetrieveRange;

        public override void Tick (EquipmentSlot slot, float dt) {
            State state = (slot.State as State)!;

            if (state.Activated) {
                // Deploy launchables
                // If no launchables are currently launching
                if (state.LaunchSlots.FindAll (s => s.Status == State.SlotState.SlotStatus.Launching).Count == 0) {
                    foreach (State.SlotState launchSlot in state.LaunchSlots) {
                        // If launchable is loaded
                        if (launchSlot.Status == State.SlotState.SlotStatus.Loaded) {
                            launchSlot.Status = State.SlotState.SlotStatus.Launching;
                            break;
                        }
                    }
                }
            } else {
                // Retrieve launchables
                // If no launchables are currently landing
                if (state.LaunchSlots.FindAll (s => s.Status == State.SlotState.SlotStatus.Landing).Count == 0) {
                    foreach (State.SlotState launchSlot in state.LaunchSlots) {
                        // If launchable is launched
                        if (launchSlot.Status == State.SlotState.SlotStatus.Launched) {
                            // If launchable is within retrieve range
                            if (NavigationManager.Instance.GetLocalDistance (slot.Equipper, launchSlot.Structure) <= RetrieveRange) {
                                launchSlot.Status = State.SlotState.SlotStatus.Landing;
                                break;
                            }
                        }
                    }
                }
            }

            state.LaunchSlots.ForEach (s => s.Tick (slot, dt));
        }

        public override bool CanClick (EquipmentSlot slot) {
            State state = (slot.State as State)!;

            if (slot.Equipper == null) return false;

            if (state.Activated) {
                // If equipment is activated and selected is null or target
                // Assume user wants to deactivate equipment
                if (slot.Equipper.Selected == null || slot.Equipper.Selected == state.Target) return true;
                // If equipment is activated and selected is not null
                // Assume user wants to change target
                else {
                    if (!slot.Equipper.Locks.ContainsKey (slot.Equipper.Selected)) return false;
                    return true;
                }
            } else {
                // If equipment is not activated
                // Assume user wants to activate equipment
                if (slot.Equipper.Selected == null) return false;
                if (state.LaunchSlots.FindAll (s => s.Status == State.SlotState.SlotStatus.Loaded).Count == 0) return false;
                if (!slot.Equipper.Locks.ContainsKey (slot.Equipper.Selected)) return false;
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
            public bool Activated;
            public Structure? Target;
            public List<SlotState> LaunchSlots;

            public State (EquipmentSlot slot, HangarBayPrototype equipment) : base (slot, equipment) {
                LaunchSlots = new byte[equipment.MaxSquadronSize].ToList ().ConvertAll (s => new SlotState ());
            }

            public override EquipmentPrototype.State.Serializable ToSerializable () {
                return new Serializable {
                    Durability = Durability,
                    Activated = Activated,
                    TargetId = Target == null ? "" : Target.Id,
                    LaunchSlots = LaunchSlots.ConvertAll (s => s.ToSerializable ()),
                };
            }

            public override void FromSerializable (EquipmentPrototype.State.Serializable serializable) {
                Serializable converted = (serializable as Serializable)!;
                Durability = converted.Durability;
                Activated = converted.Activated;
                Target = Singletons.Get<StructureManager> ().GetStructure (converted.TargetId);
                LaunchSlots = converted.LaunchSlots.ConvertAll (s => {
                    SlotState state = new SlotState ();
                    state.FromSerializable (s);
                    return state;
                });
            }

            [Serializable]
            public class SlotState : ISavableState<SlotState.Serializable> {
                public SlotStatus Status { get => status; set => status = value; }
                [SerializeField] private SlotStatus status = SlotStatus.Unloaded;
                public HangarLaunchableSO? Launchable { get => launchable; }
                [SerializeField] private HangarLaunchableSO? launchable;
                public bool Loaded { get => launchable != null && loadingProgress == launchable.LoadingPreparation; }
                public bool Unloaded { get => launchable == null || loadingProgress == 0; }
                public float LoadingProgress { get => loadingProgress; }
                [SerializeField] private float loadingProgress;
                public bool Fueled { get => launchable != null && fuelingProgress == launchable.FuelCapacity; }
                public bool OutOfFuel { get => launchable == null || fuelingProgress == 0; }
                public float FuelingProgress { get => fuelingProgress; }
                [SerializeField] private float fuelingProgress;
                public bool Launched { get => launchable != null && launchingProgress == launchable.LaunchingPreparation && structure != null; }
                public bool Landed { get => launchable == null || launchingProgress == 0 || structure == null; }
                public float LaunchingProgress { get => launchingProgress; }
                [SerializeField] private float launchingProgress;
                public Structure? Structure { get => structure; }
                [SerializeField] private Structure? structure;

                public bool Load (HangarLaunchableSO launchable) {
                    if (status != SlotStatus.Unloaded) return false;
                    this.launchable = launchable;
                    status = SlotStatus.Loading;
                    return true;
                }

                public void Tick (EquipmentSlot slot, float dt) {
                    if (slot.Equipper == null) return;

                    HangarBayPrototype hangarBay = (slot.Equipment as HangarBayPrototype)!;
                    State state = (slot.State as State)!;

                    if (status == SlotStatus.Unloaded) {
                        // Enforce expected state
                        launchable = null;
                        loadingProgress = 0;
                        fuelingProgress = 0;
                        launchingProgress = 0;
                        // If for some reason structure is non-null, destroy it
                        if (structure != null) Singletons.Get<StructureManager> ().DisposeStructure (structure);
                    } else if (status == SlotStatus.Unloading) {
                        if (launchable == null) {
                            status = SlotStatus.Unloaded;
                            return;
                        }
                        loadingProgress = Mathf.Max (loadingProgress - hangarBay.LoadingSpeed * dt, 0);
                        // If unloaded
                        if (loadingProgress == 0) {
                            // If equipper has enough inventory space to hold the craft
                            if (slot.Equipper.Inventory.AddQuantity (launchable, 1) == 1) {
                                // Set state to unloaded
                                status = SlotStatus.Unloaded;
                            }
                        }
                    } else if (status == SlotStatus.Loading) {
                        if (launchable == null) {
                            status = SlotStatus.Unloaded;
                            return;
                        }
                        loadingProgress = Mathf.Min (loadingProgress + hangarBay.LoadingSpeed * dt, launchable.LoadingPreparation);
                        // If loaded
                        if (loadingProgress == launchable.LoadingPreparation) {
                            // Set state to loaded
                            status = SlotStatus.Loaded;
                        }
                    } else if (status == SlotStatus.Loaded) {
                        if (launchable == null) {
                            status = SlotStatus.Unloaded;
                            return;
                        }
                        // Enforce expected state
                        loadingProgress = launchable.LoadingPreparation;
                        launchingProgress = 0;
                        // Add fuel
                        fuelingProgress = Mathf.Min (fuelingProgress + hangarBay.FuelingSpeed * dt, launchable.FuelCapacity);
                    } else if (status == SlotStatus.Launching) {
                        if (launchable == null) {
                            status = SlotStatus.Unloaded;
                            return;
                        }
                        launchingProgress = Mathf.Min (launchingProgress + hangarBay.LoadingSpeed * dt, launchable.LaunchingPreparation);
                        // If launched
                        if (launchingProgress == launchable.LaunchingPreparation) {
                            // Spawn structure
                            structure = Singletons.Get<StructureManager> ().SpawnStructure (launchable, slot.Equipper.Faction.Id.Value, slot.Equipper.Sector.Id.Value, new Location (slot.transform));
                            HangarManagedCraftAI ai = CreateInstance<HangarManagedCraftAI> ();
                            ai.Launchable = launchable;
                            ai.State = state;
                            structure.AI = ai;
                            // Set state to launched
                            status = SlotStatus.Launched;
                        }
                    } else if (status == SlotStatus.Launched) {
                        if (launchable == null) {
                            status = SlotStatus.Unloaded;
                            return;
                        }
                        // If structure has been destroyed
                        if (structure == null) {
                            status = SlotStatus.Unloaded;
                            return;
                        }
                        // Enforce expected state
                        loadingProgress = launchable.LoadingPreparation;
                        launchingProgress = launchable.LaunchingPreparation;
                        // Consume fuel
                        fuelingProgress = Mathf.Max (fuelingProgress - launchable.FuelConsumption * Time.deltaTime, 0);
                        // Check fuel
                        if (fuelingProgress == 0) {
                            // If out of fuel add penalties
                            structure.Stats.GetStat (StatNames.LinearMaxSpeedMultiplier, 1).AddModifier (new StatModifier {
                                Name = "No Fuel Max Speed Penalty",
                                Id = "hangar-launched-structure-no-fuel-max-speed-penalty",
                                Value = launchable.NoFuelMaxSpeedPenalty,
                                Type = StatModifierType.Multiplicative,
                                Duration = float.PositiveInfinity,
                            });
                            structure.Stats.GetStat (StatNames.LinearAccelerationMultiplier, 1).AddModifier (new StatModifier {
                                Name = "No Fuel Acceleration Penalty",
                                Id = "hangar-launched-structure-no-fuel-acceleration-penalty",
                                Value = launchable.NoFuelAccelerationPenalty,
                                Type = StatModifierType.Multiplicative,
                                Duration = float.PositiveInfinity,
                            });
                        } else {
                            // Remove modifiers if not out of fuel
                            structure.Stats.GetStat (StatNames.LinearMaxSpeedMultiplier, 1).RemoveModifier ("hangar-launched-structure-no-fuel-max-speed-penalty");
                            structure.Stats.GetStat (StatNames.LinearAccelerationMultiplier, 1).RemoveModifier ("hangar-launched-structure-no-fuel-acceleration-penalty");
                        }
                    } else if (status == SlotStatus.Landing) {
                        if (launchable == null) {
                            status = SlotStatus.Unloaded;
                            return;
                        }
                        // If structure has been destroyed
                        if (structure == null) {
                            status = SlotStatus.Unloaded;
                            return;
                        }
                        launchingProgress = Mathf.Max (launchingProgress - hangarBay.LoadingSpeed * Time.deltaTime, 0);
                        // If landed
                        if (launchingProgress == 0) {
                            // Destroy structure
                            Singletons.Get<StructureManager> ().DisposeStructure (structure);
                            // Set state to loaded
                            status = SlotStatus.Loaded;
                        }
                    }
                }

                public enum SlotStatus {
                    Unloaded,
                    Unloading,
                    Loading,
                    Loaded,
                    Launching,
                    Launched,
                    Landing,
                }

                public Serializable ToSerializable () {
                    return new Serializable {
                        Status = Status,
                        LaunchableId = Launchable == null ? "" : Launchable.Id,
                        LoadingProgress = LoadingProgress,
                        FuelingProgress = FuelingProgress,
                        LaunchingProgress = LaunchingProgress,
                        StructureId = Structure == null ? "" : Structure.Id,
                    };
                }

                public void FromSerializable (Serializable serializable) {
                    status = serializable.Status;
                    launchable = ItemManager.Instance.GetItem (serializable.LaunchableId) as HangarLaunchableSO;
                    loadingProgress = serializable.LoadingProgress;
                    fuelingProgress = serializable.FuelingProgress;
                    launchingProgress = serializable.LaunchingProgress;
                    structure = Singletons.Get<StructureManager> ().GetStructure (serializable.StructureId);
                }

                [Serializable]
                public class Serializable {
                    public SlotStatus Status;
                    public string LaunchableId = "";
                    public float LoadingProgress;
                    public float FuelingProgress;
                    public float LaunchingProgress;
                    public string StructureId = "";
                }
            }

            [Serializable]
            public new class Serializable : EquipmentPrototype.State.Serializable {
                public bool Activated;
                public string TargetId = "";
                public List<SlotState.Serializable> LaunchSlots = new List<SlotState.Serializable> ();
            }
        }
    }
}
#nullable restore
