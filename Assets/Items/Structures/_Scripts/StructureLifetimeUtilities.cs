using System;
using DarkFrontier.Factions;
using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Locations;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace DarkFrontier.Structures
{
    public class StructureLifetimeUtilities
    {
        private readonly BehaviorManager _behaviorManager;
        private readonly StructureRegistry _registry;
        
        public StructureLifetimeUtilities(BehaviorManager behaviorManager, StructureRegistry registry)
        {
            _behaviorManager = behaviorManager;
            _registry = registry;
        }

        public Structure Create(StructurePrototype prototype, Sector sector)
        {
            var gameObject = UnityEngine.Object.Instantiate(prototype.Prefab, sector.transform);
            
            Structure structure = gameObject.GetComponent<Structure>();
            if (structure == null)
            {
                structure = gameObject.AddComponent<Structure>();
            }
            
            return structure;
        }

        public Structure Create(StructurePrototype prototype, Sector sector, Transform parent)
        {
            var structure = Create(prototype, sector);
            structure.transform.position = parent.position;
            return structure;
        }

        public Structure Create(StructurePrototype prototype, Sector sector, Transform parent, Faction? faction)
        {
            var structure = Create(prototype, sector, parent);
            structure.uFaction.UId.Value = faction?.Id ?? "";
            return structure;
        }

        public void Destroy(Structure structure, StructureDestructionMode mode)
        {
            switch (mode)
            {
                case StructureDestructionMode.Destroy:
                    Destroy(structure);
                    break;
                case StructureDestructionMode.Dispose:
                    Dispose(structure);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public void Destroy(Structure structure)
        {
            _behaviorManager.DisableImmediately(structure);
                    
            // Remove from registries
            _registry.Remove(structure);
            if (structure.uFaction.UValue != null)
            {
                structure.uFaction.UValue.Property.Remove(structure);
            }
            if (structure.uSector.UValue != null)
            {
                structure.uSector.UValue.UPopulation.Remove(structure);
            }
                    
            // Destroy docked structures
            for (int i = 0, l = structure.uDockingPoints.Count; i < l; i++)
            {
                var docker = structure.uDockingPoints[i].UDocker;
                if (docker != null)
                {
                    Dispose(docker);
                }
            }
            
            // Destroy equipment
            for (int i = 0, l = structure.uEquipment.uAll.Count; i < l; i++)
            {
                structure.uEquipment.uAll[i].ChangeEquipment(null);
            }
            
            // Spawn destruction effect
            if (structure.uPrototype != null && structure.uPrototype.DestructionEffect != null)
            {
                GameObject effect = UnityEngine.Object.Instantiate(structure.uPrototype.DestructionEffect, structure.transform.parent);
                effect.transform.localPosition = structure.transform.localPosition;
            }
            
            // TODO Drop stuff according to DropPercentage
            
            // Destroy structure game object
            UnityEngine.Object.Destroy(structure.gameObject);
        }

        public void Dispose(Structure structure)
        {
            _behaviorManager.DisableImmediately(structure);
                    
            // Remove from registries
            _registry.Remove(structure);
            if (structure.uFaction.UValue != null)
            {
                structure.uFaction.UValue.Property.Remove(structure);
            }
            if (structure.uSector.UValue != null)
            {
                structure.uSector.UValue.UPopulation.Remove(structure);
            }
                    
            // Destroy docked structures
            for (int i = 0, l = structure.uDockingPoints.Count; i < l; i++)
            {
                var docker = structure.uDockingPoints[i].UDocker;
                if (docker != null)
                {
                    Dispose(docker);
                }
            }
            
            // Destroy equipment
            for (int i = 0, l = structure.uEquipment.uAll.Count; i < l; i++)
            {
                structure.uEquipment.uAll[i].ChangeEquipment(null);
            }
            
            // Destroy structure game object
            UnityEngine.Object.Destroy(structure.gameObject);
        }
    }

    public enum StructureDestructionMode
    {
        Destroy,
        Dispose
    }
}
