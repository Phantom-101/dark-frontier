using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Locations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DarkFrontier.Structures {
    public class StructureManager : ComponentBehavior {
        public StructureRegistry Registry { get => registry; }
        [SerializeReference] private StructureRegistry registry = new StructureRegistry ();

        private readonly BehaviorTicker ticker = new BehaviorTicker ();
        private readonly BehaviorTicker lateTicker = new BehaviorTicker ();
        private readonly BehaviorTicker fixedTicker = new BehaviorTicker ();

        private readonly SectorManager sectorManager = Singletons.Get<SectorManager> ();

        protected override void PropagateTick (float dt) => ticker.Tick (dt);
        protected override void PropagateLateTick (float dt) => lateTicker.Tick (dt);
        protected override void PropagateFixedTick (float dt) => fixedTicker.Tick (dt);

        protected override void InternalSubscribeEventListeners () {
            ticker.Notifier += TickStructures;
            lateTicker.Notifier += LateTickStructures;
            fixedTicker.Notifier += FixedTickStructures;
        }

        protected override void InternalUnsubscribeEventListeners () {
            ticker.Notifier -= TickStructures;
            lateTicker.Notifier -= LateTickStructures;
            fixedTicker.Notifier -= FixedTickStructures;
        }

        private void TickStructures (object sender, EventArgs args) => registry.Structures.ForEach (e => e.Tick (GetTickArgs (args).dt));
        private void LateTickStructures (object sender, EventArgs args) => registry.Structures.ForEach (e => e.LateTick (GetTickArgs (args).dt));
        private void FixedTickStructures (object sender, EventArgs args) => registry.Structures.ForEach (e => e.FixedTick (GetTickArgs (args).dt));
        private BehaviorTicker.BehaviorTickArgs GetTickArgs (EventArgs args) => args as BehaviorTicker.BehaviorTickArgs;

        public Structure GetStructure (string id) => registry.Structures.Find (e => e.Id == id);

        public bool Detects (Structure detector, Structure detectee) {
            if (detector.Sector.Id.Value != detectee.Sector.Id.Value) return false;

            float sqrDis = (detector.transform.localPosition - detectee.transform.localPosition).sqrMagnitude;
            float range = detector.Stats.GetAppliedValue (StatNames.SensorStrength, 0) * detectee.Stats.GetAppliedValue (StatNames.Detectability, 0);
            float sqrRange = range * range;
            return sqrDis <= sqrRange;
        }

        public HashSet<Structure> GetDetected (Structure detector) {
            List<Structure> inSector = detector.Sector.Value.Population.Structures;
            HashSet<Structure> ret = new HashSet<Structure> ();
            foreach (Structure detectee in inSector)
                if (detectee != detector && Detects (detector, detectee))
                    ret.Add (detectee);
            return ret;
        }

        public Structure SpawnStructure (StructureSO profile, string faction, string sector, Location location) {
            Structure spawned = Instantiate (profile.Prefab, location.Position, Quaternion.identity).GetComponent<Structure> ();
            spawned.Faction.Id.Value = faction;
            spawned.Sector.Id.Value = sector;
            if (spawned.Sector.Value != null) {
                spawned.transform.SetParent (spawned.Sector.Value.transform);
            }
            return spawned;
        }

        public void DestroyStructure (Structure structure) {
            // Destroy docked structures
            structure.DockingBays.Dockers.ForEach (e => DestroyStructure (e));
            // Spawn destruction effect
            if (structure.Profile.DestructionEffect != null) {
                GameObject effect = Instantiate (structure.Profile.DestructionEffect, structure.transform.parent);
                effect.transform.localPosition = structure.transform.localPosition;
                effect.transform.localScale = Vector3.one * structure.Profile.ApparentSize;
            }
            // TODO Drop stuff according to StructureSO.DropPercentage
            // Destroy structure game object
            Destroy (structure.gameObject);
        }

        public void DisposeStructure (Structure structure) {
            // Destroy docked structures
            structure.DockingBays.Dockers.ForEach (e => DisposeStructure (e));
            // Destroy structure game object
            Destroy (structure.gameObject);
        }

        public void SaveGame (DirectoryInfo directory) {
            List<StructureSaveData> saveData = new List<StructureSaveData> ();
            registry.Structures.ForEach (structure => { saveData.Add (structure.GetSaveData ()); });
            FileInfo file = PathManager.GetStructureFile (directory);
            if (!file.Exists) file.Create ().Close ();
            File.WriteAllText (
                file.FullName,
                JsonConvert.SerializeObject (
                    saveData,
                    Formatting.Indented,
                    new JsonSerializerSettings {
                        TypeNameHandling = TypeNameHandling.All,
                    }
                )
            );
        }

        public void LoadGame (DirectoryInfo directory) {
            FileInfo file = PathManager.GetStructureFile (directory);
            if (!file.Exists) return;
            List<StructureSaveData> structures = JsonConvert.DeserializeObject (
                File.ReadAllText (file.FullName),
                new JsonSerializerSettings {
                    TypeNameHandling = TypeNameHandling.All,
                }
            ) as List<StructureSaveData>;
            registry.Structures.ForEach (structure => DisposeStructure (structure));
            registry.Clear ();
            structures.ForEach (data => {
                StructureSO profile = ItemManager.Instance.GetItem (data.ProfileId) as StructureSO;
                GameObject structure = Instantiate (profile.Prefab, sectorManager.Registry.Find (data.SectorId).transform);
                structure.name = profile.Name;
                Structure comp = structure.GetComponent<Structure> ();
                comp.SetSaveData (data);
                if (data.IsPlayer) PlayerController.Instance.Player = comp;
            });
        }
    }
}
