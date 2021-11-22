using DarkFrontier.Factions;
using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Locations;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.Installers {
    public class BaseInstaller : Installer {
        public override void InstallBindings () {
            if (!Singletons.Exists<BehaviorTimekeeper> ()) {
                var lGameObject = new GameObject ("[Service] Behavior Timekeeper");
                Singletons.Bind (lGameObject.AddComponent<BehaviorTimekeeper> ());
                DontDestroyOnLoad (lGameObject);
            }

            if (!Singletons.Exists<BehaviorManager> ()) {
                Singletons.Bind (new BehaviorManager ());
            }
            
            if (!Singletons.Exists<NavigationManager> ()) {
                Singletons.Bind (new NavigationManager ());
            }

            if (!Singletons.Exists<SectorManager> ()) {
                var lGameObject = new GameObject ("[Service] Sector Manager");
                Singletons.Bind (lGameObject.AddComponent<SectorManager> ());
                DontDestroyOnLoad (lGameObject);
            }

            if (!Singletons.Exists<FactionManager> ()) {
                var lGameObject = new GameObject ("[Service] Faction Manager");
                Singletons.Bind (lGameObject.AddComponent<FactionManager> ());
                DontDestroyOnLoad (lGameObject);
            }

            if (!Singletons.Exists<StructureManager> ()) {
                var lGameObject = new GameObject ("[Service] Structure Manager");
                Singletons.Bind (lGameObject.AddComponent<StructureManager> ());
                DontDestroyOnLoad (lGameObject);
            }
        }
    }
}