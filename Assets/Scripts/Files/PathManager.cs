using System.IO;
using System.Linq;
using UnityEngine;

namespace DarkFrontier.Files {
    public class PathManager {
        public static DirectoryInfo GetUniversesDirectory () {
            return new DirectoryInfo ($"{Application.persistentDataPath}/saves");
        }

        public static string[] GetUniverseNames () {
            return Directory.GetDirectories ($"{Application.persistentDataPath}/saves");
        }

        public static DirectoryInfo[] GetUniverseDirectories () {
            return GetUniverseNames ().ToList ().ConvertAll (e => new DirectoryInfo (e)).ToArray ();
        }

        public static DirectoryInfo GetUniverseDirectory (string name) {
            return new DirectoryInfo ($"{Application.persistentDataPath}/saves/{name}");
        }

        public static string[] GetSaveNames (string universe) {
            return Directory.GetDirectories ($"{Application.persistentDataPath}/saves/{universe}");
        }

        public static DirectoryInfo[] GetSaveDirectories (string universe) {
            return GetSaveNames (universe).ToList ().ConvertAll (e => new DirectoryInfo (e)).ToArray ();
        }

        public static string[] GetSaveNames (DirectoryInfo universe) {
            return Directory.GetDirectories (universe.FullName);
        }

        public static DirectoryInfo[] GetSaveDirectories (DirectoryInfo universe) {
            return GetSaveNames (universe).ToList ().ConvertAll (e => new DirectoryInfo (e)).ToArray ();
        }

        public static DirectoryInfo GetSaveDirectory (string universe, string name) {
            return new DirectoryInfo ($"{Application.persistentDataPath}/saves/{universe}/{name}");
        }

        public static DirectoryInfo GetSaveDirectory (DirectoryInfo universe, string name) {
            return new DirectoryInfo ($"{universe.FullName}/{name}");
        }

        public static FileInfo GetStructureFile (string universe, string save) {
            return new FileInfo ($"{GetSaveDirectory (universe, save)}/structures.txt");
        }

        public static FileInfo GetStructureFile (DirectoryInfo universe, string save) {
            return new FileInfo ($"{GetSaveDirectory (universe, save)}/structures.txt");
        }

        public static FileInfo GetStructureFile (DirectoryInfo save) {
            return new FileInfo ($"{save.FullName}/structures.txt");
        }

        public static FileInfo GetFactionFile (string universe, string save) {
            return new FileInfo ($"{GetSaveDirectory (universe, save)}/factions.txt");
        }

        public static FileInfo GetFactionFile (DirectoryInfo universe, string save) {
            return new FileInfo ($"{GetSaveDirectory (universe, save)}/factions.txt");
        }

        public static FileInfo GetFactionFile (DirectoryInfo save) {
            return new FileInfo ($"{save.FullName}/factions.txt");
        }

        public static FileInfo GetSectorFile (string universe, string save) {
            return new FileInfo ($"{GetSaveDirectory (universe, save)}/sectors.txt");
        }

        public static FileInfo GetSectorFile (DirectoryInfo universe, string save) {
            return new FileInfo ($"{GetSaveDirectory (universe, save)}/sectors.txt");
        }

        public static FileInfo GetSectorFile (DirectoryInfo save) {
            return new FileInfo ($"{save.FullName}/sectors.txt");
        }
    }
}
