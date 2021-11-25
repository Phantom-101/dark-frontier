using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Locations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DarkFrontier.Files;
using DarkFrontier.Items.Prototypes;
using UnityEngine;

namespace DarkFrontier.Structures {
    public class StructureManager : ComponentBehavior {
        public StructureRegistry Registry => registry;
#pragma warning disable IDE0044 // Add readonly modifier
        [SerializeReference] private StructureRegistry registry = new StructureRegistry ();
#pragma warning restore IDE0044 // Add readonly modifier

        private readonly Ticker ticker = new Ticker ();
        private readonly Ticker fixedTicker = new Ticker ();

        public override void Enable () {
            Singletons.Get<BehaviorTimekeeper> ().Subscribe (this);
            ticker.Notifier += TickStructures;
            fixedTicker.Notifier += FixedTickStructures;
        }

        public override void Disable () {
            Singletons.Get<BehaviorTimekeeper> ().Unsubscribe (this);
            ticker.Notifier -= TickStructures;
            fixedTicker.Notifier -= FixedTickStructures;
        }

        public override void Tick (object aTicker, float aDt) {
            ticker.Tick (this, aDt);
        }

        public override void FixedTick (object aTicker, float aDt) {
            fixedTicker.Tick (this, aDt);
        }

        private void TickStructures(object aSender, float aArgs) {
            var lStructures = registry.UStructures;
            var lLength = lStructures.Count;
            for (var lIndex = 0; lIndex < lLength; lIndex++) {
                lStructures[lIndex].Tick(this, aArgs);
            }
        }

        private void FixedTickStructures(object aSender, float aArgs) {
            var lStructures = registry.UStructures;
            var lLength = lStructures.Count;
            for (var lIndex = 0; lIndex < lLength; lIndex++) {
                lStructures[lIndex].FixedTick(this, aArgs);
            }
        }

        public Structure GetStructure(string aId) {
            var lStructures = registry.UStructures;
            var lLength = lStructures.Count;
            for (var lIndex = 0; lIndex < lLength; lIndex++) {
                if (lStructures[lIndex].UId == aId) {
                    return lStructures[lIndex];
                }
            }
            return null;
        }
        
        public Structure SpawnStructure (StructurePrototype profile, string faction, string sector, Location location) {
            GameObject gameObject = Instantiate (profile.Prefab, location.UPosition, Quaternion.identity);
            Structure spawned = gameObject.GetComponent<Structure> ();
            if (spawned == null) {
                spawned = gameObject.AddComponent<Structure> ();
            }
            spawned.UFaction.UId.Value = faction;
            spawned.USector.UId.Value = sector;
            if (spawned.USector.UValue != null) {
                spawned.transform.SetParent (spawned.USector.UValue.transform);
            }
            return spawned;
        }

        public void DestroyStructure (Structure structure) {
            if (structure == null) {
                return;
            }
            Singletons.Get<BehaviorManager> ().DisableImmediately (structure);
            // Remove from registries
            registry.Remove (structure);
            structure.UFaction.UValue?.Property.Remove (structure);
            if (structure.USector.UValue != null) structure.USector.UValue.UPopulation.Remove (structure);
            // Destroy docked structures
            structure.UDockingPoints.ForEach (e => DestroyStructure (e.UDocker));
            // Destroy equipment
            structure.UEquipment.UAll.ForEach(aSlot => aSlot.ChangeEquipment(null));
            // Spawn destruction effect
            if (structure.UPrototype.DestructionEffect != null) {
                GameObject effect = Instantiate (structure.UPrototype.DestructionEffect, structure.transform.parent);
                effect.transform.localPosition = structure.transform.localPosition;
            }
            // TODO Drop stuff according to StructureSO.DropPercentage
            // Destroy structure game object
            Destroy (structure.gameObject);
        }

        public void DisposeStructure (Structure structure) {
            if (structure == null) {
                return;
            }
            Singletons.Get<BehaviorManager> ().DisableImmediately (structure);
            // Remove from registries
            registry.Remove (structure);
            structure.UFaction.UValue?.Property.Remove (structure);
            if (structure.USector.UValue != null) structure.USector.UValue.UPopulation.Remove (structure);
            // Destroy docked structures
            structure.UDockingPoints.ForEach (aPoint => DisposeStructure (aPoint.UDocker));
            // Destroy equipment
            structure.UEquipment.UAll.ForEach(aSlot => aSlot.ChangeEquipment(null));
            // Destroy structure game object
            Destroy (structure.gameObject);
        }

        public void SaveGame (DirectoryInfo directory) {
            FileInfo file = PathManager.GetStructureFile (directory);
            if (!file.Exists) file.Create ().Close ();
            File.WriteAllText (file.FullName, JsonConvert.SerializeObject (registry.UStructures, Formatting.Indented, new Structure.Converter ()));
        }

        public void LoadGame (DirectoryInfo directory) {
            FileInfo file = PathManager.GetStructureFile (directory);
            if (!file.Exists) return;
            registry.UStructures.ForEach (DisposeStructure);
            registry.Clear ();
            JsonConvert.DeserializeObject<List<Structure>> (File.ReadAllText (file.FullName), new Structure.Converter ())!.ForEach (aStructure => registry.Add (aStructure));
        }
    }
}
