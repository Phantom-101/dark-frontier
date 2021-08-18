using DarkFrontier.Foundation.Behaviors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;

namespace DarkFrontier.Structures {
    public class StructureManager : ComponentBehavior {
        [SerializeReference] private StructureRegistry registry;

        private readonly BehaviorTicker ticker = new BehaviorTicker ();
        private readonly BehaviorTicker lateTicker = new BehaviorTicker ();
        private readonly BehaviorTicker fixedTicker = new BehaviorTicker ();

        [Inject]
        public void Construct (StructureRegistry registry) {
            this.registry = registry;
        }

        protected override void PropagateTick (float dt, float? edt = null) => ticker.Tick (dt, edt);
        protected override void PropagateLateTick (float dt, float? edt = null) => lateTicker.Tick (dt, edt);
        protected override void PropagateFixedTick (float dt, float? edt = null) => fixedTicker.Tick (dt, edt);

        protected override void SubscribeEventListeners () {
            ticker.Notifier += TickStructures;
            lateTicker.Notifier += LateTickStructures;
            fixedTicker.Notifier += FixedTickStructures;
        }

        protected override void UnsubscribeEventListeners () {
            ticker.Notifier -= TickStructures;
            lateTicker.Notifier -= LateTickStructures;
            fixedTicker.Notifier -= FixedTickStructures;
        }

        private void TickStructures (object sender, EventArgs args) => registry.Structures.ForEach (e => e.Tick (GetTickArgs (args).dt, GetTickArgs (args).edt));
        private void LateTickStructures (object sender, EventArgs args) => registry.Structures.ForEach (e => e.LateTick (GetTickArgs (args).dt, GetTickArgs (args).edt));
        private void FixedTickStructures (object sender, EventArgs args) => registry.Structures.ForEach (e => e.FixedTick (GetTickArgs (args).dt, GetTickArgs (args).edt));
        private BehaviorTicker.BehaviorTickArgs GetTickArgs (EventArgs args) => args as BehaviorTicker.BehaviorTickArgs;

        public Structure GetStructure (string id) => registry.Structures.Find (e => e.Id == id);

        public bool Detects (Structure a, Structure b) {
            if (a.Sector != b.Sector) return false;

            float sqrDis = (a.transform.localPosition - b.transform.localPosition).sqrMagnitude;
            float range = a.Stats.GetAppliedValue (StatNames.SensorStrength, 0) * b.Stats.GetAppliedValue (StatNames.Detectability, 0);
            float sqrRange = range * range;
            return sqrDis <= sqrRange;
        }

        public HashSet<Structure> GetDetected (Structure structure) {
            List<Structure> inSector = structure.Sector.Value (SectorManager.Instance.GetSector).Population;
            HashSet<Structure> ret = new HashSet<Structure> ();
            foreach (Structure candidate in inSector)
                if (candidate != structure && Detects (structure, candidate))
                    ret.Add (candidate);
            return ret;
        }

        public Structure SpawnStructure (StructureSO profile, Faction owner, Sector sector, Location location) {
            GameObject go = Instantiate (profile.Prefab, sector.transform);
            go.name = profile.Name;
            go.transform.localPosition = location.GetPosition ();
            Structure structure = go.GetComponent<Structure> ();
            structure.Faction.Id.Value = owner.Id;
            structure.Sector.Id.Value = sector.Id;
            return structure;
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
            structures.ForEach (data => {
                StructureSO profile = ItemManager.Instance.GetItem (data.ProfileId) as StructureSO;
                GameObject structure = Instantiate (profile.Prefab, SectorManager.Instance.GetSector (data.SectorId).transform);
                structure.name = profile.Name;
                Structure comp = structure.GetComponent<Structure> ();
                comp.SetSaveData (data);
                if (data.IsPlayer) PlayerController.Instance.Player = comp;
            });
        }
    }
}
