using UnityEngine;
using UnityEngine.SceneManagement;

#nullable enable
namespace DarkFrontier.Positioning.Sectors
{
    public class SectorSpawner
    {
        public static SectorComponent Create(SectorInstance instance)
        {
            var scene = SceneManager.CreateScene(instance.Name, new CreateSceneParameters(LocalPhysicsMode.Physics3D));
            
            var instantiated = new GameObject($"{instance.Name} (SectorComponent)");
            SceneManager.MoveGameObjectToScene(instantiated, scene);
            
            var sector = instantiated.AddComponent<SectorComponent>();
            sector.instance = instance;
            
            return sector;
        }

        public static Scene Create(SectorComponent component)
        {
            var scene = SceneManager.CreateScene(component.instance?.Name ?? "Unknown Sector", new CreateSceneParameters(LocalPhysicsMode.Physics3D));

            SceneManager.MoveGameObjectToScene(component.gameObject, scene);

            return scene;
        }
    }
}
#nullable restore