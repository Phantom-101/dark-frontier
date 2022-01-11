using DarkFrontier.Controllers;
using DarkFrontier.Factions;
using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Locations;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.Installers
{
    public class GameInstaller : Installer
    {
        public override void InstallBindings()
        {
            if (!Singletons.Exists<SectorManager>())
            {
                var instantiatedGameObject = new GameObject("[Service] Sector Manager");
                Singletons.Bind(instantiatedGameObject.AddComponent<SectorManager>());
                DontDestroyOnLoad(instantiatedGameObject);
            }

            if (!Singletons.Exists<FactionManager>())
            {
                var instantiatedGameObject = new GameObject("[Service] Faction Manager");
                Singletons.Bind(instantiatedGameObject.AddComponent<FactionManager>());
                DontDestroyOnLoad(instantiatedGameObject);
            }

            if (!Singletons.Exists<StructureManager>())
            {
                var instantiatedGameObject = new GameObject("[Service] Structure Manager");
                var instantiatedComponent = instantiatedGameObject.AddComponent<StructureManager>();
                var registry = new StructureRegistry();
                var lifetimeUtilities = new StructureLifetimeUtilities(Singletons.Get<BehaviorManager>(), registry);
                var serializationUtilities = new StructureSerializationUtilities(registry, lifetimeUtilities);
                instantiatedComponent.Construct((Singletons.Get<BehaviorTimekeeper>(), registry, lifetimeUtilities,
                    serializationUtilities));
                Singletons.Bind(instantiatedComponent);
                DontDestroyOnLoad(instantiatedGameObject);
            }

            if (!Singletons.Exists<PlayerController>())
            {
                var instantiatedGameObject = new GameObject("[Service] Player Controller");
                Singletons.Bind(instantiatedGameObject.AddComponent<PlayerController>());
                DontDestroyOnLoad(instantiatedGameObject);
            }
        }
    }
}