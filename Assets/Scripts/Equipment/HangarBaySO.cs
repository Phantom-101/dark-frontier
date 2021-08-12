using DarkFrontier.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

[CreateAssetMenu (menuName = "Items/Equipment/Weapons/Hangar Bay")]
public class HangarBaySO : EquipmentSO {
    public float LoadingSpeed;
    public float FuelingSpeed;
    public float LaunchingSpeed;
    public int MaxSquadronSize;
    public List<HangarLaunchableSO> Launchables;
    public float RetrieveRange;

    public override void OnAwake (EquipmentSlot slot) => EnsureDataType (slot);

    public override void OnEquip (EquipmentSlot slot) {
        slot.Data = new HangarBaySlotData {
            Slot = slot,
            Equipment = this,
            Durability = Durability,
            Activated = false,
            Target = null,
            LaunchSlots = (new byte[MaxSquadronSize]).Select (x => new HangarBayLaunchSlot ()).ToList (),
        };
    }

    public override void OnUnequip (EquipmentSlot slot) {
        slot.Data = new EquipmentSlotData {
            Slot = slot,
            Equipment = null,
            Durability = 0,
        };
    }

    public override void Tick (EquipmentSlot slot, float dt) {
        EnsureDataType (slot);

        HangarBaySlotData data = slot.Data as HangarBaySlotData;

        if (data.Activated) {
            // Deploy launchables
            // If no launchables are currently launching
            if (data.LaunchSlots.FindAll (s => s.State == HangarBayLaunchSlotState.Launching).Count == 0) {
                foreach (HangarBayLaunchSlot launchSlot in data.LaunchSlots) {
                    // If launchable is loaded
                    if (launchSlot.State == HangarBayLaunchSlotState.Loaded) {
                        launchSlot.State = HangarBayLaunchSlotState.Launching;
                        break;
                    }
                }
            }
        } else {
            // Retrieve launchables
            // If no launchables are currently landing
            if (data.LaunchSlots.FindAll (s => s.State == HangarBayLaunchSlotState.Landing).Count == 0) {
                foreach (HangarBayLaunchSlot launchSlot in data.LaunchSlots) {
                    // If launchable is launched
                    if (launchSlot.State == HangarBayLaunchSlotState.Launched) {
                        // If launchable is within retrieve range
                        if (NavigationManager.Instance.GetLocalDistance (slot.Equipper, launchSlot.Structure) <= RetrieveRange) {
                            launchSlot.State = HangarBayLaunchSlotState.Landing;
                            break;
                        }
                    }
                }
            }
        }

        data.LaunchSlots.ForEach (s => s.Tick (slot, dt));
    }

    public override void FixedTick (EquipmentSlot slot, float dt) { }

    public override bool CanClick (EquipmentSlot slot) {
        HangarBaySlotData data = slot.Data as HangarBaySlotData;
        if (data.Activated) {
            // If equipment is activated and selected is null or target
            // Assume user wants to deactivate equipment
            if (slot.Equipper.Selected == null || slot.Equipper.Selected == data.Target) return true;
            // If equipment is activated and selected is not null
            // Assume user wants to change target
            else {
                if (data.LaunchSlots.FindAll (s => s.State == HangarBayLaunchSlotState.Loaded).Count == 0) return false;
                if (!slot.Equipper.Locks.ContainsKey (slot.Equipper.Selected)) return false;
                return true;
            }
        } else {
            // If equipment is not activated
            // Assume user wants to activate equipment
            if (slot.Equipper.Selected == null) return false;
            if (data.LaunchSlots.FindAll (s => s.State == HangarBayLaunchSlotState.Loaded).Count == 0) return false;
            if (!slot.Equipper.Locks.ContainsKey (slot.Equipper.Selected)) return false;
            return true;
        }
    }

    public override void OnClicked (EquipmentSlot slot) {
        if (!CanClick (slot)) return;
        HangarBaySlotData data = slot.Data as HangarBaySlotData;
        if (data.Activated) {
            // If equipment is activated and selected is null or target
            // Assume user wants to deactivate equipment
            if (slot.Equipper.Selected == null || slot.Equipper.Selected == data.Target) data.Activated = false;
            // If equipment is activated and selected is not null
            // Assume user wants to change target
            else data.Target = slot.Equipper.Selected;
        } else {
            // If equipment is not activated
            // Assume user wants to activate equipment
            data.Activated = true;
            data.Target = slot.Equipper.Selected;
        }
    }

    public override void EnsureDataType (EquipmentSlot slot) {
        if (!(slot.Data is HangarBaySlotData)) slot.Data = new HangarBaySlotData {
            Slot = slot,
            Equipment = this,
            Durability = Durability,
            Activated = false,
            Target = null,
            LaunchSlots = (new byte[MaxSquadronSize]).Select (x => new HangarBayLaunchSlot ()).ToList (),
        };
    }
}

[Serializable]
public class HangarBaySlotData : EquipmentSlotData {
    public bool Activated;
    public Structure Target;
    public List<HangarBayLaunchSlot> LaunchSlots;

    public override EquipmentSlotSaveData Save () {
        return new HangarBaySlotSaveData {
            EquipmentId = Equipment == null ? "" : Equipment.Id,
            Durability = Durability,
            Activated = Activated,
            TargetId = Target == null ? "" : Target.Id,
            LaunchSlots = LaunchSlots.ConvertAll (s => s.Save ()),
        };
    }
}

[Serializable]
public class HangarBaySlotSaveData : EquipmentSlotSaveData {
    public bool Activated;
    public string TargetId;
    public List<HangarBayLaunchSlotSaveData> LaunchSlots;

    private StructureManager structureManager;

    [Inject]
    public void Construct (StructureManager structureManager) {
        this.structureManager = structureManager;
    }

    public override EquipmentSlotData Load () {
        return new HangarBaySlotData {
            Equipment = ItemManager.Instance.GetItem (EquipmentId) as EquipmentSO,
            Durability = Durability,
            Activated = Activated,
            Target = structureManager.GetStructure (TargetId),
            LaunchSlots = LaunchSlots.ConvertAll (s => s.Load ()),
        };
    }
}

[Serializable]
public class HangarBayLaunchSlot : ISaveTo<HangarBayLaunchSlotSaveData> {
    public HangarBayLaunchSlotState State { get => state; set => state = value; }
    [SerializeField] private HangarBayLaunchSlotState state = HangarBayLaunchSlotState.Unloaded;
    public HangarLaunchableSO Launchable { get => launchable; }
    [SerializeField] private HangarLaunchableSO launchable;
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
    public Structure Structure { get => structure; }
    [SerializeField] private Structure structure;

    private StructureManager structureManager;

    [Inject]
    public void Construct (StructureManager structureManager) {
        this.structureManager = structureManager;
    }

    public HangarBayLaunchSlot () { }
    public HangarBayLaunchSlot (HangarBayLaunchSlotSaveData saveData) {
        state = saveData.State;
        launchable = ItemManager.Instance.GetItem (saveData.LaunchableId) as HangarLaunchableSO;
        loadingProgress = saveData.LoadingProgress;
        fuelingProgress = saveData.FuelingProgress;
        launchingProgress = saveData.LaunchingProgress;
        structure = structureManager.GetStructure (saveData.StructureId);
    }

    public bool Load (HangarLaunchableSO launchable) {
        if (state != HangarBayLaunchSlotState.Unloaded) return false;
        this.launchable = launchable;
        state = HangarBayLaunchSlotState.Loading;
        return true;
    }

    public void Tick (EquipmentSlot slot, float dt) {
        HangarBaySO hangarBay = slot.Data.Equipment as HangarBaySO;
        HangarBaySlotData data = slot.Data as HangarBaySlotData;

        if (state == HangarBayLaunchSlotState.Unloaded) {
            // Enforce expected state
            launchable = null;
            loadingProgress = 0;
            fuelingProgress = 0;
            launchingProgress = 0;
            // If for some reason structure is non-null, destroy it
            if (structure != null) structureManager.DisposeStructure (structure);
        } else if (state == HangarBayLaunchSlotState.Unloading) {
            if (launchable == null) {
                state = HangarBayLaunchSlotState.Unloaded;
                return;
            }
            loadingProgress = Mathf.Max (loadingProgress - hangarBay.LoadingSpeed * dt, 0);
            // If unloaded
            if (loadingProgress == 0) {
                // If equipper has enough inventory space to hold the craft
                if (slot.Equipper.Inventory.AddQuantity (launchable, 1) == 1) {
                    // Set state to unloaded
                    state = HangarBayLaunchSlotState.Unloaded;
                }
            }
        } else if (state == HangarBayLaunchSlotState.Loading) {
            if (launchable == null) {
                state = HangarBayLaunchSlotState.Unloaded;
                return;
            }
            loadingProgress = Mathf.Min (loadingProgress + hangarBay.LoadingSpeed * dt, launchable.LoadingPreparation);
            // If loaded
            if (loadingProgress == launchable.LoadingPreparation) {
                // Set state to loaded
                state = HangarBayLaunchSlotState.Loaded;
            }
        } else if (state == HangarBayLaunchSlotState.Loaded) {
            if (launchable == null) {
                state = HangarBayLaunchSlotState.Unloaded;
                return;
            }
            // Enforce expected state
            loadingProgress = launchable.LoadingPreparation;
            launchingProgress = 0;
            // Add fuel
            fuelingProgress = Mathf.Min (fuelingProgress + hangarBay.FuelingSpeed * dt, launchable.FuelCapacity);
        } else if (state == HangarBayLaunchSlotState.Launching) {
            if (launchable == null) {
                state = HangarBayLaunchSlotState.Unloaded;
                return;
            }
            launchingProgress = Mathf.Min (launchingProgress + hangarBay.LoadingSpeed * dt, launchable.LaunchingPreparation);
            // If launched
            if (launchingProgress == launchable.LaunchingPreparation) {
                // Spawn structure
                structure = structureManager.SpawnStructure (launchable, slot.Equipper.Faction.Value (FactionManager.Instance.GetFaction), slot.Equipper.Sector.Value (SectorManager.Instance.GetSector), new Location (slot.LocalPosition));
                HangarManagedCraftAI ai = ScriptableObject.CreateInstance<HangarManagedCraftAI> ();
                ai.Launchable = launchable;
                ai.HangarBay = data;
                structure.AI = ai;
                // Set state to launched
                state = HangarBayLaunchSlotState.Launched;
            }
        } else if (state == HangarBayLaunchSlotState.Launched) {
            if (launchable == null) {
                state = HangarBayLaunchSlotState.Unloaded;
                return;
            }
            // If structure has been destroyed
            if (structure == null) {
                state = HangarBayLaunchSlotState.Unloaded;
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
        } else if (state == HangarBayLaunchSlotState.Landing) {
            if (launchable == null) {
                state = HangarBayLaunchSlotState.Unloaded;
                return;
            }
            // If structure has been destroyed
            if (structure == null) {
                state = HangarBayLaunchSlotState.Unloaded;
                return;
            }
            launchingProgress = Mathf.Max (launchingProgress - hangarBay.LoadingSpeed * Time.deltaTime, 0);
            // If landed
            if (launchingProgress == 0) {
                // Destroy structure
                structureManager.DisposeStructure (structure);
                // Set state to loaded
                state = HangarBayLaunchSlotState.Loaded;
            }
        }
    }

    public HangarBayLaunchSlotSaveData Save () {
        return new HangarBayLaunchSlotSaveData {
            State = state,
            LaunchableId = launchable.Id,
            LoadingProgress = loadingProgress,
            FuelingProgress = fuelingProgress,
            LaunchingProgress = launchingProgress,
            StructureId = structure.Id,
        };
    }
}

public class HangarBayLaunchSlotSaveData : ILoadTo<HangarBayLaunchSlot> {
    public HangarBayLaunchSlotState State;
    public string LaunchableId;
    public float LoadingProgress;
    public float FuelingProgress;
    public float LaunchingProgress;
    public string StructureId;

    public HangarBayLaunchSlot Load () => new HangarBayLaunchSlot (this);
}

public enum HangarBayLaunchSlotState {
    Unloaded,
    Unloading,
    Loading,
    Loaded,
    Launching,
    Launched,
    Landing,
}
