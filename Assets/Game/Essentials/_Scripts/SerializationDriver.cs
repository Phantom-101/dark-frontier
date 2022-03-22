#nullable enable
using System.IO;

namespace DarkFrontier.Game.Essentials
{
    public class SerializationDriver
    {
        public DirectoryInfo? saveDirectory;

        public void Serialize(string universeName)
        {
            // TODO handle serialization into file
        }

        public void Deserialize()
        {
            if(saveDirectory is not { Exists: true }) return;
            
            // TODO read from files and create

            saveDirectory = null;
        }
    }
}