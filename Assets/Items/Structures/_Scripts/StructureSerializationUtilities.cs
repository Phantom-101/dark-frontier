using System.Collections.Generic;
using System.IO;
using DarkFrontier.Files;
using Newtonsoft.Json;

namespace DarkFrontier.Structures
{
    public class StructureSerializationUtilities
    {
        private readonly StructureRegistryOld _registry;
        private readonly StructureLifetimeUtilities _lifetimeUtilities;

        public StructureSerializationUtilities(StructureRegistryOld registry, StructureLifetimeUtilities lifetimeUtilities)
        {
            _registry = registry;
            _lifetimeUtilities = lifetimeUtilities;
        }

        public void Serialize(DirectoryInfo directory)
        {
            var file = PathManager.GetStructureFile(directory);
            using var stream = file.Exists ? file.OpenWrite() : file.Create();
            using var writer = new StreamWriter(stream);

            stream.SetLength(0);
            writer.Write(JsonConvert.SerializeObject(_registry.UStructures, Formatting.Indented,
                new Structure.Converter()));
        }

        public void Deserialize(DirectoryInfo directory)
        {
            var file = PathManager.GetStructureFile(directory);
            using var stream = file.Exists ? file.OpenRead() : file.Create();
            using var reader = new StreamReader(stream);

            for (int i = 0, l = _registry.UStructures.Count; i < l; i++)
            {
                _lifetimeUtilities.Dispose(_registry.UStructures[i]);
            }

            _registry.Clear();

            var deserialized =
                JsonConvert.DeserializeObject<List<Structure>>(reader.ReadToEnd(), new Structure.Converter());
            for (int i = 0, l = deserialized!.Count; i < l; i++)
            {
                _registry.Add(deserialized[i]);
            }
        }
    }
}