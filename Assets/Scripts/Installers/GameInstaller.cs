using DarkFrontier.Factions;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Locations;
using DarkFrontier.Structures;
using UnityEngine;

public class GameInstaller : Installer {
    public override void InstallBindings () {
        if (!Singletons.Exists<SectorManager> ()) {
            GameObject obj = new GameObject ("[Service] Sector Manager");
            Singletons.Bind<SectorManager> (obj.AddComponent<SectorManager> ());
            DontDestroyOnLoad (obj);
        }

        if (!Singletons.Exists<FactionManager> ()) {
            GameObject obj = new GameObject ("[Service] Faction Manager");
            Singletons.Bind<FactionManager> (obj.AddComponent<FactionManager> ());
            DontDestroyOnLoad (obj);
        }

        if (!Singletons.Exists<StructureManager> ()) {
            GameObject obj = new GameObject ("[Service] Structure Manager");
            Singletons.Bind<StructureManager> (obj.AddComponent<StructureManager> ());
            DontDestroyOnLoad (obj);
        }
    }
}
