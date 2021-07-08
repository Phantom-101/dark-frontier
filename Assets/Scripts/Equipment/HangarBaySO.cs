using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Equipment/Weapons/Hangar Bay")]
public class HangarBaySO : EquipmentSO {
    //public float RefuelRate;
    public float HangarVolume;
    public int MaxSquadronSize;
    public List<CraftSO> LaunchableCrafts;
    public float RetrieveRange;

    public override void OnAwake (EquipmentSlot slot) => EnsureDataType (slot);

    public override void OnEquip (EquipmentSlot slot) {
        slot.Data = new HangarBaySlotData {
            Slot = slot,
            Equipment = this,
            Durability = Durability,
            Activated = false,
            Target = null,
            StowedCrafts = new CappedSingleInventory (MaxSquadronSize, HangarVolume),
            ActiveCrafts = new List<HangarManagedCraft> (),
        };
    }

    public override void OnUnequip (EquipmentSlot slot) {
        slot.Data = new EquipmentSlotData {
            Slot = slot,
            Equipment = null,
            Durability = 0,
        };
    }

    public override void Tick (EquipmentSlot slot) {
        EnsureDataType (slot);

        HangarBaySlotData data = slot.Data as HangarBaySlotData;

        if (data.Activated) {
            // Deploy crafts
            CraftSO craft = data.StowedCrafts.Item as CraftSO;
            int quantity = data.StowedCrafts.RemoveQuantity (craft, data.StowedCrafts.Quantity);
            for (int i = 0; i < quantity; i++) {
                Structure structure = StructureManager.Instance.SpawnStructure (craft, slot.Equipper.Faction, slot.Equipper.Sector, new Location (slot.LocalPosition));
                HangarManagedCraftAI ai = CreateInstance<HangarManagedCraftAI> ();
                ai.Craft = craft;
                ai.HangarBay = data;
                structure.AI = ai;
                data.ActiveCrafts.Add (new HangarManagedCraft {
                    Structure = structure,
                    Craft = craft,
                    FuelLevel = craft.FuelCapacity,
                });
            }
            // Consume fuel
            data.ActiveCrafts.ForEach (c => {
                c.FuelLevel = Mathf.Max (c.FuelLevel - c.Craft.FuelConsumption * Time.deltaTime, 0);
                if (c.FuelLevel == 0) {
                    c.MaxSpeedPenalty = new StatModifier {
                        Name = "No Fuel Max Speed Penalty",
                        Id = "hangar-managed-craft-no-fuel-max-speed-penalty",
                        Value = c.Craft.NoFuelMaxSpeedPenalty,
                        Type = StatModifierType.Multiplicative,
                        Duration = float.PositiveInfinity,
                    };
                    c.Structure.Stats.GetStat (StatNames.LinearMaxSpeedMultiplier, 1).AddModifier (c.MaxSpeedPenalty);
                    c.AccelerationPenalty = new StatModifier {
                        Name = "No Fuel Acceleration Penalty",
                        Id = "hangar-managed-craft-no-fuel-acceleration-penalty",
                        Value = c.Craft.NoFuelAccelerationPenalty,
                        Type = StatModifierType.Multiplicative,
                        Duration = float.PositiveInfinity,
                    };
                    c.Structure.Stats.GetStat (StatNames.LinearAccelerationMultiplier, 1).AddModifier (c.AccelerationPenalty);
                }
            });
        } else {
            data.ActiveCrafts.FindAll (c => NavigationManager.Instance.GetLocalDistance (slot.Equipper, c.Structure) <= RetrieveRange).ForEach (c => {
                data.ActiveCrafts.Remove (c);
                data.StowedCrafts.AddQuantity (c.Structure.Profile, 1);
                StructureManager.Instance.DisposeStructure (c.Structure);
            });
        }
    }

    public override void FixedTick (EquipmentSlot slot) { }

    public override bool CanClick (EquipmentSlot slot) {
        HangarBaySlotData data = slot.Data as HangarBaySlotData;
        if (data.Activated) {
            // If equipment is activated and selected is null or target
            // Assume user wants to deactivate equipment
            if (slot.Equipper.Selected == null || slot.Equipper.Selected == data.Target) return true;
            // If equipment is activated and selected is not null
            // Assume user wants to change target
            else {
                if (data.StowedCrafts.Quantity + data.ActiveCrafts.Count == 0) return false;
                CraftSO stowed = data.StowedCrafts.Item as CraftSO;
                if (stowed == null) stowed = data.ActiveCrafts[0].Craft;
                if (!LaunchableCrafts.Contains (stowed)) return false;
                if (!slot.Equipper.Locks.ContainsKey (slot.Equipper.Selected)) return false;
                if ((slot.Equipper.Selected.transform.position - slot.Equipper.transform.position).sqrMagnitude > stowed.MaxOperationalRange * stowed.MaxOperationalRange) return false;
                return true;
            }
        } else {
            // If equipment is not activated
            // Assume user wants to activate equipment
            if (slot.Equipper.Selected == null) return false;
            if (data.StowedCrafts.Quantity + data.ActiveCrafts.Count == 0) return false;
            CraftSO stowed = data.StowedCrafts.Item as CraftSO;
            if (stowed == null) stowed = data.ActiveCrafts[0].Craft;
            if (!LaunchableCrafts.Contains (stowed)) return false;
            if (!slot.Equipper.Locks.ContainsKey (slot.Equipper.Selected)) return false;
            if ((slot.Equipper.Selected.transform.position - slot.Equipper.transform.position).sqrMagnitude > stowed.MaxOperationalRange * stowed.MaxOperationalRange) return false;
            return true;
        }
    }

    public override void OnClicked (EquipmentSlot slot) {
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
            StowedCrafts = new CappedSingleInventory (MaxSquadronSize, HangarVolume),
            ActiveCrafts = new List<HangarManagedCraft> (),
        };
    }
}

[Serializable]
public class HangarBaySlotData : EquipmentSlotData {
    public bool Activated;
    public Structure Target;
    public CappedSingleInventory StowedCrafts;
    public List<HangarManagedCraft> ActiveCrafts;

    public override EquipmentSlotSaveData Save () {
        return new HangarBaySlotSaveData {
            EquipmentId = Equipment == null ? "" : Equipment.Id,
            Durability = Durability,
            Activated = Activated,
            TargetId = Target == null ? "" : Target.Id,
            StowedCrafts = StowedCrafts.Save (),
            ActiveCrafts = ActiveCrafts.ConvertAll (c => c.Save ()),
        };
    }
}

[Serializable]
public class HangarBaySlotSaveData : EquipmentSlotSaveData {
    public bool Activated;
    public string TargetId;
    public CappedSingleInventorySaveData StowedCrafts;
    public List<HangarManagedCraftSaveData> ActiveCrafts;

    public override EquipmentSlotData Load () {
        return new HangarBaySlotData {
            Equipment = ItemManager.Instance.GetItem (EquipmentId) as EquipmentSO,
            Durability = Durability,
            Activated = Activated,
            Target = StructureManager.Instance.GetStructure (TargetId),
            StowedCrafts = StowedCrafts.Load (),
            ActiveCrafts = ActiveCrafts.ConvertAll (c => c.Load ()),
        };
    }
}

[Serializable]
public class HangarManagedCraft : ISaveTo<HangarManagedCraftSaveData> {
    public Structure Structure;
    public CraftSO Craft;
    public float FuelLevel;
    public StatModifier MaxSpeedPenalty;
    public StatModifier AccelerationPenalty;

    public HangarManagedCraftSaveData Save () {
        return new HangarManagedCraftSaveData {
            StructureId = Structure.Id,
            CraftId = Craft.Id,
            FuelLevel = FuelLevel,
            MaxSpeedPenalty = MaxSpeedPenalty,
            AccelerationPenalty = AccelerationPenalty,
        };
    }
}

public class HangarManagedCraftSaveData : ILoadTo<HangarManagedCraft> {
    public string StructureId;
    public string CraftId;
    public float FuelLevel;
    public StatModifier MaxSpeedPenalty;
    public StatModifier AccelerationPenalty;

    public HangarManagedCraft Load () {
        return new HangarManagedCraft {
            Structure = StructureManager.Instance.GetStructure (StructureId),
            Craft = ItemManager.Instance.GetItem (CraftId) as CraftSO,
            FuelLevel = FuelLevel,
            MaxSpeedPenalty = MaxSpeedPenalty,
            AccelerationPenalty = AccelerationPenalty,
        };
    }
}