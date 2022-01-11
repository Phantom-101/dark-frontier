using System.Collections.Generic;
using System.IO;
using DarkFrontier.Files;
using DarkFrontier.Foundation.Behaviors;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Structures
{
    public class StructureManager : ComponentBehavior,
        ICtorArgs<(BehaviorTimekeeper, StructureRegistry, StructureLifetimeUtilities, StructureSerializationUtilities)>
    {
        [field: SerializeReference]
        public StructureRegistry Registry { get; private set; }

        public StructureLifetimeUtilities LifetimeUtilities { get; private set; }

        public StructureSerializationUtilities SerializationUtilities { get; private set; }

        private Ticker _ticker;
        private Ticker _fixedTicker;

        private BehaviorTimekeeper _timekeeper;

        public void Construct(
            (BehaviorTimekeeper, StructureRegistry, StructureLifetimeUtilities, StructureSerializationUtilities) args)
        {
            var (timekeeper, registry, lifetimeUtilities, serializationUtilities) = args;
            _timekeeper = timekeeper;
            Registry = registry;
            LifetimeUtilities = lifetimeUtilities;
            SerializationUtilities = serializationUtilities;
        }

        public override void Initialize()
        {
            _ticker = new Ticker();
            _fixedTicker = new Ticker();

            _ticker.Notifier += TickStructures;
            _fixedTicker.Notifier += FixedTickStructures;
        }

        public override void Enable()
        {
            _timekeeper.Subscribe(this);
        }

        public override void Disable()
        {
            _timekeeper.Unsubscribe(this);
        }

        public override void Tick(object aTicker, float aDt)
        {
            _ticker.Tick(this, aDt);
        }

        public override void FixedTick(object aTicker, float aDt)
        {
            _fixedTicker.Tick(this, aDt);
        }

        private void TickStructures(object aSender, float aArgs)
        {
            var lStructures = Registry.UStructures;
            var lLength = lStructures.Count;
            for (var lIndex = 0; lIndex < lLength; lIndex++)
            {
                lStructures[lIndex].Tick(this, aArgs);
            }
        }

        private void FixedTickStructures(object aSender, float aArgs)
        {
            var lStructures = Registry.UStructures;
            var lLength = lStructures.Count;
            for (var lIndex = 0; lIndex < lLength; lIndex++)
            {
                lStructures[lIndex].FixedTick(this, aArgs);
            }
        }

        public Structure GetStructure(string aId)
        {
            var lStructures = Registry.UStructures;
            var lLength = lStructures.Count;
            for (var lIndex = 0; lIndex < lLength; lIndex++)
            {
                if (lStructures[lIndex].uId == aId)
                {
                    return lStructures[lIndex];
                }
            }

            return null;
        }

        public void SaveGame(DirectoryInfo directory)
        {
            FileInfo file = PathManager.GetStructureFile(directory);
            if (!file.Exists) file.Create().Close();
            File.WriteAllText(file.FullName,
                JsonConvert.SerializeObject(Registry.UStructures, Formatting.Indented, new Structure.Converter()));
        }

        public void LoadGame(DirectoryInfo directory)
        {
            FileInfo file = PathManager.GetStructureFile(directory);
            if (!file.Exists) return;
            Registry.UStructures.ForEach(LifetimeUtilities.Dispose);
            Registry.Clear();
            JsonConvert.DeserializeObject<List<Structure>>(File.ReadAllText(file.FullName), new Structure.Converter())!
                .ForEach(aStructure => Registry.Add(aStructure));
        }
    }
}